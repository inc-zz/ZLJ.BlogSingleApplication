using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OSS.StorageApiService;


/// <summary>
/// 名单策略
/// </summary>
public enum EffectTypeEnum
{
    /// <summary>
    /// 白名单
    /// </summary>
    [Description("Allow")]
    Allow = 1,
    /// <summary>
    /// 黑名单
    /// </summary>
    [Description("Deny")]
    Deny

}
