using Blogs.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.Enums
{

    /// <summary>
    /// 通用枚举
    /// </summary>
    public enum ApiEnum
    {
        [EnumText("处理成功")]
        Success = 200,
        [EnumText("未授权")]
        Unauthorized = 401,
        [EnumText("数据不存在")]
        NoFound = 404,
        [EnumText("数据已存在")]
        DataExists = 1003,
        [EnumText("处理失败")]
        Failed = 2001,
        [EnumText("缓存过期")]
        CacheExpiredError = 3001
    }

}