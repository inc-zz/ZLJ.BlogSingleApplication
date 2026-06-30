using System;
using System.Collections.Generic;
using System.Text;

namespace OSS.StorageApiService
{
    /// <summary>
    /// 存储桶访问策略
    /// </summary>
    public class BucketPolicyResult
    {
        /// <summary>
        /// 策略版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 策略名称
        /// </summary>
        public string Sid { get; set; } 
        /// <summary>
        /// 名单方式 白名单/黑名单
        /// </summary>
        public string Effect { get; set; }
        /// <summary>
        /// Effect名称
        /// </summary>
        public string EffectName { get; set; }
        /// <summary>
        /// 操作方法
        /// </summary>
        public string[] Action { get; set; }
        /// <summary>
        /// 资源
        /// </summary>
        public string[] Resource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string,string[]> Principal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string,Dictionary<string,List<string>>> Condition { get; set; }

    }
}
