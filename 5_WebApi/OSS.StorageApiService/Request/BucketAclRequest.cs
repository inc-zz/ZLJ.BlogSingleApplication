
using System.ComponentModel.DataAnnotations;

namespace OSS.StorageApiService
{
    public class BucketAclRequest
    {
        /// <summary>
        /// Bucket名称
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 权限 1只读，2只写，3读写，4 完全控制
        /// </summary>
        [AllowedValues("ReadOnly", "WriteOnly", "ReadWrite", "FurrControl")]
        public string Permissions { get; set; }
        /// <summary>
        /// 权限受众(阿里云未提供)
        /// </summary>
        public List<AclUser> AclUsers { get; set; }

    }

    /// <summary>
    /// 权限受众
    /// </summary>
    public class AclUser
    {
        /// <summary>
        /// 用户名 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
    }

}
