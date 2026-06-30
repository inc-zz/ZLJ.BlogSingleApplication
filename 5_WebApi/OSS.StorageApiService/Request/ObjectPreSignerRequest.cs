using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text; 

namespace OSS.StorageApiService
{

    /// <summary>
    /// 
    /// </summary>
    public class ObjectPreSignerRequest
    {
        /// <summary>
        /// 存储桶
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 签名对象 如 publicDir/test.jpg
        /// </summary> 
        public string ObjectName { get; set; }
        /// <summary>
        /// 操作权限 ReadOnly、WriteOnly、ReadWrite、FurrControl
        /// </summary> 
        public string MethodType { get; set; } = "ReadOnly";
        /// <summary>
        /// 过期时间（秒）默认5分钟
        /// </summary>
        public double TimeExpire { get; set; } = 300;
        /// <summary>
        /// 上传内容类型
        /// </summary>
        public string? ContentType { get; set; }

    }
}
