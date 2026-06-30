using Amazon.S3.Model;
using Amazon.S3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapster;

namespace OSS.StorageApiService
{
    public partial class MinIOStorageClient
    {
        /// <summary>
        /// 获取ACL权限
        /// </summary>
        /// <param name="param"></param>
        public async Task<ResultObject<List<BucketAclResponse>>> GetAclAuthAsync(BucketNameRequest param)
        {

            var result = new ResultObject<List<BucketAclResponse>>();
            try
            {
                GetACLRequest request = new GetACLRequest
                {
                    BucketName = param.BucketName
                };

                GetACLResponse response = await _awsclient.GetACLAsync(request);
                result.code = (int)response.HttpStatusCode;

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var grants = response.AccessControlList.Grants;
                    var list = new List<BucketAclResponse>();
                    foreach (var item in grants)
                    {
                        list.Add(new BucketAclResponse
                        {
                            Permissions = item.Permission.Value,
                            HeaderName = item.Permission.HeaderName,
                            PermissionsName = item.Permission.Value,
                            BucketName = param.BucketName,
                            CanonicalUser = item.Grantee?.CanonicalUser,
                            DisplayName = item.Grantee?.DisplayName,
                            Owner = new StorageOwner(response.AccessControlList.Owner?.Id, response.AccessControlList.Owner?.DisplayName)
                        });
                    }

                    result.data = list;
                    result.message = "处理成功";
                }
                else
                {
                    result.code = (int)response.HttpStatusCode;
                    result.message = JsonConvert.SerializeObject(response.ResponseMetadata);
                }
            }
            catch (AmazonS3Exception e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    result.code = (int)ApiStatusEnum.Fail;
                    result.message = "禁止该操作";
                    return result;
                }
                if(e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result.code = (int)ApiStatusEnum.DataNotExists;
                    result.message = ApiStatusEnum.DataNotExists.GetDescription();
                    return result;
                }
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /// <summary>
        /// 更新存储桶ACL权限
        /// </summary>
        public async Task<ResultObject> PutAclAuthAsync(BucketAclRequest param)
        {
            var result = new ResultObject();

            /*
                Object（对象）        — Read（读取）	READ	允许被授权者读取对象数据及其元数据
                Object ACL（对象 ACL）— Read（读取）	READ_ACP	允许被授权者读取对象 ACL
                Object ACL（对象 ACL）— Write（写入）	WRITE_ACP	允许被授权者为适用的对象编写 ACL
             */

            // 查询Bucket的相关授权
            GetACLResponse aclResponse = await _awsclient.GetACLAsync(new GetACLRequest
            {
                BucketName = param.BucketName
            });

            S3AccessControlList acl = aclResponse.AccessControlList;
            // 检索所有者（清除ACL后使用它来重新添加权限）。
            Amazon.S3.Model.Owner owner = acl.Owner;

            // 拿到旧的授权,新的用户授权直接覆盖
            var oldGrants = new List<S3Grant>();
            foreach (var item in acl.Grants)
            {
                if (item.Grantee.CanonicalUser == null)
                    continue;

                if (!param.AclUsers.Select(x => x.UserName).Contains(item.Grantee.CanonicalUser) && owner.Id != item.Grantee.CanonicalUser)
                {
                    oldGrants.Add(item);
                }
            }

            // 清除现有授权.
            acl.Grants.Clear();
            // 给Bucket拥有者完全权限
            S3Grant fullControlGrant = new S3Grant
            {
                Grantee = new S3Grantee { CanonicalUser = owner.Id, DisplayName = owner.DisplayName },
                Permission = S3Permission.FULL_CONTROL
            };

            //授予其它用户权限
            var s3Grants = new List<S3Grant>();
            var dicPermissions = new Dictionary<string, string>();
            switch (param.Permissions)
            {
                case OssObjectHandleConstant.READ:
                    dicPermissions.Add("READ", "x-amz-grant-read");
                    break;
                case OssObjectHandleConstant.WRITE:
                    dicPermissions.Add("WRITE", "x-amz-grant-write");
                    break;
                case OssObjectHandleConstant.READ_WRITE:
                    //dicPermissions.Add("READ_ACP", "x-amz-grant-read-acp");
                    dicPermissions.Add("READ", "x-amz-grant-read");
                    dicPermissions.Add("WRITE", "x-amz-grant-write");
                    break;
                case OssObjectHandleConstant.OWINER_CONTROL:
                    dicPermissions.Add("FULL_CONTROL", "x-amz-grant-full-control");
                    break;
                default:
                    break;
            }

            // 增加一个权限受众
            S3Grant grantUser = null;
            foreach (var item in param.AclUsers)
            {
                foreach (var dic in dicPermissions)
                {
                    grantUser = new S3Grant
                    {
                        Grantee = new S3Grantee { CanonicalUser = item.UserName, DisplayName = item.DisplayName },
                        Permission = new S3Permission(dic.Key, dic.Value)
                    };
                    s3Grants.Add(grantUser);
                }
            }

            acl.Grants = new List<S3Grant>();
            acl.Grants.Add(fullControlGrant);
            acl.Grants.AddRange(s3Grants);
            acl.Grants.AddRange(oldGrants);

            var request = new PutACLRequest
            {
                BucketName = param.BucketName,
                AccessControlList = acl
            };

            // 设置ACL权限
            PutACLResponse response = await _awsclient.PutACLAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                // 如果给了别的用户完全控制，需要禁用该用户对PutAcl的授权 
                var userGroup = string.Empty;
                //if (StorageSettingConfig.Instance.awsStorageConfig.Agent == AwsStorageAgentConstant.H3C)
                //    userGroup = AwsRestApiConfig.Instance.h3cApiConfig.Tenant;

                var userPrincipal = new List<string>();
                foreach (var user in param.AclUsers)
                {
                    if (user.UserName != owner.Id)
                        userPrincipal.Add($"arn:aws:iam::{userGroup}:{user.UserName.Replace(userGroup + "$", "")}");
                }



                string newPolicy =
                @"{
                    ""Version"":""2012-10-17"",
                    ""Statement"":[{  
                        ""Sid"":""202110180001"",
                        ""Effect"":""Deny"", 
                        ""Action"":[""s3:PutBucketAcl""],  
                        ""Principal"": {""AWS"": @userPrincipal},
                        ""Resource"":[""arn:aws:s3:::@bucket"",""arn:aws:s3:::@bucket/*""] 
                      },{  
                        ""Sid"":""202110180002"",
                        ""Effect"":""Allow"", 
                        ""Action"":[""s3:ListBucket""],  
                        ""Principal"": {""AWS"": @userPrincipal},
                        ""Resource"":[""arn:aws:s3:::@bucket"",""arn:aws:s3:::@bucket/*""] 
                      }]}".Replace("@tenant", userGroup).Replace("@userPrincipal", JsonConvert.SerializeObject(userPrincipal)).Replace("@bucket", param.BucketName);

                JObject jObject = JsonConvert.DeserializeObject<JObject>(newPolicy);
                List<JObject> statementListObjects = JsonConvert.DeserializeObject<List<JObject>>(jObject["Statement"].ToString());
                var statementList = JsonConvert.DeserializeObject<List<AwsPolicyStatement>>(JsonConvert.SerializeObject(statementListObjects));
                //获取已经存在的Policy，防止被覆盖
                var existsBucketPolicy = await GetBucketPolicy(new BucketNameRequest { BucketName = param.BucketName });
                if (existsBucketPolicy.isSuccess())
                {
                    var existsBucketList = existsBucketPolicy.data;
                    var otherStatementList = existsBucketList.Adapt<List<AwsPolicyStatement>>();
                    statementList.AddRange(otherStatementList);
                }
                var policyModel = new AwsPolicyRootDto
                {
                    Id = $"{param.BucketName}_BucketPolicy",
                    Version = "2012-10-17",
                    Statement = statementList
                };

                PutBucketPolicyRequest putRequest = new PutBucketPolicyRequest
                {
                    BucketName = param.BucketName,
                    Policy = JsonConvert.SerializeObject(policyModel)
                };

                var policyStatementList = new List<AwsPolicyStatement>();
                var principalList = new Dictionary<string, string[]>();
                principalList.Add("AWS", userPrincipal.ToArray());

                //非存储桶的拥有者禁用设置桶ACL权限
                var denyPolicyStatement = new AwsPolicyStatement
                {
                    Sid = Guid.NewGuid().ToString("N"),
                    Effect = EffectTypeEnum.Deny.GetDescription(),
                    Action = new string[] { "s3:PutBucketAcl" },
                    Principal = principalList,
                    Resource = new string[] { $"arn:aws:s3:::{param.BucketName}" }
                };

                //拥有完全控制权限的用户给与文件下载权限
                var allowPolicyStatement = new AwsPolicyStatement
                {
                    Sid = Guid.NewGuid().ToString("N"),
                    Effect = EffectTypeEnum.Allow.GetDescription(),
                    Action = new string[] { "s3:ListBucket", "s3:GetObject" },
                    Principal = principalList,
                    Resource = new string[] { $"arn:aws:s3:::{param.BucketName}/*" }
                };

                policyStatementList.Add(denyPolicyStatement);
                policyStatementList.Add(allowPolicyStatement);

                //追加存储桶策略【额外在Policy手动配置】
                //await AttachBucketPolicy(policyStatementList, param.BucketName);


                result.code = (int)ApiStatusEnum.Success;
                result.message = ApiStatusEnum.Success.GetDescription();
                return result;
            }
            result.code = (int)response.HttpStatusCode;

            return result;
        }

        /// <summary>
        /// 删除ACL授权
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultObject> DeleteBucketAclAsync(BucketNameRequest param)
        {

            var result = new ResultObject();

            GetACLResponse aclResponse = await _awsclient.GetACLAsync(new GetACLRequest
            {
                BucketName = param.BucketName
            });

            S3AccessControlList acl = aclResponse.AccessControlList;
            // 检索所有者（清除ACL后使用它来重新添加权限）。
            Amazon.S3.Model.Owner owner = acl.Owner;

            // 清除现有授权.
            acl.Grants.Clear();
            // 给Bucket拥有者完全权限
            S3Grant fullControlGrant = new S3Grant
            {
                Grantee = new S3Grantee { CanonicalUser = owner.Id, DisplayName = owner.DisplayName },
                Permission = S3Permission.FULL_CONTROL
            };

            acl.Grants = new List<S3Grant>();
            acl.Grants.Add(fullControlGrant);

            // 设置ACL权限
            PutACLResponse response = await _awsclient.PutACLAsync(new PutACLRequest
            {
                BucketName = param.BucketName,
                AccessControlList = acl
            });
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                result.code = (int)ApiStatusEnum.Success;
                result.message = ApiStatusEnum.Success.GetDescription();
            }
            else
            {
                result.code = (int)response.HttpStatusCode;
                result.message = JsonConvert.SerializeObject(response.ResponseMetadata);
            }
            return result;

        }



        /// <summary>
        /// 追加存储桶Policy
        /// </summary>
        /// <param name="policyStatements"></param>
        /// <param name="bucketName"></param>
        private async Task AttachBucketPolicy(List<AwsPolicyStatement> policyStatements, string bucketName)
        {

            //获取已经存在的Policy，防止被覆盖
            var existsBucketPolicy = await GetBucketPolicy(new BucketNameRequest { BucketName = bucketName });
            if (existsBucketPolicy.isSuccess())
            {
                var existsBucketList = existsBucketPolicy.data;
                var otherStatementList = existsBucketList.Adapt<List<AwsPolicyStatement>>();
                policyStatements.AddRange(otherStatementList);
            }

            var bucketPolicy = new AwsPolicyRootDto
            {
                Statement = policyStatements,
                Id = Guid.NewGuid().ToString("N"),
            };

            var policy = JsonConvert.SerializeObject(bucketPolicy);

            PutBucketPolicyRequest putRequest = new PutBucketPolicyRequest
            {
                BucketName = bucketName,
                Policy = policy
            };
            PutBucketPolicyResponse response = await _awsclient.PutBucketPolicyAsync(putRequest);


        }

        /// <summary>
        /// 对象桶策略操作权限
        /// </summary>
        /// <param name="policyType"></param>
        /// <param name="actions"></param>
        private void GetOssActions(BucketPolicyTypeEnum policyType, ref List<string> actions)
        {
            switch (policyType)
            {
                case BucketPolicyTypeEnum.LIST:
                    actions.Add("s3:ListBucket");
                    actions.Add("s3:GetObject");
                    break;
                case BucketPolicyTypeEnum.WRITE:
                    actions.Add("s3:PutObject");
                    break;
                case BucketPolicyTypeEnum.READ_WRITE:
                    actions.Add("s3:GetObject");
                    actions.Add("s3:ListBucket");
                    actions.Add("s3:PutObject");
                    break;
                case BucketPolicyTypeEnum.DELETE:
                    actions.Add("s3:DeleteObject");
                    actions.Add("s3:DeleteObject");
                    break;
                case BucketPolicyTypeEnum.ACL_CONTROL:
                    actions.Add("s3:PutBucketAcl");
                    actions.Add("s3:GetBucketAcl");
                    actions.Add("s3:PutObjectAcl");
                    actions.Add("s3:GetObjectAcl");
                    break;
                case BucketPolicyTypeEnum.BUCKET_CONTROL:
                    actions.Add("s3:DeleteBucket");
                    actions.Add("s3:PutBucket");
                    break;
            }
        }




        /// <summary>
        /// 获取存储桶策略
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultObject<List<BucketPolicyResult>>> GetBucketPolicy(BucketNameRequest param)
        {
            var result = new ResultObject<List<BucketPolicyResult>>();

            var request = new GetBucketPolicyRequest
            {
                BucketName = param.BucketName
            };
            var response = await _awsclient.GetBucketPolicyAsync(request);
            var jsonStr = JsonConvert.SerializeObject(response);

            JObject jobj = JsonConvert.DeserializeObject<JObject>(jsonStr);
            if (jobj == null || jobj.Count == 0)
            {
                result.code = (int)ApiStatusEnum.DataNotExists;
                result.message = ApiStatusEnum.DataNotExists.GetDescription();
                return result;
            }

            var policy = jobj["Policy"].ToString();
            JObject jObject = JsonConvert.DeserializeObject<JObject>(policy);

            if (jObject == null || jObject.Count == 0)
            {
                result.code = (int)ApiStatusEnum.DataNotExists;
                result.message = ApiStatusEnum.DataNotExists.GetDescription();
                return result;
            }

            List<JObject> statementList = JsonConvert.DeserializeObject<List<JObject>>(jObject["Statement"].ToString());

            var list = new List<BucketPolicyResult>();

            foreach (JObject item in statementList)
            {
                var action = JsonConvert.DeserializeObject<List<string>>(item["Action"]?.ToString());
                var resource = JsonConvert.DeserializeObject<List<string>>(item["Resource"]?.ToString());
                var principal = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(item["Principal"]?.ToString());
                list.Add(new BucketPolicyResult
                {
                    Version = jObject["Version"]?.ToString(),
                    Sid = item["Sid"]?.ToString(),
                    Action = action?.ToArray(),
                    Effect = item["Effect"]?.ToString(),
                    Resource = resource?.ToArray(),
                    Principal = principal,
                    EffectName = item["Effect"].ToString().ToLower() == "allow" ? "白名单" : "黑名单"
                });
            }

            result.code = (int)ApiStatusEnum.Success;
            result.message = ApiStatusEnum.Success.GetDescription();
            result.data = list;
            return result;

        }



    }
}
