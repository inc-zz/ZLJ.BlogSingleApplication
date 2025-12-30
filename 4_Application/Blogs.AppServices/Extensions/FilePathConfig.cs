namespace Blogs.AppServices.Extensions
{
    /// <summary>
    /// 文件路径映射
    /// </summary>
    public static class FilePathConfig
    {

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
