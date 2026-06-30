using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace OSS.StorageApiService
{

    /// <summary>
    /// 
    /// </summary>
    [Table("ossuserimagedata")]
    public class OssUserImageData : Entity<long>
    {

        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件大小(B)
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 签名地址
        /// </summary>
        public string ShareLink { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
