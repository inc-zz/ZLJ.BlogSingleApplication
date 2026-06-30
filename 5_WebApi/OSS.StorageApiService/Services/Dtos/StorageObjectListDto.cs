using System;
using System.Collections.Generic;
using System.Text;

namespace OSS.StorageApiService;


/// <summary>
/// 
/// </summary>
public class StorageObjectListDto
{
    /// <summary>
    /// 
    /// </summary>
    public string BucketName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<StorageObject> StorageObject { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> Dirs { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsTruncated { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string NextMarker { get; set; }
}
