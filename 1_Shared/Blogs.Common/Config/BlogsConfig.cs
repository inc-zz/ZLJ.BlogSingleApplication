using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core;

/// <summary>
/// 系统配置参数
/// </summary>
public class BlogsConfig
{
    /// <summary>
    /// Redis连接字符串  
    /// </summary>
    public string? RedisConnectionString { get; set; }
    /// <summary>
    /// Redis数据库索引
    /// </summary>
    public string? RedisDbIndex { get; set; }
    /// <summary>
    /// MySql连接字符串(读)
    /// </summary>
    public string? ConnectionReadString { get; set; }
    /// <summary>
    /// MySql连接字符(写)
    /// </summary>
    public string? ConnectionWriteString { get; set; } 
    /// <summary>
    /// 缓存类型
    /// </summary>
    public string? CacheType { get; set; }
    /// <summary>
    /// 日志类型
    /// </summary>
    public string? LoggerType { get; set; }
    /// <summary>
    /// 运行环境
    /// </summary>
    public string? Environment { get; set; }
    /// <summary>
    /// 登录用户缓存Key
    /// </summary>
    public string? LoginKey { get; set; }
    /// <summary>
    /// 错误重试次数
    /// </summary>
    public string? LoginErrorCount { get; set; }
    /// <summary>
    /// 登录过期时间（分钟）
    /// </summary>
    public string? LoginFreezingTime { get; set; } 

}
