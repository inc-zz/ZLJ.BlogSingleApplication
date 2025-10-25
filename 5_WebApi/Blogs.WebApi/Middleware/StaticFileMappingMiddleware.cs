using Microsoft.Extensions.FileProviders;

namespace Blogs.WebApi.Middleware
{
    /// <summary>
    /// 文件上传路径映射中间件
    /// </summary>
    public static class StaticFileMappingMiddleware
    {
        public static IApplicationBuilder UseStaticFileMapping(
            this IApplicationBuilder app,
            string rootPath,
            string busTypePath)
        {
            var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            var logger = app.ApplicationServices.GetService<ILogger<Program>>();

            try
            {
                var physicalPath = Path.Combine(environment.ContentRootPath, rootPath, busTypePath);

                // 确保目录存在
                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                    logger?.LogInformation("已创建目录: {PhysicalPath}", physicalPath);
                }

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(physicalPath),
                    RequestPath = $"/{busTypePath}",
                    OnPrepareResponse = ctx =>
                    {
                        ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=3600");
                    }
                });

                logger?.LogInformation("已映射静态文件: {VirtualPath} -> {PhysicalPath}", $"/{busTypePath}", physicalPath);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "添加静态文件映射失败: {RootPath}/{BusTypePath}", rootPath, busTypePath);
            }

            return app;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="mappings"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticFileMappings(
            this IApplicationBuilder app,
            params (string rootPath, string busTypePath)[] mappings)
        {
            foreach (var mapping in mappings)
            {
                app.UseStaticFileMapping(mapping.rootPath, mapping.busTypePath);
            }
            return app;
        }
    }
}
