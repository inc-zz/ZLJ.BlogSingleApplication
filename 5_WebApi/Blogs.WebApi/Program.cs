using Blogs.AppServices.CommandHandlers.Admin;
using Blogs.AppServices.QueryHandlers.Admin;
using Blogs.Core;
using Blogs.Core.Config;
using Blogs.Domain;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.EventBus;
using Blogs.Domain.EventNotices;
using Blogs.Domain.IRepositorys.Admin;
using Blogs.Domain.IServices;
using Blogs.Domain.Notices;
using Blogs.Infrastructure;
using Blogs.Infrastructure.Context;
using Blogs.Infrastructure.JwtAuthorize;
using Blogs.Infrastructure.OpenIdDict;
using Blogs.Infrastructure.Repositorys.Admin;
using Blogs.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Serilog.Events;
using Serilog;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Blogs.AppServices.AppServices.Interface;
using Blogs.AppServices.AppServices.implement;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// ���ó�ʼ��
AppConfig.Init(builder.Services, builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region ����Swagger��������֤

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // ���JWT��ȫ����
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT ��֤����ͷ. ��ʽ: 'Bearer <token>'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

#endregion

// ע��HttpContext������
builder.Services.AddHttpContextAccessor();

#region ע��Redis����
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = AppConfig.GetSettingString("ConnectionStrings:RedisConnection");
    return ConnectionMultiplexer.Connect(configuration);
});
#endregion
 
#region JWT����
var jwtConfig = AppConfig.GetConfigModel<JwtConfig>("JwtConfig");
Console.WriteLine("JwtConfig=================================================");
Console.WriteLine(JsonConvert.SerializeObject(jwtConfig));

// ע��JWT����
//builder.Services.AddScoped<IJwtService, JwtService>();
#endregion

#region ע��ִ�
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
// ע�������ִ�...
#endregion

#region ע���¼����ߴ������
builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
#endregion

#region ����ע������Command��Query Handler
// ����1: ʹ��MediatR�Զ�ע�ᣨ�Ƽ���
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetUserQueryHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UserCommandHandler).Assembly);
});

// ����2: �ֶ�����ע��
// builder.Services.AddAllCommandAndQueryHandlers();

// ����3: �������ռ�ע��
// builder.Services.AddHandlersByNamespace();

#endregion

#region ����Ǩ��-��ṹ����

// ��ʼ�����ݿ�������
var dbContext = new SqlSugarDbContext();
//dbContext.InitTables();
#endregion

#region ע������֪ͨ������
builder.Services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
#endregion

#region ����OpenidDict
// ע���Զ������
builder.Services.AddScoped<IOpenIddictService, OpenIddictService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

// ����EF Core������
builder.Services.AddDbContext<OpenIddictDbContext>(options =>
{
    var connectionString = AppConfig.GetSettingString("ConnectionStrings:MySqlConnectionWrite");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

    // ���ø���ϸ�Ĵ�����Ϣ���������ݼ�¼��������������
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// ����OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<OpenIddictDbContext>();
    })
    .AddServer(options =>
    {
        // ������Ȩ�롢���롢ˢ�����ƺͿͻ���ƾ֤����
        options.AllowAuthorizationCodeFlow()
               .AllowPasswordFlow()
               .AllowRefreshTokenFlow()
               .AllowClientCredentialsFlow();

        // �������ƶ˵�
        options.SetTokenEndpointUris("/connect/token");

        // ������Ȩ�˵㣨�����Ҫ��Ȩ�����̣�
        options.SetAuthorizationEndpointUris("/connect/authorize");

        // �����û���Ϣ�˵�
        options.SetUserInfoEndpointUris("/connect/userinfo");

        // ע�᷶Χ
        options.RegisterScopes(
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Roles,
            OpenIddictConstants.Scopes.OfflineAccess);

        // ע������
        options.RegisterClaims(OpenIddictConstants.Claims.Subject);

        // ���ܺ�ǩ������
        if (builder.Environment.IsDevelopment())
        {
            options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();
        }
        else
        {
            // ��������ʹ����ʵ��֤��
            // options.AddEncryptionCertificate(certificate)
            //        .AddSigningCertificate(certificate);
        }

        // ����������������
        options.SetAccessTokenLifetime(TimeSpan.FromHours(1));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

        // ʹ��ASP.NET Core����
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
 
// ������֤
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
// ������Ȩ
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api");
    });
});

// ���� Redis
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConnectionString))
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(
        ConnectionMultiplexer.Connect(redisConnectionString));
}

#endregion

#region ��־����
// ���� Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // ����ȫ����С��־����
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) 
    .Enrich.FromLogContext() // ����־�������зḻ����
    .WriteTo.Console() // ͬʱ���������̨
    .WriteTo.File(
        path: "Logs/.txt", // ��־�ļ�·��������ģʽ
        rollingInterval: RollingInterval.Day, // ����ָ��ļ�
        retainedFileCountLimit: 30, // �������30�����־�ļ�
        fileSizeLimitBytes: 10 * 1024 * 1024, // �����ļ����10MB
        rollOnFileSizeLimit: true, // ���ļ��ﵽ��С���ƺ�Ҳ����
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        encoding: System.Text.Encoding.UTF8
    )
    .CreateLogger();
builder.Host.UseSerilog();
#endregion

#region CORS��������
builder.Services.AddCors(options =>
{
    options.AddPolicy("WebSiteCors", builder =>
    {
        var corsUrls = AppConfig.GetSection("CorsConfig").Value;
        var origins = corsUrls.Split(';', StringSplitOptions.RemoveEmptyEntries);

        builder.WithOrigins(origins)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // ���ǰ����Ҫ����cookies��ƾ֤
    });
});

#endregion

#region �ļ��洢
builder.Services.AddScoped<IAppFileService, AppFileService>();
// ���� Kestrel �������������С���ƣ���������������
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024; // ���磺����Ϊ 1GB
});
// ���� IIS ������ѡ���Բ��� IIS �������
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 1024 * 1024 * 1024; // ���磺����Ϊ 1GB
});
// ���ñ�ѡ���� Multipart  Body ��������
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue; // ȡ�����ƣ�������Ϊһ���ܴ��ֵ
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

#endregion

var app = builder.Build();

#region Openiddict��ʼ��
// ��ʼ��CurrentUser��̬��
using (var scope = app.Services.CreateScope())
{
    var openIddictService = scope.ServiceProvider.GetRequiredService<IOpenIddictService>();
    CurrentUser.SetProvider(openIddictService);
}

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//����CORS
app.UseCors("WebSiteCors");

//app.UseHttpsRedirection();

//������֤�м��
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();