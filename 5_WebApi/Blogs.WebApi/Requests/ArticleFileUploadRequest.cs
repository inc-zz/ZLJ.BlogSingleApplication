namespace Blogs.WebApi.Requests
{
    /// <summary>
    /// 文件上传格式
    /// </summary>
    public class ArticleFileUploadRequest
    {
        public IFormFile File { get; set; }
        public string BusinessType { get; set; }

    }
}
