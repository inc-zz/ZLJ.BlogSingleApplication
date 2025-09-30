using Blogs.Domain.Entity.Blogs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.AppServices.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppFileService
    {
        Task<FileUploadResult> UploadSingleFileAsync(IFormFile file, string businessType, string? description = null, string? userId = null);
        Task<IEnumerable<FileUploadResult>> UploadMultipleFilesAsync(IEnumerable<IFormFile> files, string businessType, string? description = null, string? userId = null);
        Task<BlogsFileRecord?> GetFileRecordAsync(long fileId);
        Task<bool> DeleteFileAsync(long fileId);
    }

    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public BlogsFileRecord? FileRecord { get; set; }
        public string? FileUrl { get; set; }
    }
}
