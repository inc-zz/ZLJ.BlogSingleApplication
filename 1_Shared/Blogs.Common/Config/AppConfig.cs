using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Net.Sockets;

namespace Blogs.Core.Config;
 
/// <summary>
/// 应用程序配置封装类
/// </summary>
public class AppConfig
{
    private static IConfiguration _configuration;

    /// <summary>
    /// 初始化配置系统
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置对象</param>
    public static void Init(IServiceCollection services, IConfiguration configuration)
    {
        _configuration = configuration;

        // 注册配置选项
        services.Configure<BlogsConfig>(configuration.GetSection("AppConfig"));
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));

        // 注册AppConfig为单例服务，便于依赖注入
        services.AddSingleton<AppConfig>();
    }

    /// <summary>
    /// 获取配置值
    /// </summary>
    /// <param name="key">配置键</param>
    /// <returns>配置值</returns>
    public static string GetSettingString(string key)
    {
        return _configuration?[key];
    }

    /// <summary>
    /// 获取写库连接字符串
    /// </summary>
    public static string ConnectionReadString
    {
        get
        {
            return _configuration?.GetSection("AppConfig:ConnectionReadString").Value;
        }
    }
    /// <summary>
    ///     
    /// </summary>
    public static string ConnectionWriteString
    {
        get
        {
            return _configuration?.GetSection("AppConfig:ConnectionWriteString").Value; 
        }
    } 

    /// <summary>
    /// 获取配置节
    /// </summary>
    /// <param name="key">配置节键</param>
    /// <returns>配置节</returns>
    public static IConfigurationSection GetSection(string key)
    {
        return _configuration?.GetSection(key);
    }

    /// <summary>
    /// 从配置中获取指定类型的配置节并转换为模型
    /// </summary>
    /// <typeparam name="T">要转换的目标类型</typeparam>
    /// <param name="key">配置节的键</param>
    /// <returns>绑定后的配置模型实例</returns>
    public static T GetConfigModel<T>(string key) where T : class, new()
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        if (_configuration == null)
            throw new InvalidOperationException("配置未初始化，请先调用Init方法");

        var section = _configuration.GetSection(key);
        if (!section.Exists())
            return new T(); // 或者可以抛出异常，取决于您的需求

        return section.Get<T>() ?? new T();
    }

}
 