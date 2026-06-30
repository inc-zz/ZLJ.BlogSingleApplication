using System;
using System.Collections.Generic;
using System.Text; 

namespace OSS.StorageApiService;

/// <summary>
/// 获取文件列表请求参数
/// </summary>
public class ObjectListRequest: BucketNameRequest
{ 
    /// <summary>
    /// 分组方式 空或""：CommonPrefixes：空，S3Objects：文件对象
    /// "/"：CommonPrefixes：列举所有目录，S3Objects：所有对象（包括目录，目录为null对象）
    /// </summary>
    public string Delimiter { get; set; }
    /// <summary>
    /// 最大遍历文件个数
    /// </summary>
    public int MaxKeys { get; set; } = 100;
    /// <summary>
    /// 存储路径匹配头  如 OtherDir/000  结合上面的Delimiter和Bucket 拿到的文件路径  Bucket/DelimiterDir/Prefix.../xxx.jpg
    /// </summary>
    public string Prefix { get; set; }
    /// <summary>
    /// 列举指定marker之后的文件
    /// </summary>
    public string Marker { get; set; }
    /// <summary>
    /// 在 此之后开始输出
    /// </summary>
    public string StartAfter { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ContinuationToken { get; set; }
}
