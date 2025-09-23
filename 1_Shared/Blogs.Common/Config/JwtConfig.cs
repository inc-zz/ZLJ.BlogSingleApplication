using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core;


/// <summary>
/// JWT配置
/// </summary>
public class JwtConfig
{
    /// <summary>
    /// 签发者
    /// </summary>
    public string Issuer { get; set; } = "BlogsApp";

    /// <summary>
    /// 接收者
    /// </summary>
    public string Audience { get; set; } = "BlogsAppUsers";

    /// <summary>
    /// 安全密钥
    /// </summary>
    public string? SecurityKey { get; set; }

    /// <summary>
    /// 令牌过期时间(分钟)
    /// </summary>
    public int TokenExpires { get; set; } = 60;

    /// <summary>
    /// 刷新令牌过期时间(天)
    /// </summary>
    public int RefreshTokenExpires { get; set; } = 7;
}
