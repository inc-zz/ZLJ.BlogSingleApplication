using Azure.Core;
using Blogs.AppServices.Commands.Blogs.Article;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Requests.App;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
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
                RecommendationType = param.Type
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
                Keyword = param.Keyword,
                SortBy = param.SortBy
            };

            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        ///// <summary>
        ///// 文章搜索
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpGet("search")]
        //public async Task<ActionResult> SearchArticlesAsync([FromQuery] ArticleSearchRequest param)
        //{
        //    var query = new SearchArticlesQuery
        //    {
        //        Keyword = param.Keyword,
        //        PageIndex = param.PageIndex,
        //        PageSize = param.PageSize,
        //        CategoryId = param.CategoryId,
        //        TagId = param.TagId
        //    };

        //    var result = await _mediator.Send(query);
        //    return new OkObjectResult(result);
        //}

        ///// <summary>
        ///// 增加文章阅读量
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost("view-count")]
        //public async Task<ActionResult> IncreaseViewCountAsync([FromBody] ArticleIdRequest param)
        //{
        //    var command = new IncreaseArticleViewCountCommand
        //    {
        //        ArticleId = param.ArticleId
        //    };

        //    var result = await _mediator.Send(command);

        //    if (result)
        //    {
        //        return Ok(ResultObject.Success("阅读量更新成功"));
        //    }
        //    else
        //    {
        //        var notifications = _notificationHandler.GetNotifications();
        //        return BadRequest(notifications);
        //    }
        //}

        ///// <summary>
        ///// 点赞文章
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost("like")]
        //public async Task<ActionResult> LikeArticleAsync([FromBody] ArticleLikeRequest param)
        //{
        //    var command = new LikeArticleCommand
        //    {
        //        ArticleId = param.ArticleId,
        //        UserId = param.UserId // 从token或session中获取
        //    };

        //    var result = await _mediator.Send(command);

        //    if (result)
        //    {
        //        return Ok(ResultObject.Success("点赞成功"));
        //    }
        //    else
        //    {
        //        var notifications = _notificationHandler.GetNotifications();
        //        return BadRequest(notifications);
        //    }
        //}

        ///// <summary>
        ///// 取消点赞
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost("unlike")]
        //public async Task<ActionResult> UnlikeArticleAsync([FromBody] ArticleLikeRequest param)
        //{
        //    var command = new UnlikeArticleCommand
        //    {
        //        ArticleId = param.ArticleId,
        //        UserId = param.UserId
        //    };

        //    var result = await _mediator.Send(command);

        //    if (result)
        //    {
        //        return Ok(ResultObject.Success("取消点赞成功"));
        //    }
        //    else
        //    {
        //        var notifications = _notificationHandler.GetNotifications();
        //        return BadRequest(notifications);
        //    }
        //}

        ///// <summary>
        ///// 获取相关文章
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpGet("related")]
        //public async Task<ActionResult> GetRelatedArticlesAsync([FromQuery] RelatedArticlesRequest param)
        //{
        //    var query = new GetRelatedArticlesQuery
        //    {
        //        ArticleId = param.ArticleId,
        //        TopCount = param.TopCount
        //    };

        //    var result = await _mediator.Send(query);
        //    return new OkObjectResult(result);
        //}



        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("comments")]
        public async Task<ActionResult> GetArticleCommentListAsync([FromQuery] GetArticleCommentsRequest param)
        {
            var query = new GetArticleCommentsQuery
            {
                ArticleId = param.ArticleId,
                PageIndex = param.PageIndex,
                PageSize = param.PageSize
            };
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }


        /// <summary>
        /// 提交评论
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("setcomment")]
        public async Task<ActionResult> SetArticleCommentAsync([FromBody] SetArticleCommentRequest param)
        {
            CreateArticleCommentCommand command = new CreateArticleCommentCommand(param.ArticleId, param.Content);
            // 发送命令并获取结果
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("评论成功！"));
            }
            return BadRequest(ResultObject.Success("提交失败！"));

        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("comment")]
        public async Task<ActionResult> DeleteArticleCommentAsync([FromBody] IdParam param)
        {

            DeleteArticleCommentCommand command = new DeleteArticleCommentCommand(param.Id);
            // 发送命令并获取结果
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("删除成功！"));
            }
            return BadRequest(ResultObject.Success("删除失败！"));
        }


    }
}
