using Blogs.AppServices.AppServices.implement;
using Blogs.AppServices.AppServices.Interface;
using Blogs.AppServices.CommandHandlers.Admin;
using Blogs.AppServices.QueryHandlers.Admin;
using Blogs.Core;
using Blogs.Core.Config;
using Blogs.Domain.EventBus;
using Blogs.Domain.EventNotices;
using Blogs.Domain.IRepositorys.Admin;
using Blogs.Domain.IRepositorys.Blogs;
using Blogs.Domain.IServices;
using Blogs.Domain.Notices;
using Blogs.Infrastructure;
using Blogs.Infrastructure.Context;
using Blogs.Infrastructure.OpenIdDict;
using Blogs.Infrastructure.Repositorys.Admin;
using Blogs.Infrastructure.Repositorys.Blogs;
using Blogs.Infrastructure.Services.Admin;
using Blogs.Infrastructure.Services.App;
using Blogs.WebApi.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 配置初始化
AppConfig.Init(builder.Services, builder.Configuration);

// 获取环境变量
var environment = builder.Environment.EnvironmentName;
Console.WriteLine($"当前环境: {environment}");
Console.WriteLine($"ContentRootPath: {builder.Environment.ContentRootPath}");

// 加载环境特定的配置文件
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region 配置Swagger并开启认证
builder.Services.AddSwaggerGen(options =>
{
    // 为App接口创建文档
    options.SwaggerDoc("app", new OpenApiInfo
    {
        Title = "App API",
        Version = "v1",
        Description = "移动端应用程序接口"
    });

    // 为Admin后端接口创建文档
    options.SwaggerDoc("admin", new OpenApiInfo
    {
        Title = "Admin API",
        Version = "v1",
        Description = "后台管理系统接口"
    });

    // 添加JWT安全方案
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT 认证请求头. 格式: 'Bearer <token>'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    // 为两个文档都添加安全要求
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });

    // 配置文档筛选器，根据路由自动分组
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        // 获取控制器路由模板
        var attribute = apiDesc.ActionDescriptor.EndpointMetadata
            .OfType<RouteAttribute>()
            .FirstOrDefault();

        if (attribute != null)
        {
            var template = attribute.Template ?? "";
            if (docName == "app" && template.StartsWith("api/app"))
                return true;
            if (docName == "admin" && template.StartsWith("api/admin"))
                return true;
        }

        return false;
    });

    // 添加XML注释
    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    var xmlFile = $"{assemblyName}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, true);
    }
});

#endregion

// 注册HttpContext访问器
builder.Services.AddHttpContextAccessor();

#region 注册Redis连接
var redisConnection = AppConfig.GetSettingString("ConnectionStrings:RedisConnection");
if (!string.IsNullOrEmpty(redisConnection))
{
    Console.WriteLine($"Redis连接字符串: {redisConnection}");
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        ConnectionMultiplexer.Connect(redisConnection));
}
else
{
    Console.WriteLine("警告: Redis连接字符串未配置");
}
#endregion

#region 注册仓储
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

var domainAssembly = typeof(IUserRepository).Assembly;
var infrastructureAssembly = typeof(UserRepository).Assembly;

builder.Services.Scan(scan => scan
    .FromAssemblies(infrastructureAssembly)
    .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
#endregion

#region 注册事件总线处理程序
builder.Services.AddSingleton<IMediatorHandler, MediatorHandler>();
#endregion

#region 批量注册所有Command和Query Handler
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AdminUserQueryHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(AppUserCommandHandler).Assembly);
});
#endregion

#region 注册领域通知处理器
builder.Services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
builder.Services.AddScoped<DomainNotificationHandler>();
builder.Services.AddScoped<GlobalExceptionFilter>();
#endregion

#region 集成OpenIdDict

// 注册自定义服务
builder.Services.AddScoped<IOpenIddictService, OpenIddictService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddScoped<IAppOpenIddictService, AppOpenIddictService>();
builder.Services.AddScoped<IAppAuthService, AppAuthService>();

