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
    /// 文章评论控制器
    /// </summary>
    [ApiController]
    [Route("api/app/[controller]")]
    public class ArticleCommentController : ControllerBase
    {
        private readonly ILogger<ArticleCommentController> _logger;
        private readonly IMediator _mediator; //查询调用处理器
        private readonly DomainNotificationHandler _notificationHandler; //领域通知处理器
        public ArticleCommentController(ILogger<ArticleCommentController> logger, IMediator mediator,
            INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("list")]
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
        /// 创建文章评论
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
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
        [HttpDelete]
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
        /// <summary>
        /// 评论回复
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("reply")]
        public async Task<ActionResult> ReplyArticleCommentAsync([FromBody] ReplyArticleCommentRequest param)
        {
            CreateArticleCommentCommand command = new CreateArticleCommentCommand(param.ArticleId,param.ParentId, param.Content);
            // 发送命令并获取结果
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("评论成功！"));
            }
            return BadRequest(ResultObject.Success("提交失败！"));

        }
         

    }
}
