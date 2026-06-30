using System.ComponentModel;

namespace OSS.StorageApiService;

/// <summary>
/// 存储服务类型
/// </summary>
public enum StorageTypeEnum
{
    /// <summary>
    /// Aws
    /// </summary>
    [Description("AwsStorage")]
    Aws = 1,
    /// <summary>
    /// Aliyun
    /// </summary>
    [Description("AliStorage")]
    Aliyun,
    /// <summary>
    /// MinIO
    /// </summary>
    [Description("MinIOStorage")]
    MinIO,
    /// <summary>
    /// 华为云 
    /// </summary>
    [Description("HuaWeiStorage")]
    Huawei

}
