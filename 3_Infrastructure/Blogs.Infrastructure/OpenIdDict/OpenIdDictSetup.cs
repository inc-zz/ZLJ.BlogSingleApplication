using Blogs.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using System.Text;


namespace Blogs.Infrastructure.OpenIdDict
{
    /// <summary>
    /// OpenIdDict配置
    /// </summary>
    public static class OpenIdDictSetup
    {
        /// <summary>
        /// 添加OpenIdDict服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddOpenIdDictServices(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 从服务容器中获取配置
            var serviceProvider = services.BuildServiceProvider();
            var jwtConfig = serviceProvider.GetService<IOptions<JwtConfig>>()?.Value;

            if (jwtConfig == null)
                throw new InvalidOperationException("JWT配置未找到，请确保已在Startup中正确配置");

            services.AddOpenIddict()
                .AddServer(options =>
                {
                    // 设置端点
                    options.SetAuthorizationEndpointUris("/connect/authorize")
                   .SetTokenEndpointUris("/connect/token");

                    // 允许授权码流程和刷新令牌流程
                    options.AllowAuthorizationCodeFlow()
                           .AllowRefreshTokenFlow();

                    // 禁用权限检查（根据需要选择）
                    options.IgnoreEndpointPermissions()
                           .IgnoreGrantTypePermissions()
                           .IgnoreScopePermissions();

                    
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });

            // 配置认证方案
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });
        }
    }
}