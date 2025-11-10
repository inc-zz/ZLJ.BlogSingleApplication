using Blogs.AppServices.AppServices.Interface;
using Blogs.AppServices.Commands.Admin.Article;
using Blogs.AppServices.Commands.Admin.Category;
using Blogs.AppServices.Commands.Blogs.Article;
using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.AppServices.Requests.Admin;
using Blogs.AppServices.Requests.App;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Blogs;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using Blogs.Infrastructure.Context;
using Blogs.WebApi.Controllers.Store;
using Blogs.WebApi.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 文章管理
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticleController:ControllerBase
    {

        private readonly ILogger<ArticleController> _logger;
        private readonly IMediator _mediator; //查询调用处理器
        private readonly DomainNotificationHandler _notificationHandler; //领域通知处理器
        private readonly IAppFileService _fileUploadService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="notifications"></param>
        public ArticleController(ILogger<ArticleController> logger,
         IMediator mediator,
         INotificationHandler<DomainNotification> notifications,
         IAppFileService fileUploadService)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
            _fileUploadService = fileUploadService;
        }

        /// <summary>
        /// 获取技术板块
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("category/ddl")]
        public async Task<ActionResult> GetHotArticlesAsync([FromQuery] HotArticlesRequest param)
        {
            var query = new GetHotArticlesQuery
            {
                TopCount = param.TopCount
            };

            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取文章列表（支持分类筛选）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult> GetArticleListAsync([FromQuery] ArticleListRequest param)
        {
            var query = new GetArticleListQuery
            {
                PageIndex = param.PageIndex,
                PageSize = param.PageSize,
                CategoryId = param.CategoryId,
                TagId = param.TagId,
                SearchTerm = param.Where,
                SortBy = param.SortBy
            };
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteArticleAsync([FromBody] IdParam param)
        {
            await Task.CompletedTask;
            return Ok(ResultObject.Success("删除成功！"));
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> CreateArticleAsync([FromBody] CreateArticleRequest param)
        {
            param.SetCreateBy(CurrentUser.Instance.UserInfo.UserName);
            CreateArticleCommand command = new CreateArticleCommand(param);
            // 发送命令并获取结果
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("文章创建成功！"));
            }
            return BadRequest(ResultObject.Success("文章创建失败！"));
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("detail")]
        public async Task<ActionResult> GetArticleDetailAsync([FromQuery] ArticleIdRequest request)
        {
            var query = new GetArticleDetailQuery(request.ArticleId);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound(ResultObject.Error("文章不存在"));
            }
            return new OkObjectResult(result);
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
                var userId = CurrentUser.Instance.UserId.ToString();
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

        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("category")]
        public async Task<ActionResult> GetArticleCategoryAsync([FromQuery] GetBlogsCategoryListQuery request)
        {
            var result = await _mediator.Send(request);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取文章分类详情
        /// </summary>
        [HttpGet("categories/{id}")]
        public async Task<ActionResult> GetBlogsCategoryByIdAsync(long id)
        {
            var query = new GetBlogsCategoryByIdQuery(id);
            var result = await _mediator.Send(query);
            return new OkObjectResult(ResultObject<BlogsCategoryDto>.Success(result));
        }
        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("category")]
        public async Task<ActionResult> CreateCategoryAsync([FromBody] AddCategoryRequest request)
        {
            var command = request.Adapt<CreateBlogsCategoryCommand>();
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("文章分类创建成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(ResultObject.FromDomainNotifications(notifications));
            }
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("category")]    
        public async Task<ActionResult> DeleteCategoryAsync([FromBody] IdParam request)
        {
            var command = new DeleteBlogsCategoryCommand(request.Id);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("文章分类删除成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(ResultObject.FromDomainNotifications(notifications));
            }
        }
        /// <summary>
        /// 评论列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("comment")]
        public async Task<ActionResult> GetCommentCategoryAsync([FromQuery] PagedRequest request)
        {
            var query = new GetBlogsCommentListQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SearchTerm = request.SearchTerm
            };
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("comment")]
        public async Task<ActionResult> DeleteCommentAsync([FromBody] IdParam param)
        {
            var command = new DeleteBlogsCommentCommand(param.Id);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("评论删除成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(ResultObject.FromDomainNotifications(notifications));
            }
        }


    }
}
