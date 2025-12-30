using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Common.Config
{
    /// <summary>
    /// 文件存储配置
    /// </summary>
    public class FileStoreConfig
    {
        /// <summary>
        /// 存储服务器URL
        /// </summary>
        public string? StoreServerUrl { get; set; }
        /// <summary>
        /// 允许的文件扩展名
        /// </summary>
        public string[] AllowedFileExtensions { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 最大文件大小（字节）
        /// </summary>
        public long MaxFileSize { get; set; } = 100 * 1024 * 1024; // 默认100MB

        /// <summary>
        /// 大文件阈值（字节）
        /// </summary>
        public long LargeFileThreshold { get; set; } = 50 * 1024 * 1024; // 大文件阈值50MB

        /// <summary>
        /// 最大并发上传数
        /// </summary>
        public int MaxConcurrentUploads { get; set; } = 5;

        /// <summary>
        /// 默认业务类型
        /// </summary>
        public string DefaultBusinessType { get; set; } = "blog-attachments";
        /// <summary>
        /// 文件保存路径
        /// </summary>
        public string FileSavePath { get; set; }
    }

    /// <summary>
    /// 上传目录配置
    /// </summary>
    public class DirectoryConfig
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; } = string.Empty;

        /// <summary>
        /// 物理路径
        /// </summary>
        public string PhysicalPath { get; set; } = string.Empty;

        /// <summary>
        /// URL路径
        /// </summary>
        public string UrlPath { get; set; } = string.Empty;
    }
}
