using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.AppServices.Requests.Admin;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Enums;
using Blogs.Core;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    /// <summary>
    /// 按钮查询处理器
    /// </summary>
    public class SysButtonsQueryHandler : SqlSugarDbContext,
        IRequestHandler<GetButtonListQuery, PagedResult<ButtonListDto>>,
        IRequestHandler<GetButtonDetailQuery, ButtonDetailDto>,
        IRequestHandler<GetAvailableButtonsQuery, List<ButtonListDto>>
    {
        private readonly ILogger<SysButtonsQueryHandler> _logger;

        public SysButtonsQueryHandler(ILogger<SysButtonsQueryHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取按钮列表（分页）
        /// </summary>
        public async Task<PagedResult<ButtonListDto>> Handle(GetButtonListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalCount = new RefAsync<int>();
                var list = await DbContext.Queryable<SysButtons>()
                    .Where(b => b.IsDeleted == 0)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Name), it => it.Code == request.Name || it.Name == request.Name)
                    .WhereIF(!string.IsNullOrEmpty(request.Position), it => it.Position == request.Position)
                    .WhereIF(request.Status.HasValue, it => it.Status == request.Status)
                      .Select(b => new ButtonListDto
                      {
                          Id = b.Id,
                          Code = b.Code,
                          Name = b.Name,
                          Description = b.Description,
                          Icon = b.Icon,
                          ButtonType = b.ButtonType,
                          Position = b.Position, 
                          Status = b.Status,
                          CreatedAt = b.CreatedAt,
                          CreatedBy = b.CreatedBy
                      })
                .ToPageListAsync(request.PageIndex, request.PageSize, totalCount);

                foreach (var item in list)
                {
                    item.StatusName = EnumHelper.GetEnumText(new ApproveStatusEnum(), item.Status);
                }
                return new PagedResult<ButtonListDto>(list, totalCount, request.PageIndex, request.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取按钮列表失败");
                throw;
            }
        }

        /// <summary>
        /// 获取按钮详情
        /// </summary>
        public async Task<ButtonDetailDto> Handle(GetButtonDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var button = await DbContext.Queryable<SysButtons>()
                    .Where(b => b.Id == request.Id && b.IsDeleted == 0)
                    .Select(b => new ButtonDetailDto
                    {
                        Id = b.Id,
                        Code = b.Code,
                        Name = b.Name,
                        Description = b.Description,
                        Icon = b.Icon,
                        ButtonType = b.ButtonType,
                        Position = b.Position,
                        Status = b.Status,
                        CreatedAt = b.CreatedAt,
                        CreatedBy = b.CreatedBy
                    })
                    .FirstAsync();

                if (button == null)
                    return null;

                // 获取关联的菜单信息
                var relatedMenus = await DbContext.Queryable<SysMenuButton>()
                    .LeftJoin<SysMenu>((mb, m) => mb.MenuId == m.Id)
                    .Where((mb, m) => mb.ButtonId == request.Id && m.IsDeleted == 0)
                    .Select((mb, m) => new ButtonMenuDto
                    {
                        MenuId = m.Id,
                        MenuName = m.Name
                    })
                    .ToListAsync();

                button.RelatedMenus = relatedMenus;

                return button;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取按钮详情失败，按钮ID: {ButtonId}", request.Id);
                throw;
            }
        }

        /// <summary>
        /// 获取所有可用按钮
        /// </summary>
        public async Task<List<ButtonListDto>> Handle(GetAvailableButtonsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = DbContext.Queryable<SysButtons>()
                    .Where(b => b.IsDeleted == 0 && b.Status == 1);

                if (!string.IsNullOrEmpty(request.Position))
                {
                    query = query.Where(b => b.Position == request.Position);
                }

                var buttons = await query
                    .OrderBy(b => b.SortOrder)
                    .Select(b => new ButtonListDto
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Description = b.Description,
                    Icon = b.Icon,
                    ButtonType = b.ButtonType,
                    Position = b.Position,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    CreatedBy = b.CreatedBy
                })
                .ToListAsync();

                return buttons;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取可用按钮列表失败");
                throw;
            }
        }
    }
}
