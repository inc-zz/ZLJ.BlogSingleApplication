using Amazon.S3; 
using Amazon.S3.Model; 
using Microsoft.Extensions.Options; 
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel;
using Minio.DataModel.Args; 
using System.Reactive.Linq; 

namespace OSS.StorageApiService
{
    /// <summary>
    /// 对象存储
    /// </summary>
     //[Dependency(ServiceLifetime.Singleton)]
    public partial class MinIOStorageClient : IMinIOStorageClient
    {
        protected IMinioClient _client { get; set; }

        protected AmazonS3Client _awsclient { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MinIOStorageClient(IOptions<MinioStorageConfig> options)
        {
            var config = options.Value;
            _client = new MinioClient()
            .WithEndpoint(config.Endpoint)
            .WithCredentials(config.AccessKey, config.SecretKey)
            .WithSSL(config.Secure == true)
            .Build();

            var amazonS3Config = new Amazon.S3.AmazonS3Config()
            {
                ForcePathStyle = true,
                ServiceURL = $"http://{config.Endpoint}",
                UseHttp = true
            };
            _awsclient = new Amazon.S3.AmazonS3Client(config.AccessKey, config.SecretKey, amazonS3Config);
        }

        /// <summary>
        /// 桶列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResultObject> GetBucketListAsync()
        {
            var result = new ResultObject<List<BucketResult>>();

            var response = await _client.ListBucketsAsync();
            if (response == null) return result;

            result.data = new List<BucketResult>();
            var bucketList = response.Buckets;
            foreach (var item in bucketList)
            {
                var model = new BucketResult();
                if (!string.IsNullOrEmpty(response.Owner))
                {
                    model.OwnerId = response.Owner;
                    model.OwnerName = response.Owner;
                }
                model.CreateDate = item.CreationDateDateTime;
                model.Name = item.Name;
                result.data.Add(model);
            }
            result.code = (int)ApiStatusEnum.Success;
            result.message = ApiStatusEnum.Success.GetDescription();

            return result;
        }

        /// <summary>
        /// 上传对象
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResultObject PutObjectAsync(ObjectCreateDto request)
        {
            var args = new PutObjectArgs()
                .WithBucket(request.BucketName)
                .WithObject(request.ObjectName)
                .WithObjectSize(request.InputStream.Length)
                .WithStreamData(request.InputStream);
            var response = _client.PutObjectAsync(args).GetAwaiter().GetResult();
            if (response.Size > 0)
            {
                return new ResultObject(ApiStatusEnum.Success.GetDescription(), (int)ApiStatusEnum.Success);
            }
            return new ResultObject(ApiStatusEnum.Fail.GetDescription(), (int)ApiStatusEnum.Fail);

        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResultObject DeleteObject(StorageObjectNameDto request)
        {
            var args = new RemoveObjectArgs()
                .WithBucket(request.BucketName)
                .WithObject(request.ObjectName);
            try
            {
                _client.RemoveObjectAsync(args).GetAwaiter().GetResult();
                return new ResultObject(ApiStatusEnum.Fail.GetDescription(), (int)ApiStatusEnum.Fail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="downloadFilename"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ResumableGetObject(StorageObjectNameDto request, string downloadFilename)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 对象列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResultObject ListObjects(ObjectListRequest request)
        {

            if (!string.IsNullOrWhiteSpace(request.Prefix))
            {
                request.Prefix = request.Prefix.TrimEnd('/') + "/";
            }
            var result = new ResultObject<StorageObjectListDto>();

            try
            {
                var listObjectsArgs = new ListObjectsArgs()
                    .WithBucket(request.BucketName)
                    .WithPrefix(request.Prefix);
                var response = _client.ListObjectsAsync(listObjectsArgs);

                result.data = new StorageObjectListDto
                {
                    BucketName = request.BucketName,
                    StorageObject = new List<StorageObject>()
                };
                foreach (var item in response)
                {
                    var model = new StorageObject
                    {
                        ObjectName = item.Key,
                        ETag = item.ETag,
                        LastModified = item.LastModified.ToDateTime(),
                        Length = item.Size.ToLong()
                    };
                    result.data.StorageObject.Add(model);
                }


            }
            catch (Exception e)
            {

                throw;
            }
            result.code = (int)ApiStatusEnum.Success;
            result.message = ApiStatusEnum.Success.GetDescription();
            return result;

        }

        /// <summary>
        /// 获取预签名
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultObject> GetPreSignerUrlAsync(ObjectPreSignerRequest param)
        {
            var result = new ResultObject<string>();

            // 生成签名URL。
            var url = string.Empty;
            if (param.MethodType == OssObjectHandleConstant.READ)
            {
                var request = new PresignedGetObjectArgs();
                request.WithBucket(param.BucketName);
                request.WithObject(param.ObjectName);
                request.WithExpiry((int)param.TimeExpire);
                url = await _client.PresignedGetObjectAsync(request);
            }
            else if (param.MethodType == OssObjectHandleConstant.WRITE)
            {
                var request = new PresignedPutObjectArgs();
                request.WithBucket(param.BucketName);
                request.WithObject(param.ObjectName);
                request.WithExpiry((int)param.TimeExpire);
                url = await _client.PresignedPutObjectAsync(request);
            }

            result.data = url;
            result.code = (int)ApiStatusEnum.Success;
            result.message = "处理成功";
            return result;
        }

 
        /// <summary>
        /// 分片上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResultObject> MultipartUploadExample(MultipartUploadDto request)
        {

            var result = new ResultObject<List<string>>();
            result.data = new List<string>();

            var filePath = request.FilePath;
            var bucketName = request.BucketName;
            var objectName = request.ObjectName;
            FileStream file = System.IO.File.OpenRead(filePath);

            // Create list to store upload part responses.
            List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();

            // Setup information required to initiate the multipart upload.
            var guid = Guid.NewGuid();
            InitiateMultipartUploadRequest initiateRequest = new InitiateMultipartUploadRequest
            {
                BucketName = request.BucketName,
                Key = request.ObjectName
            };
            // 初始化分片上传
            InitiateMultipartUploadResponse initResponse =
                await _awsclient.InitiateMultipartUploadAsync(initiateRequest);
            // Upload parts.
            long contentLength = new FileInfo(filePath).Length;
            long partSize = 5 * (long)Math.Pow(2, 20); // 5 MB
            try
            {
                long filePosition = 0;
                for (int i = 1; filePosition < contentLength; i++)
                {
                    UploadPartRequest uploadRequest = new UploadPartRequest
                    {
                        BucketName = bucketName,
                        Key = objectName,
                        UploadId = initResponse.UploadId,
                        PartNumber = i,
                        PartSize = partSize,
                        FilePosition = filePosition,
                        FilePath = filePath
                    };
                    // Track upload progress.
                    //uploadRequest.StreamTransferProgress +=
                    //    new EventHandler<StreamTransferProgressArgs>(UploadPartProgressEventCallback);

                    // 上传分片
                    uploadResponses.Add(await _awsclient.UploadPartAsync(uploadRequest));

                    filePosition += partSize;
                }

                // 合并所有分片
                CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    UploadId = initResponse.UploadId
                };
                completeRequest.AddPartETags(uploadResponses);

                //完成上传
                CompleteMultipartUploadResponse completeUploadResponse =
                  await _awsclient.CompleteMultipartUploadAsync(completeRequest);

                result.code = (int)ApiStatusEnum.Success;
                result.message = ApiStatusEnum.Success.GetDescription();
            }
            catch (Exception exception)
            {
                Console.WriteLine("An AmazonS3Exception was thrown: { 0}", exception.Message);

                // Abort the upload.
                AbortMultipartUploadRequest abortMPURequest = new AbortMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    UploadId = initResponse.UploadId
                };
                //中止分段上传
                await _awsclient.AbortMultipartUploadAsync(abortMPURequest);
            }
            return result;
        }


        public void Dispose()
        {
            _client.Dispose();
        }

    }
}
