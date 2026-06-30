using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OSS.StorageApiService;


/// <summary>
/// 存储对象
/// </summary>
public class StorageObject
{
    /// <summary>
    /// 
    /// </summary>
    public string ETag { get; set; }
    /// <summary>
    /// 存储桶
    /// </summary>
    public string BucketName { get; set; }
    /// <summary>
    /// 对象
    /// </summary>
    public string ObjectName { get; set; }
    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? LastModified { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long Length { get; set; }
    /// <summary>
    /// 所有者
    /// </summary>
    public StorageObjectOwner Owner { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Stream ResponseStream { get; set; }
}
