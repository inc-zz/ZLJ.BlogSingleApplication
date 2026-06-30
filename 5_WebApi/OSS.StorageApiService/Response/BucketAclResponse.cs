using Amazon.S3.Model;

namespace OSS.StorageApiService
{
    public class BucketAclResponse
    {
        /// <summary>
        /// 桶名
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 桶所有者
        /// </summary>
        public StorageOwner Owner { get; set; }
        /// <summary>
        /// 授权用户
        /// </summary>
        public string CanonicalUser { get; set; }
        /// <summary>
        /// 授权用户名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Acl权限
        /// </summary>
        public string Permissions { get; set; }
        /// <summary>
        /// 权限值
        /// </summary>
        public string HeaderName { get; set; }
        /// <summary>
        /// 权限说明
        /// </summary>
        public string PermissionsName { get; set; }
    }
}
