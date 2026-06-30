using Volo.Abp.Domain.Entities;

namespace ZLJ.Storage.ApiService.Entities
{
    /// <summary>
    /// 商品图片
    /// </summary>
    public class OssProductImage : Entity<long>
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
