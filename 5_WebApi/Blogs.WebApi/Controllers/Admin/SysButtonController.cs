using Blogs.AppServices.Commands.Admin.SysButtons;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.AppServices.Requests.Admin;
using Blogs.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.Admin
{
    /// <summary>
    /// 操作按钮管理
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/admin/[controller]")]
    public class SysButtonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SysButtonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 获取按钮列表（分页）
        /// </summary>
        [HttpGet("list")]
        public async Task<ActionResult> GetButtons([FromQuery] GetButtonListQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(ResultObject<PagedResult<ButtonListDto>>.Success(result));
        }

        /// <summary>
        /// 获取按钮详情
        /// </summary>
        [HttpGet("info")]
        public async Task<ActionResult> GetButtonDetail(long id)
        {
            var result = await _mediator.Send(new GetButtonDetailQuery(id));

            if (result == null)
                return NotFound(ResultObject.Error("按钮不存在"));

            return Ok(result);
        }

        /// <summary>
        /// 获取所有可用按钮（用于下拉选择）
        /// </summary>
        [HttpGet("available")]
        public async Task<ActionResult> GetAvailableButtons([FromQuery] string position = null)
        {
            var result = await _mediator.Send(new GetAvailableButtonsQuery(position));
            return Ok(result);
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CreateButton([FromBody] CreateSysButtonRequest request)
        {
            SysCreateButtonCommand command = new SysCreateButtonCommand(request);
            var result = await _mediator.Send(command);

            if (result)
                return Ok(ResultObject.Success("创建按钮成功"));
            else
                return BadRequest(ResultObject.Error("创建按钮失败"));
        }

        /// <summary>
        /// 更新按钮
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> UpdateButton([FromBody] UpdateSysButtonRequest request)
        {
            var command = new SysUpdateButtonCommand(request);
            var result = await _mediator.Send(command);

            if (result)
                return Ok(ResultObject.Success("更新按钮成功"));
            else
                return BadRequest(ResultObject.Error("更新按钮失败"));
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        [HttpDelete]
        public async Task<ActionResult> DeleteButton([FromBody] IdParam param)
        {
            var command = new SysDeleteButtonCommand { Id = param.Id };
            var result = await _mediator.Send(command);

            if (result)
                return Ok(ResultObject.Success("删除按钮成功"));
            else
                return BadRequest(ResultObject.Error("删除按钮失败"));
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        [HttpPut("status")]
        public async Task<ActionResult> UpdateButtonStatus([FromBody] UpdateStatusRequest request)
        {
            var command = new SysUpdateButtonStatusCommand
            {
                Id = request.Id,
                Status = request.Status
            };

            var result = await _mediator.Send(command);

            if (result)
                return Ok(ResultObject.Success("更新按钮状态成功"));
            else
                return BadRequest(ResultObject.Error("更新按钮状态失败"));
        }
    }
}
