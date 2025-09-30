using Blogs.AppServices.AppServices.Interface;
using Blogs.Domain.Entity.Blogs;
using Blogs.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Store
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileStoreController : ControllerBase
    {
        private readonly IAppFileService _fileUploadService;
        private readonly ILogger<FileStoreController> _logger;

        public FileStoreController(IAppFileService fileUploadService, ILogger<FileStoreController> logger)
        {
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        [HttpPost("upload")]
        //[AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> UploadFile(IFormFile file, [FromForm] string businessType,[FromForm] string? description = null)
        {
            try
            {
                var userId = CurrentUser.Instance.UserId.ToString();
                var result = await _fileUploadService.UploadSingleFileAsync(file, businessType, description, userId);

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
                _logger.LogError(ex, "文件上传过程中发生异常: {FileName}", file.FileName);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"文件上传失败: {ex.Message}"
                });
            }
        }

        [HttpPost("upload-multiple")]
        public async Task<ActionResult<ApiResponse>> UploadMultipleFiles(
            List<IFormFile> files,
            [FromForm] string businessType,
            [FromForm] string? description = null)
        {
            try
            {
                var userId = CurrentUser.Instance.UserId.ToString();
                var results = await _fileUploadService.UploadMultipleFilesAsync(files, businessType, description, userId);

                return Ok(new ApiResponse<IEnumerable<FileUploadResult>>
                {
                    Success = true,
                    Data = results,
                    Message = $"成功处理 {results.Count(r => r.Success)} 个文件，失败 {results.Count(r => !r.Success)} 个"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "多文件上传过程中发生异常");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"文件上传失败: {ex.Message}"
                });
            }
        }

        [HttpGet("{fileId}")]
        public async Task<ActionResult<ApiResponse>> GetFileInfo(long fileId)
        {
            try
            {
                var fileRecord = await _fileUploadService.GetFileRecordAsync(fileId);
                if (fileRecord == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "文件记录不存在"
                    });
                }

                return Ok(new ApiResponse<BlogsFileRecord>
                {
                    Success = true,
                    Data = fileRecord
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取文件信息失败: {FileId}", fileId);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"获取文件信息失败: {ex.Message}"
                });
            }
        }

        [HttpDelete("{fileId}")]
        public async Task<ActionResult<ApiResponse>> DeleteFile(long fileId)
        {
            try
            {
                var success = await _fileUploadService.DeleteFileAsync(fileId);
                if (success)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "文件删除成功"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "文件记录不存在"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除文件失败: {FileId}", fileId);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"删除文件失败: {ex.Message}"
                });
            }
        }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }
    }
}
