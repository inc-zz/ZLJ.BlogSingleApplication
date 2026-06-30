using Amazon.S3;
using Amazon.S3.Model;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.AccessControl;
using Volo.Abp.AspNetCore.Mvc;

 
namespace OSS.StorageApiService
{

    [ApiController]
    [Route("api/storage")]
    public class ObjectStorageController : AbpController
    {
        public IStorageAggregateService _aggregateService { get; set; }

        public IMinIOStorageClient _client { get; set; }

        private StorageTypeEnum _clientType;

        public ObjectStorageController()
        {
            _clientType = StorageConfigs.GetStorageType();
        }

        /// <summary>
        /// 桶列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("buckets")]
        public async Task<ResultObject> GetBucketAsync()
        {
            using var client = _aggregateService.GetInstance(_clientType);
            var result = await client.GetBucketListAsync();
            return result;
        }


        /// <summary>
        /// 对象列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("objects")]
        public ResultObject ListObjects([FromQuery] ObjectListRequest request)
        {
            using var client = _aggregateService.GetInstance(_clientType);
            var result = client.ListObjects(request);

            //var result2 = _client.ListObjects(request);

            return result;
        }

        /// <summary>
        /// 获取签名地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("presigner")]
        public async Task<ResultObject> PresignerObjectAsync([FromQuery] ObjectPreSignerRequest request)
        {
            using var client = _aggregateService.GetInstance(_clientType);
            var result = await client.GetPreSignerUrlAsync(request);
            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut("uploadObject")]
        [Authorize]
        public ResultObject UploadObject(string bucketName,string objectName,  IFormFile file)
        {
            var ms = file.OpenReadStream();
            var inputStream = file.OpenReadStream();
            var request = new ObjectCreateDto
            {
                BucketName = bucketName,
                ObjectName = objectName,
                InputStream = inputStream
            };
            using var _client2 = _aggregateService.GetInstance(_clientType);
            var result = _client2.PutObjectAsync(request);
            return result;
        }
        /// <summary>
        /// 临时签名上传对象
        /// </summary>
        /// <param name="token"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("preSignerUpload")]
        public ResultObject<string> PreSignerUpload(string token, IFormFile file)
        {
            var result = new ResultObject<string>();
            HttpWebRequest httpRequest = WebRequest.Create(token) as HttpWebRequest;
            httpRequest.Method = "Put";
            using (Stream dataStream = httpRequest.GetRequestStream())
            {
                var buffer = new byte[file.Length];
                var fileStream = file.OpenReadStream();
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    dataStream.Write(buffer, 0, bytesRead);
                }
            }
            HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                result.code = (int)ApiStatusEnum.Success;
                result.message = ApiStatusEnum.Success.GetDescription();
            }
            return result;
        }

        /// <summary>
        /// 分片上传
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("multipartUpload")]
        public ResultObject PutObject(string bucketName, string objectName, string filePath, IFormFile file)
        {
            var result = _client.MultipartUploadExample(new MultipartUploadDto
            {
                BucketName = bucketName,
                ObjectName = objectName,
                FilePath = filePath
            }).GetAwaiter().GetResult();
            return result;

        }
    }
 
}
