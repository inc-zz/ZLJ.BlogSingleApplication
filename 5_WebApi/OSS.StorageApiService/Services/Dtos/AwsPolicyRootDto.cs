using System;
using System.Collections.Generic;

namespace OSS.StorageApiService
{

    /// <summary>
    /// AWS存储桶策略
    /// </summary>
    public class AwsPolicyRootDto
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; } = "2012-10-17";
        /// <summary>
        /// 策略描述
        /// </summary>
        public List<AwsPolicyStatement> Statement { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// Aws策略规则
    /// </summary>
    public class AwsPolicyStatement
    {

        #region 格式

        /*
         * 委托人格式
            AWS Bucket Policy格式:
           {
           "Version": "2012-10-17",
           "Id": "ExamplePolicy01",
           "Statement": [
               {
                   "Sid": "ExampleStatement01",
                   "Effect": "Allow",
                   "Principal": {
                       "AWS": "arn:aws:iam::123456789012:user/Dave"
                   },
                   "Action": [
                       "s3:GetObject",
                       "s3:GetBucketLocation",
                       "s3:ListBucket"
                   ],
                   "Resource": [
                       "arn:aws:s3:::awsexamplebucket1/*",
                       "arn:aws:s3:::awsexamplebucket1"
                   ]
               }
           ]
           }
            
         *

            */
        #endregion

        /// <summary>
        /// 策略名称
        /// </summary>
        public string Sid { get; set; } = "BucketStatementPolicyConfig";
        /// <summary>
        /// 名单方式
        /// </summary>
        public string Effect { get; set; } = "Allow";
        /// <summary>
        /// 规则  "AWS": "arn:aws:iam::123456789012:user/Dave"
        /// </summary>
        public Dictionary<string, string[]> Principal { get; set; }
        /// <summary>
        /// 请求Action
        /// </summary>
        public string[] Action { get; set; }
        /// <summary>
        /// 访问资源
        /// </summary>
        public string[] Resource { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "2012-10-17";
        ///// <summary>
        ///// 条件
        ///// </summary>
        //public Dictionary<string, Dictionary<string, dynamic>> Condition { get; set; }

    }
}
