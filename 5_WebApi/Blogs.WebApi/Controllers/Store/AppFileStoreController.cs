using Blogs.AppServices.AppServices.Interface;
using Blogs.Infrastructure.Context;
using Blogs.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Store
{
    /// <summary>
    /// 前端文件存储
    /// </summary>
    [ApiController]
    [Route("api/app/[controller]")]
    public class AppFileStoreController : ControllerBase
    {
        private readonly IAppFileService _fileUploadService;
        private readonly ILogger<AppFileStoreController> _logger;
        private readonly IWebHostEnvironment _env;
        public AppFileStoreController(IAppFileService fileUploadService,
            IWebHostEnvironment env,
            ILogger<AppFileStoreController> logger)
        {
            _fileUploadService = fileUploadService;
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse>> UploadFile([FromForm] ArticleFileUploadRequest request)
        {
            try
            {
                var userId = CurrentAppUser.Instance.UserId.ToString();
                var file = request.File;    
                var businessType = request.BusinessType;
                var result = await _fileUploadService.UploadSingleFileAsync(file, businessType, "", userId);

                if (result.Success)
                {
                    _logger.LogInformation("文件上传成功: {FileName} -> {StoredFileName}", file.FileName, result.FileRecord?.StoredFileName);
                    return Ok(new ApiResponse<FileUploadResult>
                    {
                        Success = true,
                        Data = result,
                        Message = result.Message
                    });
                }
                else
                {
                    _logger.LogWarning("文件上传失败: {FileName} - {Message}", file.FileName, result.Message);
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = result.Message
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "文件上传过程中发生异常: {FileName}", request.File.FileName);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"文件上传失败: {ex.Message}"
                });
            }
        }


        [HttpGet("check-files")]
        public IActionResult CheckFiles()
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var uploadsPath = Path.Combine(currentDirectory, "Uploads");
                var coverImagePath = Path.Combine(uploadsPath, "CoverImage");

                // 检查目录和文件
                var uploadsExists = Directory.Exists(uploadsPath);
                var coverImageExists = Directory.Exists(coverImagePath);

                // 获取所有文件
                var allFiles = Directory.GetFiles(coverImagePath).Select(f => new
                {
                    Name = Path.GetFileName(f),
                    Size = new FileInfo(f).Length,
                    FullPath = f
                }).ToList();

                // 检查特定文件
                var targetFile = Path.Combine(coverImagePath, "2_20251231002638684.png");
                var fileExists = System.IO.File.Exists(targetFile);

                return Ok(new
                {
                    Success = true,
                    CurrentDirectory = currentDirectory,
                    UploadsDirectory = new
                    {
                        Path = uploadsPath,
                        Exists = uploadsExists,
                        Subdirectories = uploadsExists
                            ? Directory.GetDirectories(uploadsPath)
                            : Array.Empty<string>()
                    },
                    coverImageDirectory = new
                    {
                        Path = coverImagePath,
                        Exists = coverImageExists
                    },
                    TargetFile = new
                    {
                        Path = targetFile,
                        Exists = fileExists,
                        Size = fileExists ? new FileInfo(targetFile).Length : 0
                    },
                    AllFiles = allFiles,
                    Environment = new
                    {
                        IsDevelopment = _env.IsDevelopment(),
                        ContentRoot = _env.ContentRootPath,
                        WebRoot = _env.WebRootPath
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查文件时发生错误");
                return StatusCode(500, new
                {
                    Success = false,
                    Error = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }

        /// <summary>
        /// DownloadTest
        /// </summary>
        /// <returns></returns>
        [HttpGet("download-test")]
        public IActionResult DownloadTest()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var testFile = Path.Combine(currentDirectory, "Uploads", "coverImage", "2_20251231002638684.png");

            if (!System.IO.File.Exists(testFile))
            {
                return NotFound(new { Message = "文件不存在", Path = testFile });
            }

            var fileBytes = System.IO.File.ReadAllBytes(testFile);
            return File(fileBytes, "image/png", "test.png");
        }
    }

}
