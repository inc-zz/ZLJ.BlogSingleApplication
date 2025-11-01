using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Requests.App;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 文章管理控制器
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/admin/[controller]")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator; //查询调用处理器
        private readonly DomainNotificationHandler _notificationHandler; //领域通知处理器

        public HomeController(ILogger<HomeController> logger,
            IMediator mediator,
            INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 获取文章分类列表(应该使用缓存)
        /// </summary>
        /// <returns></returns>
        [HttpGet("articleList")]
        public async Task<ActionResult> GetArticleListAsync([FromQuery] HotArticlesRequest param)
        {
            var query = new GetArticleListQuery
            {
                PageIndex = 1,
                PageSize = param.TopCount,
                SortBy = "ViewCount"
            };
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 首页统计
        /// </summary>
        /// <returns></returns>
        [HttpGet("statistics")]
        public async Task<ActionResult> GetSysStatisticsAsync()
        {
            var query = new GetSysStatisticsQuery
            {

            };
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }



    }
}
