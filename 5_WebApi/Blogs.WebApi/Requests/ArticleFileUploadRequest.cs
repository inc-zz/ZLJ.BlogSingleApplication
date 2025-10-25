namespace Blogs.WebApi.Requests
{
    public class ArticleFileUploadRequest
    {
        public IFormFile File { get; set; }
        public string BusinessType { get; set; }

    }
}
