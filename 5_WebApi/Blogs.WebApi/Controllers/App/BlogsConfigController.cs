using Blogs.AppServices.Commands.Blogs.Article;
using Blogs.AppServices.Commands.Blogs.Settings;
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
    /// 博客配置控制器
    /// </summary>
    [ApiController]
    [Route("api/app/[controller]")]
    public class BlogsConfigController : ControllerBase
    {
        private readonly ILogger<BlogsConfigController> _logger;
        private readonly IMediator _mediator; //查询调用处理器

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="notifications"></param>
        public BlogsConfigController(ILogger<BlogsConfigController> logger,
          IMediator mediator,
          INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// 获取配置列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult> GetBlogsSettingsAsync([FromQuery] GetBlogsSettingRequest request)
        {
            var query = new GetBlogsSettingsQuery(request);
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 创建配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult> CreateBlogSettingAsync([FromBody] CreateBlogSettingRequest param)
        {
            CreateBlogSettingCommand command = new CreateBlogSettingCommand(param);
            // 发送命令并获取结果
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("创建成功！"));
            }
            return BadRequest(ResultObject.Success("创建失败！"));
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> CreateBlogSettingAsync([FromBody] UpdateBlogSettingRequest param)
        {
            UpdateBlogSettingCommand command = new UpdateBlogSettingCommand(param);
            // 发送命令并获取结果
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("创建成功！"));
            }
            return BadRequest(ResultObject.Success("创建失败！"));
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteBlogSettingAsync([FromBody] IdParam param)
        {
            DeleteBlogSettingCommand command = new DeleteBlogSettingCommand(param);
            // 发送命令并获取结果
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("创建成功！"));
            }
            return BadRequest(ResultObject.Success("创建失败！"));
        }

    }
}

