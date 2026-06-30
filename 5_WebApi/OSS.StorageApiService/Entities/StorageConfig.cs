using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace OSS.StorageApiService
{
    /// <summary>
    /// 对象存储配置表
    /// </summary>
    public class StorageConfig : Entity<long>
    {
        /// <summary>
        /// 存储供应商
        /// </summary>
        public string StorageType { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string StorageName { get; set; }
        /// <summary>
        /// 默认桶
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 对象存储服务地址
        /// </summary>
        public string Endpoint { get; set; }
        /// <summary>
        /// 对象存储API地址
        /// </summary>
        public string ApiAddress { get; set; }
        /// <summary>
        /// 对象存储AK
        /// </summary>
        public string AccessKey { get; set; }
        /// <summary>
        /// 对象存储SK
        /// </summary>
        public string SecretKey { get; set; } 

    }
}
