using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OSS.StorageApiService;
using System.Net;
using Volo.Abp.AspNetCore.Mvc;
using ZLJ.AbpFramework.Enums; 

namespace OSS.StorageApiService
{

    [ApiController]
    [Route("test")]
    public class TestController : AbpController
    {
        public ApiServiceDbContext _dbContext { get; set; }
        public IStorageAggregateService _client { get; set; }
        private StorageTypeEnum clientType;

        public TestController()
        {
            clientType = StorageConfigs.GetStorageType();
        }

        /// <summary>
        /// 获取预签名
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        [HttpGet("preSigner")]
        public async Task<ResultObject> GetPresignerUrl([FromQuery] ObjectPreSignerRequest request)
        {
            using var client = _client.GetInstance(clientType);
            var result = await client.GetPreSignerUrlAsync(request);
            return result;
        }

        /// <summary>
        /// 上传对象
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("putobject")]
        public ResultObject PutObject(string bucketName, string objectName, IFormFile file)
        {
            using var client = _client.GetInstance(clientType);
            var ms = file.OpenReadStream();
            var result = client.PutObjectAsync(new ObjectCreateDto
            {
                BucketName = bucketName,
                ObjectName = objectName,
                InputStream = ms
            });
            return result;
        }

        /// <summary>
        /// 临时签名上传对象
        /// </summary>
        /// <param name="token"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("putSignerObject")]
        public ResultObject<string> PreSignerPutObject(string token, IFormFile file)
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
        /// 根据临时令牌下载文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        [HttpGet("getobjectbypresigner")]
        public ResultObject<string> GetPresignerObject(string token, string savePath)
        {

            var result = new ResultObject<string>();
            FileStream fs = null;

            HttpWebRequest httpRequest = WebRequest.Create(token) as HttpWebRequest;

            HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;
            Stream ioStream = response.GetResponseStream();

            long fileLength = response.ContentLength;
            fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write);

            int length = 1024;
            byte[] buffer = new byte[1025];
            int bytesread = 0;
            while ((bytesread = ioStream.Read(buffer, 0, length)) > 0)
            {
                fs.Write(buffer, 0, bytesread);
            }
            result.code = (int)ApiStatusEnum.Success;
            result.message = $"保存路径{savePath}";
            return result;
        }

        /// <summary>
        /// Get配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("storageConfig")]
        public ResultObject<StorageConfig> GetStorageConfig()
        {
            //获取存储目录
            try
            {
                var model = _dbContext.StorageConfig.FirstOrDefault();
                return new ResultObject<StorageConfig>
                {
                    code = (int)ResponseType.Ok,
                    data = model,
                    message = ""
                };
            }
            catch (Exception e)
            {

                throw;
            }
        }

    }
}
