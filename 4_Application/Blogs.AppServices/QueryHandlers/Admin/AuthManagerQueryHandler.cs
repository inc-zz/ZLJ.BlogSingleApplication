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
      IRequestHandler<GetMenuButtonListQuery, ResultObject<List<AuthMenuButtonDto>>>
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
        public async Task<ResultObject<List<AuthMenuButtonDto>>> Handle(GetMenuButtonListQuery request, CancellationToken cancellationToken)
        {
            var list = await DbContext.Queryable<SysMenu>()
                .Where(it => it.IsDeleted == 0)
                .Select(it => new AuthMenuButtonDto
                {
                    MenuId = it.Id,
                    MenuName = it.Name
                }).ToListAsync();

            var menuId = list.Select(it => it.MenuId).ToList();
            var menuButtonList = await DbContext.Queryable<SysMenuButton, SysButtons>
                ((a, b) => a.ButtonId == b.Id).Where((a, b) => menuId.Contains(a.MenuId))
                .Select((a, b) => new MenuButtonDto
                {
                    MenuId = a.MenuId,
                    ButtonCode = b.Code,
                    ButtonName = b.Name

                }).ToListAsync();

            var buttonList = await DbContext.Queryable<SysButtons>()
                .Where(it => it.IsDeleted == 0)
                .OrderByDescending(o=>o.SortOrder)
                .Select(w => new AuthSysButtonDto
                {
                    Id = w.Id,
                    Code = w.Code,
                    Name = w.Name
                }).ToListAsync();
            foreach (var item in list)
            {
                var menuButtons = menuButtonList.Where(it => it.MenuId == item.MenuId).ToList();
                item.MenuAuthButtons = menuButtons;
            }
            return ResultObject<List<AuthMenuButtonDto>>.Success(list);

        }

    }
}