// 配置EF Core上下文
builder.Services.AddDbContext<OpenIddictDbContext>(options =>
{
    var connectionString = AppConfig.GetSettingString("ConnectionStrings:MySqlConnectionWrite");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("数据库连接字符串未配置");
    }
    Console.WriteLine($"数据库连接字符串: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

    // 启用更详细的错误信息和敏感数据记录（仅开发环境）
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// 配置OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<OpenIddictDbContext>();
    })
    .AddServer(options =>
    {
        // 启用授权码、密码、刷新令牌和客户端凭证流程
        options.AllowAuthorizationCodeFlow()
               .AllowPasswordFlow()
               .AllowRefreshTokenFlow()
               .AllowClientCredentialsFlow();

        // 设置令牌端点
        options.SetTokenEndpointUris("/connect/token");
        options.SetAuthorizationEndpointUris("/connect/authorize");
        options.SetUserInfoEndpointUris("/connect/userinfo");

        // 注册范围
        options.RegisterScopes(
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Roles,
            OpenIddictConstants.Scopes.OfflineAccess);

        // 注册声明
        options.RegisterClaims(OpenIddictConstants.Claims.Subject);

        // 加密和签名配置
        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine("开发环境：使用临时密钥");
            options.AddEphemeralEncryptionKey()
                   .AddEphemeralSigningKey();
        }
        else if (builder.Environment.IsProduction())
        {
            Console.WriteLine("生产环境：配置证书");

            // 从配置读取证书路径
            var certPath = builder.Configuration["OpenIddict:CertificatePath"] ??
                          "/app/certs/encryption-certificate.pfx";
            var certPassword = builder.Configuration["OpenIddict:CertificatePassword"];

            Console.WriteLine($"证书路径: {certPath}");

            if (File.Exists(certPath))
            {
                try
                {
                    var certificate = new X509Certificate2(certPath, certPassword ?? string.Empty,
                        X509KeyStorageFlags.EphemeralKeySet);

                    options.AddEncryptionCertificate(certificate);
                    options.AddSigningCertificate(certificate);

                    Console.WriteLine($"证书加载成功！主题: {certificate.Subject}");
                    Console.WriteLine($"证书指纹: {certificate.Thumbprint}");
                    Console.WriteLine($"证书有效期: {certificate.NotBefore:yyyy-MM-dd} 到 {certificate.NotAfter:yyyy-MM-dd}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"警告: 证书加载失败，使用临时密钥（生产环境不推荐）: {ex.Message}");
                    options.AddEphemeralEncryptionKey()
                           .AddEphemeralSigningKey();
                }
            }
            else
            {
                Console.WriteLine($"警告: 证书文件不存在 ({certPath})，使用临时密钥（生产环境不推荐）");
                options.AddEphemeralEncryptionKey()
                       .AddEphemeralSigningKey();
            }
        }

        // 配置令牌生命周期
        options.SetAccessTokenLifetime(TimeSpan.FromHours(1));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

        // 使用ASP.NET Core集成
        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough()
               .EnableAuthorizationEndpointPassthrough()
               .EnableUserInfoEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

// 配置认证
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtConfig = AppConfig.GetConfigModel<JwtConfig>("JwtConfig");
    if (jwtConfig == null)
    {
        throw new InvalidOperationException("JWT配置未找到");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var openIddictService = context.HttpContext.RequestServices.GetRequiredService<IOpenIddictService>();

            var token = context.HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?
                .Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                context.Fail("Token is not valid");
                return;
            }

            var exists = await openIddictService.ValidateJwtToken(token);
            if (!exists)
            {
                context.Fail("Token has been revoked");
            }
        }
    };
});

// 配置授权
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api");
    });
});

#endregion

#region 日志配置
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "Logs/.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        fileSizeLimitBytes: 10 * 1024 * 1024,
        rollOnFileSizeLimit: true,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        encoding: Encoding.UTF8
    )
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

#region CORS跨域配置
var corsConfig = AppConfig.GetSection("CorsConfig")?.Value;
if (!string.IsNullOrEmpty(corsConfig))
{
    var origins = corsConfig.Split(';', StringSplitOptions.RemoveEmptyEntries);
    Console.WriteLine($"CORS允许的源: {string.Join(", ", origins)}");

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("WebSiteCors", builder =>
        {
            builder.WithOrigins(origins)
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
    });
    Console.WriteLine("已开启CORS");
}
else
{
    Console.WriteLine("警告: CORS配置未找到");
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("WebSiteCors", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });
}
#endregion

#region 文件存储
builder.Services.AddScoped<IAppFileService, AppFileService>();

// 配置请求体大小限制
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024; // 1GB
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 1024 * 1024 * 1024;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});
#endregion

// 添加健康检查
builder.Services.AddHealthChecks();

var app = builder.Build();

#region OpenIdDict初始化
using (var scope = app.Services.CreateScope())
{
    try
    {
        // 确保数据库已创建
        var dbContext = scope.ServiceProvider.GetRequiredService<OpenIddictDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        // 初始化CurrentUser
        var openIddictService = scope.ServiceProvider.GetRequiredService<IOpenIddictService>();
        CurrentUser.SetProvider(openIddictService);

        // 初始化AppUser
        var appOpenIddictService = scope.ServiceProvider.GetRequiredService<IAppOpenIddictService>();
        CurrentAppUser.SetProvider(appOpenIddictService);

    }
    catch (Exception ex)
    {
        Console.WriteLine($"OpenIdDict初始化失败: {ex.Message}");
    }
}
#endregion

// 添加自定义静态文件路径映射
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
    Console.WriteLine($"创建上传目录: {uploadsPath}");
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Uploads"
});

// 开发环境启用Swagger
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/app/swagger.json", "App API v1");
    options.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin API v1");
    options.RoutePrefix = "swagger";
});
//}

// 启用CORS
app.UseCors("WebSiteCors");

// 启用认证和授权中间件
app.UseAuthentication();
app.UseAuthorization();

// 注册统一响应处理中间件
app.UseMiddleware<UnifiedResponseMiddleware>();

app.MapControllers();

// 映射健康检查终结点
app.MapHealthChecks("/health");

// 映射OpenIddict端点
//app.MapOpenIddict(); 

app.Run();