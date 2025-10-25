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

        public AppFileStoreController(IAppFileService fileUploadService, ILogger<AppFileStoreController> logger)
        {
            _fileUploadService = fileUploadService;
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
    }
}
