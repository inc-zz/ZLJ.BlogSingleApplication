using Azure.Core;
using Blogs.AppServices.Commands.Blogs.Article;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Requests.App;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using Blogs.Infrastructure.Constant;
using Blogs.Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.App
{

    /// <summary>
    /// 前端-文章模块
    /// </summary>
    [Route("api/app/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IMediator _mediator; //查询调用处理器
        private readonly DomainNotificationHandler _notificationHandler; //领域通知处理器
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="notifications"></param>
        public ArticleController(ILogger<ArticleController> logger,
            IMediator mediator,
            INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 获取文章分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        public async Task<ActionResult> GetCategoriesAsync([FromQuery] HotArticlesRequest request)
        {
            var query = new GetArticleCategoriesQuery(request.TopCount);
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
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
        /// 获取文章标签列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("tags")]
        public async Task<ActionResult> GetArticleTagsAsync([FromQuery] HotArticlesRequest param)
        {
            var query = new GetArticleTagsQuery
            {
                TopCount = param.TopCount
            };

            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取推荐榜单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("recommendations")]
        public async Task<ActionResult> GetRecommendationsAsync([FromQuery] RecommendationRequest param)
        {
            var query = new GetArticleRecommendationsQuery
            {
                TopCount = param.TopCount,
                RecommendationType = BlogsSettingBusType.Recommend
            };

            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取开源项目
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("openSourceProject")]
        public async Task<ActionResult> GetOpenSourceProjectAsync([FromQuery] RecommendationRequest param)
        {
            var query = new GetArticleRecommendationsQuery
            {
                TopCount = param.TopCount,
                RecommendationType = BlogsSettingBusType.OpenSourceProject
            };

            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取技术板块
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("hot")]
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
        /// 我的文章列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("myArticles")]
        public async Task<ActionResult> GetMyArticleListAsync([FromQuery] GetMyArticleListRequest request)
        {
            var query = new GetMyArticleListQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SortBy = "CreateAt",
                SortDescending = true
            };
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 点赞文章
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("like")]
        public async Task<ActionResult> LikeArticleAsync([FromBody] ArticleLikeRequest param)
        {
            var command = new LikeArticleCommand(param.ArticleId, param.IsLike);

            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(ResultObject.Success("操作成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 获取相关文章
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("related")]
        public async Task<ActionResult> GetRelatedArticlesAsync([FromQuery] RelatedArticlesRequest param)
        {
            var query = new GetRelatedArticlesQuery(param.ArticleId, param.TopCount);
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }


        /// <summary>
        /// 发布文章
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("publish")]
        public async Task<ActionResult> CreateArticleAsync([FromBody] CreateArticleRequest param)
        {
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
        /// 删除文章
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteArticleAsync([FromBody] IdParam param)
        {
            return Ok(ResultObject.Success("删除成功！"));
        }

        /// <summary>
        /// 隐藏文章
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("status")]
        public async Task<ActionResult> ChangeArticleStatusAsync([FromBody] ChangeArticleStatusRequest param)
        {
            return Ok(ResultObject.Success("状态修改成功！"));
        }


    }
}
