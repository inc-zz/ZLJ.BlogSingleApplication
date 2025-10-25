using Blogs.Common.Config;
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
    private static AppConfig _instance;

    /// <summary>
    /// AppConfig单例实例
    /// </summary>
    public static AppConfig Instance => _instance ?? throw new InvalidOperationException("AppConfig未初始化，请先调用Init方法");

    /// <summary>
    /// 博客相关配置
    /// </summary>
    public BlogsConfig BlogsConfig { get; private set; }

    /// <summary>
    /// JWT相关配置
    /// </summary>
    public JwtConfig JwtConfig { get; private set; }

    /// <summary>
    /// 文件上传配置
    /// </summary>
    public FileStoreConfig FileStoreConfig { get; private set; }

    /// <summary>
    /// 私有构造函数，确保单例
    /// </summary>
    private AppConfig(IConfiguration configuration)
    {
        _configuration = configuration;
        LoadAllConfigs();
    }

    /// <summary>
    /// 初始化配置系统
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置对象</param>
    public static void Init(IServiceCollection services, IConfiguration configuration)
    {
        if (_instance != null)
            throw new InvalidOperationException("AppConfig已经初始化");

        _configuration = configuration;
        _instance = new AppConfig(configuration);

        // 注册配置选项到DI容器
        services.Configure<BlogsConfig>(configuration.GetSection("AppConfig"));
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
        services.Configure<FileStoreConfig>(configuration.GetSection("FileUpload"));

        // 注册AppConfig为单例服务，便于依赖注入
        services.AddSingleton(_instance);
    }

    /// <summary>
    /// 加载所有配置
    /// </summary>  
    private void LoadAllConfigs()
    {
        BlogsConfig = GetConfigModel<BlogsConfig>("AppConfig") ?? new BlogsConfig();
        JwtConfig = GetConfigModel<JwtConfig>("JwtConfig") ?? new JwtConfig();
        FileStoreConfig = GetConfigModel<FileStoreConfig>("FileStoreConfig") ?? new FileStoreConfig();
    }

    /// <summary>
    /// 重新加载配置（适用于配置热更新）
    /// </summary>
    public void Reload()
    {
        LoadAllConfigs();
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
    /// 获取读库连接字符串
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
            return new T();

        return section.Get<T>() ?? new T();
    }

    /// <summary>
    /// 获取连接字符串
    /// </summary>
    /// <param name="name">连接字符串名称</param>
    /// <returns>连接字符串</returns>
    public static string GetConnectionString(string name)
    {
        return _configuration?.GetConnectionString(name);
    }

    /// <summary>
    /// 获取环境变量或配置值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>配置值</returns>
    public static string GetValue(string key, string defaultValue = "")
    {
        return _configuration?[key] ?? defaultValue;
    }

    /// <summary>
    /// 获取环境变量或配置值（泛型版本）
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>配置值</returns>
    public static T GetValue<T>(string key, T defaultValue = default(T))
    {
        var value = _configuration?[key];
        if (string.IsNullOrEmpty(value))
            return defaultValue;

        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return defaultValue;
        }
    }
}
 