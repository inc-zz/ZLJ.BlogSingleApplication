using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Blogs;
using Dm.util;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    /// <summary>
    /// 授权管理查询执行处理器
    /// </summary>
    public class AuthManagerQueryHandler : SqlSugarDbContext,
      IRequestHandler<GetMenuButtonListQuery, ResultObject<List<SysMenuTreeButtonsDto>>>
    {
        private readonly IAppUserRepository _appUserRepository;
        public AuthManagerQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<List<SysMenuTreeButtonsDto>>> Handle(GetMenuButtonListQuery request, CancellationToken cancellationToken)
        {
            var list = await DbContext.Queryable<SysMenu>()
              .Where(it => it.IsDeleted == 0)
              .OrderByDescending(x => x.Sort)
              .Select(it => new SysMenuTreeButtonsDto
              {
                  MenuId = it.Id,
                  ParentId = it.ParentId,
                  Name = it.Name,
                  Icon = it.Icon
              })
              .ToListAsync(cancellationToken);

            var menuIds = list.Where(it => it.ParentId > 0).Select(it => it.MenuId).ToList();
            var menuButtonList = await DbContext.Queryable<SysMenuButton, SysButtons>
                ((a, b) => a.ButtonId == b.Id).Where((a, b) => menuIds.Contains(a.MenuId))
                .Select((a, b) => new MenuButtonDto
                {
                    MenuId = a.MenuId,
                    ButtonId = b.Id,
                    ButtonCode = b.Code,
                    ButtonName = b.Name
                }).ToListAsync();

            var buttonList = await DbContext.Queryable<SysButtons>()
                .Where(it => it.IsDeleted == 0)
                .OrderByDescending(o => o.SortOrder)
                .Select(w => new AuthSysButtonDto
                {
                    Id = w.Id,
                    Code = w.Code,
                    Name = w.Name
                }).ToListAsync();

            //获取当前角色拥有的按钮权限
            var roleAuthButtonList = await DbContext.Queryable<SysRoleMenuAuth>()
                .Where(it => it.RoleId == request.RoleId)
                .ToListAsync();

            var rootNode = list.Where(it => it.ParentId == 0).ToList();
            foreach (var item in rootNode)
            {
                var childNodes = list.Where(it => it.ParentId == item.MenuId)
                    .Select(gt => new SysMenuTreeButtonsDto
                {
                    MenuId = gt.MenuId,
                    Name = gt.Name,
                    Icon = gt.Icon,
                    HasPermissions = roleAuthButtonList.Any(it=>it.MenuId == gt.MenuId)
                }).ToList();
                foreach (var child in childNodes)
                {
                    var menuButtons = menuButtonList.Where(it => it.MenuId == child.MenuId).ToList();
                    child.MenuButtons = menuButtons;
                    child.HasPermissions = roleAuthButtonList.Any(it=>it.MenuId == child.MenuId);   
                }
                item.Children = childNodes;
            }

            return ResultObject<List<SysMenuTreeButtonsDto>>.Success(list);

        }
    }
}
