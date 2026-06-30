 namespace OSS.StorageApiService
{
    public interface IMinIOStorageClient : IStorageClientService
    {
        /// <summary>
        /// 分片上传
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<ResultObject> MultipartUploadExample(MultipartUploadDto request);
    }
}
