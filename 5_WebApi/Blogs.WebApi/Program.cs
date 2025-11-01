using OpenIddict.Validation.AspNetCore; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
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
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 配置初始化
AppConfig.Init(builder.Services, builder.Configuration);


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

    // 添加XML注释（两个文档共享）
    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    var xmlFile = $"{assemblyName}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, true);
    }
    else
    {
        Console.WriteLine($"XML文档文件未找到: {xmlPath}");
    }
});

#endregion

// 注册HttpContext访问器
builder.Services.AddHttpContextAccessor();

#region 注册Redis连接
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = AppConfig.GetSettingString("ConnectionStrings:RedisConnection");
    return ConnectionMultiplexer.Connect(configuration);
});
#endregion
 
#region JWT配置
var jwtConfig = AppConfig.GetConfigModel<JwtConfig>("JwtConfig");
Console.WriteLine("JwtConfig=================================================");
Console.WriteLine(JsonConvert.SerializeObject(jwtConfig));

// 注册JWT服务
//builder.Services.AddScoped<IJwtService, JwtService>();
#endregion

#region 注册仓储
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IMenuRepository, MenuRepository>();
//builder.Services.AddScoped<IRoleRepository, RoleRepository>();
//builder.Services.AddScoped<IPermissionsRepository, PermissionsRepository>();
//builder.Services.AddScoped<ISysLogRepository, SysLogRepository>();
//builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

var domainAssembly = typeof(IUserRepository).Assembly;
var infrastructureAssembly = typeof(UserRepository).Assembly;

builder.Services.Scan(scan => scan
    .FromAssemblies(infrastructureAssembly) // 从实现层扫描
    .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

#endregion

#region 注册事件总线处理程序
builder.Services.AddSingleton<IMediatorHandler, MediatorHandler>();
#endregion

#region 批量注册所有Command和Query Handler
// 方法1: 使用MediatR自动注册（推荐）
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AdminUserQueryHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(AppUserCommandHandler).Assembly);
    //cfg.RegisterServicesFromAssembly(typeof(RoleQueryHandler).Assembly);
});
 
#endregion

#region 数据迁移-表结构生成

// 初始化数据库上下文
var dbContext = new SqlSugarDbContext();
//dbContext.InitTables();
#endregion

#region 注册领域通知处理器
builder.Services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
builder.Services.AddScoped<DomainNotificationHandler>();

// 注册过滤器需要的服务
builder.Services.AddScoped<GlobalExceptionFilter>();
#endregion

//builder.WebHost.UseUrls("http://0.0.0.0:8080");
//builder.WebHost.UseUrls("https://0.0.0.0:8088");

#region 集成OpenidDict


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

        // 设置授权端点（如果需要授权码流程）
        options.SetAuthorizationEndpointUris("/connect/authorize");

        // 设置用户信息端点
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
            options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();
        }
        else
        {
            // 生产环境使用真实的证书
            // options.AddEncryptionCertificate(certificate)
            //        .AddSigningCertificate(certificate);
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
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:SecurityKey"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var _openiddictService = context.HttpContext.RequestServices.GetRequiredService<IOpenIddictService>();

            Console.WriteLine("==================================================================");
            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);

            var token = context.HttpContext.Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", "");
            //var token =  context.SecurityToken as JwtSecurityToken;
            if (token == null)
            {
                context.Fail("Token is not valid");
                return;
            }

            var exists = await _openiddictService.ValidateJwtToken(token);

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

// 配置 Redis
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConnectionString))
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(
        ConnectionMultiplexer.Connect(redisConnectionString));
}

#endregion



#region 日志配置
// 配置 Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // 设置全局最小日志级别
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) 
    .Enrich.FromLogContext() // 从日志上下文中丰富属性
    .WriteTo.Console() // 同时输出到控制台
    .WriteTo.File(
        path: "Logs/.txt", // 日志文件路径和名称模式
        rollingInterval: RollingInterval.Day, // 按天分割文件
        retainedFileCountLimit: 30, // 保留最近30天的日志文件
        fileSizeLimitBytes: 10 * 1024 * 1024, // 单个文件最大10MB
        rollOnFileSizeLimit: true, // 在文件达到大小限制后也滚动
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        encoding: System.Text.Encoding.UTF8
    )
    .CreateLogger();
builder.Host.UseSerilog();
#endregion

#region CORS跨域配置
builder.Services.AddCors(options =>
{
    options.AddPolicy("WebSiteCors", builder =>
    {
        var corsUrls = AppConfig.GetSection("CorsConfig").Value;
        var origins = corsUrls.Split(';', StringSplitOptions.RemoveEmptyEntries);

        builder.WithOrigins(origins)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // 如果前端需要发送cookies等凭证
    });
});

#endregion

#region 文件存储
builder.Services.AddScoped<IAppFileService, AppFileService>();
// 配置 Kestrel 服务器请求体大小限制（针对自宿主情况）
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024; // 例如：设置为 1GB
});
// 配置 IIS 服务器选项（针对部署到 IIS 的情况）
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 1024 * 1024 * 1024; // 例如：设置为 1GB
});
// 配置表单选项，解除 Multipart  Body 长度限制
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue; // 取消限制，或设置为一个很大的值
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

#endregion

var app = builder.Build();

#region Openiddict初始化


// 初始化CurrentUser静态类
using (var scope = app.Services.CreateScope())
{
    //管理端
    var openIddictService = scope.ServiceProvider.GetRequiredService<IOpenIddictService>();
    CurrentUser.SetProvider(openIddictService);

    //App端
    var appOpeniddictService = scope.ServiceProvider.GetRequiredService<IAppOpenIddictService>();
    CurrentAppUser.SetProvider(appOpeniddictService);
}
#endregion

app.UseStaticFiles();
// 添加自定义静态文件路径映射
app.UseStaticFileMappings(
   ("Uploads", "ArticleFiles"),
   ("Uploads", "UserPhoto"),
   ("Uploads", "WebsiteImage")
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        // App接口文档
        options.SwaggerEndpoint("/swagger/app/swagger.json", "App API v1");

        // Admin接口文档  
        options.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin API v1");

        // 可选：设置默认文档
        options.RoutePrefix = "swagger";
    });
}

//启用CORS
app.UseCors("WebSiteCors");
//app.UseHttpsRedirection();

//启用认证中间件
app.UseAuthentication();
app.UseAuthorization();

//注册统一响应处理中间件（应该在所有中间件之后，Endpoint之前）
app.UseMiddleware<UnifiedResponseMiddleware>();

app.MapControllers();
app.Run();