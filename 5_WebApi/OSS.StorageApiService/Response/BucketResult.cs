namespace OSS.StorageApiService
{
    public class BucketResult
    {
        /// <summary>
        /// Bucket Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Bucket名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Bucket所有者
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// Bucket所有者名称
        /// </summary>
        public string OwnerName { get; set; }
    }
}
