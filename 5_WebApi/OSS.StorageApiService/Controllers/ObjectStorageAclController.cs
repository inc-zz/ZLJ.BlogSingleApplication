using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc; 

namespace OSS.StorageApiService
{
    /// <summary>
    /// ACL权限控制
    /// </summary>
    [ApiController]
    [Route("api/storageAcl")]
    public class ObjectStorageAclController : AbpController
    {
        public IStorageAggregateService _aggregateService { get; set; }
        private StorageTypeEnum _clientType;

        /// <summary>
        /// 
        /// </summary>
        public ObjectStorageAclController()
        {

            _clientType = StorageConfigs.GetStorageType();
        }


        /// <summary>
        /// 获取ACL
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResultObject> GetAclResultAsync([FromQuery] BucketNameRequest param)
        {
            using var _client = _aggregateService.GetInstance(_clientType);

            var result = await _client.GetAclAuthAsync(param);
            return result;
        }

        /// <summary>
        /// 设置ACL
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObject> PutAclResultAsync([FromBody] BucketAclRequest param)
        {
            using var _client = _aggregateService.GetInstance(_clientType);
            var result = await _client.PutAclAuthAsync(param);
            return result;
        }


        /// <summary>
        /// 删除ACL
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ResultObject> DeleteBucketAclAsync([FromBody] BucketNameRequest param)
        {
            using var _client = _aggregateService.GetInstance(_clientType);
            var result = await _client.DeleteBucketAclAsync(param);
            return result;
        }

    }
}
