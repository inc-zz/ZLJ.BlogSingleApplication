namespace Blogs.AppServices.Extensions
{
    /// <summary>
    /// 文件路径映射
    /// </summary>
    public static class FilePathConfig
    {
        // 上传根目录（可配置）
        public static string UploadRootPath { get; set; } = "wwwroot";

        // 业务类型对应的子目录
        public static Dictionary<string, string> BusinessTypeDirectories = new()
        {
            ["CoverImage"] = "Uploads/CoverImage",
            ["ArticleFiles"] = "Uploads/ArticleFiles",
            ["UserPhoto"] = "Uploads/UserPhoto",
            ["WebsiteImage"] = "Uploads/WebsiteImage"
        };

        // 获取完整保存路径
        public static string GetFullPath(string businessType)
        {
            if (BusinessTypeDirectories.TryGetValue(businessType, out var relativePath))
            {
                return Path.Combine(UploadRootPath, relativePath);
            }
            return Path.Combine(UploadRootPath, "Uploads", businessType);
        }

        // 获取URL访问路径
        public static string GetUrlPath(string businessType, string fileName)
        {
            if (BusinessTypeDirectories.TryGetValue(businessType, out var relativePath))
            {
                return $"/{relativePath.Replace("\\", "/")}/{fileName}";
            }
            return $"/Uploads/{businessType}/{fileName}";
        }

        /// <summary>
        /// 确保目录存在并且配置正确的权限
        /// </summary>
        /// <param name="path"></param>
        public static void EnsureDirectoryExists(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    // 设置Linux下的目录权限（如果运行在Linux环境）
                    if (Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        var process = new System.Diagnostics.Process
                        {
                            StartInfo = new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = "chmod",
                                Arguments = $"-R 755 {path}",
                                RedirectStandardOutput = true,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                            }
                        };
                        process.Start();
                        process.WaitForExit();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建目录失败: {path}");
                throw;
            }
        }
    }
}
