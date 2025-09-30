using Blogs.AppServices.Commands.Admin.SysDepartment;
using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Requests.Admin;
using Blogs.Core.Models;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("/api/admin/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _notificationHandler;

        public DepartmentController(ILogger<DepartmentController> logger,
            IMediator mediator,
            INotificationHandler<DomainNotification> notifications)
        {
            _logger = logger;
            _mediator = mediator;
            _notificationHandler = notifications as DomainNotificationHandler;
        }

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult> GetDepartmentTreeAsync([FromQuery] PageParam param)
        {
            // 创建查询对象
            var query = new GetUserListQuery
            {
                PageIndex = param.PageIndex,
                PageSize = param.PageSize,
                SearchTerm = null,
                IsActive = false,
                RoleId = 0
            };
            // 通过中介者发送查询请求
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 获取部门列表（树形结构）
        /// </summary>
        /// <returns></returns>
        [HttpGet("tree")]
        public async Task<ActionResult> GetDepartmentTreeAsync()
        {
            var query = new GetDepartmentTreeQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// 获取部门详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("info")]
        public async Task<ActionResult> GetDepartmentInfoAsync([FromQuery] IdParam param)
        {
            var query = new GetDepartmentInfoQuery
            {
                Id = param.Id
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddDepartmentAsync([FromBody] AddDepartmentRequest request)
        {
            var command = new CreateDepartmentCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("部门创建成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 编辑部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> EditDepartmentAsync([FromBody] UpdateDepartmentRequest request)
        {
            var command = new UpdateDepartmentCommand(request);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("部门更新成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteDepartmentAsync([FromBody] IdParam param)
        {
            var command = new DeleteDepartmentCommand(param.Id);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(ResultObject.Success("部门删除成功"));
            }
            else
            {
                var notifications = _notificationHandler.GetNotifications();
                return BadRequest(notifications);
            }
        } 
         
    }
}
