﻿using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Mapster;
using Polly;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.Admin
{
     public class MenuQueryHandler : SqlSugarDbContext,
        IRequestHandler<GetMenuListQuery, PagedResult<SysMenuDto>>,
        IRequestHandler<GetMenuTreeQuery, ResultObject<List<SysMenuTreeDto>>>,
        IRequestHandler<GetMenuInfoQuery, SysMenuDto> 
    {
        /// <summary>
        /// 获取菜单列表（分页）
        /// </summary>
        public async Task<PagedResult<SysMenuDto>> Handle(GetMenuListQuery request, CancellationToken cancellationToken)
        {
            RefAsync<int> total = 0;
            var list = await DbContext.Queryable<SysMenu>()
            .WhereIF(!string.IsNullOrEmpty(request.MenuName), x => x.Name.Contains(request.MenuName))
            .WhereIF(request.ParentId.HasValue, x => x.ParentId == request.ParentId || x.Id == request.ParentId)
            .OrderBy(x => x.Sort)
            .OrderBy(x => x.CreatedAt, OrderByType.Desc)
            .ToPageListAsync(request.PageIndex, request.PageSize, total, cancellationToken);

            var dtoList = list.Adapt<List<SysMenuDto>>();

            var result = new PagedResult<SysMenuDto>
            {
                code = 200,
                message = "获取成功",
                success = true,
                Items = dtoList,
                Total = total.Value,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };

            return result;
        }

        /// <summary>
        /// 获取菜单树（最多二级）
        /// </summary>
        public async Task<ResultObject<List<SysMenuTreeDto>>> Handle(GetMenuTreeQuery request, CancellationToken cancellationToken)
        {
            // 获取所有菜单并按层级排序
            var allMenus = await DbContext.Queryable<SysMenu>()
                .WhereIF(request.Status.HasValue, x=>x.Status == request.Status)
                .WhereIF(request.MenuType.HasValue, x => x.Type == request.MenuType.Value.ToString())
                .OrderBy(x => x.Sort)
                .OrderBy(x => x.CreatedAt, OrderByType.Desc)
                .Select(it=> new SysMenuDto
                {
                    Id = it.Id,
                    ParentId = it.ParentId,
                    Name=it.Name,
                    Icon = it.ICon,
                    Url = it.Url
                })
                .ToListAsync(cancellationToken);

            // 构建最多二级的菜单树
            var menuTree = BuildMenuTree(allMenus, 0, 2);
            var list =  menuTree.Adapt<List<SysMenuTreeDto>>();
            var result =  ResultObject<List<SysMenuTreeDto>>.Success(list, "");
            return result;
        }

        /// <summary>
        /// 获取菜单详情
        /// </summary>
        public async Task<SysMenuDto> Handle(GetMenuInfoQuery request, CancellationToken cancellationToken)
        {
            var menu = await DbContext.Queryable<SysMenu>()
                .Where(x => x.Id == request.Id)
                .FirstAsync(cancellationToken);

            if (menu == null)
            {
                throw new Exception("菜单不存在");
            }

            return menu.Adapt<SysMenuDto>();
        }
         
        /// <summary>
        /// 构建菜单树（限制最大层级）
        /// </summary>
        /// <param name="menus">所有菜单</param>
        /// <param name="parentId">父菜单ID</param>
        /// <param name="maxLevel">最大层级（0表示不限制）</param>
        /// <param name="currentLevel">当前层级</param>
        /// <returns>菜单树</returns>
        private List<SysMenuTreeDto> BuildMenuTree(List<SysMenuDto> menus, long parentId, int maxLevel = 0, int currentLevel = 0)
        {
            var tree = new List<SysMenuTreeDto>();

            // 如果设置了最大层级且当前层级已达到最大层级，则不再递归
            if (maxLevel > 0 && currentLevel >= maxLevel)
            {
                return tree;
            }

            var parentMenus = menus.Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Sort)
                .ToList()
                .Adapt<List<SysMenuTreeDto>>();

            foreach (var menu in parentMenus)
            {
                var children = BuildMenuTree(menus, menu.Id, maxLevel, currentLevel + 1);
                if (children.Any())
                {
                    menu.Children = children;
                }
                tree.Add(menu);
            }

            return tree;
        }
    }

}
