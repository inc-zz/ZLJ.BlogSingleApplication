using Blogs.AppServices.Queries.Admin;
using Blogs.Core;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Enums;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;
using Dm.util;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    /// <summary>
    /// 用户模块查询处理器
    /// </summary>
    public class AdminUserQueryHandler : SqlSugarDbContext,
        IRequestHandler<GetUserListQuery, PagedResult<AdminUserDto>>,
        IRequestHandler<GetUserInfoQuery, ResultObject<AdminUserDto>>
    {
        private readonly IUserRepository _userRepository;

        public AdminUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 查询管理员列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedResult<AdminUserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var searchTerm = request.SearchTerm;
            var isActive = request.IsActive;
             
            var query = DbContext.Queryable<SysUser>()
            .Where(it => it.IsDeleted == false)
            .Includes(u => u.Department) // 预加载部门
            .Includes(u => u.UserRoles)  // 预加载用户角色关系
            .WhereIF(!string.IsNullOrWhiteSpace(searchTerm), u =>
               u.UserName.Contains(searchTerm) ||
               u.Email.Contains(searchTerm) ||
               u.RealName.Contains(searchTerm))
            .WhereIF(request.RoleId > 0, u =>
            SqlFunc.Subqueryable<SysUserRoleRelation>()
                .Where(ur => ur.UserId == u.Id && ur.RoleId == request.RoleId)
                .Any())
            .WhereIF(isActive.HasValue, u => u.IsDeleted == isActive.Value);

            var totalCount = new SqlSugar.RefAsync<int>();
            var userEntities = await query.OrderByDescending(u => u.CreatedAt)
                .ToPageListAsync(request.PageIndex, request.PageSize, totalCount, cancellationToken);

            // 单独加载角色信息
            var allUserRoleIds = userEntities
                .SelectMany(u => u.UserRoles ?? new List<SysUserRoleRelation>())
                .Select(ur => ur.RoleId)
                .Distinct()
                .ToList();

            var roles = await DbContext.Queryable<SysRole>()
                .Where(r => allUserRoleIds.Contains(r.Id) && r.IsDeleted == 0)
                .ToListAsync(cancellationToken);

            var roleDict = roles.ToDictionary(r => r.Id, r => r);

            // 转换为DTO
            var users = userEntities.Select(u => new AdminUserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                RealName = u.RealName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department?.Name,
                CreatedAt = u.CreatedAt,
                LastLoginTime = u.LastLoginTime,
                Description = u.Description,
                Status = u.Status,
                Roles = (u.UserRoles ?? new List<SysUserRoleRelation>())
                    .Where(ur => roleDict.ContainsKey(ur.RoleId))
                    .Select(ur => new SysUserRoleDto
                    {
                        UserId = u.Id,
                        RoleId = ur.RoleId,
                        Code = roleDict[ur.RoleId].Code,
                        Name = roleDict[ur.RoleId].Name
                    }).ToList()
            }).ToList();

            foreach (var item in users)
            {
                item.StatusName = EnumHelper.GetEnumText(new ApproveStatusEnum(), item.Status);
            }

            return new PagedResult<AdminUserDto>(users, totalCount, request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// 查询管理员用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<AdminUserDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {

            var userInfo = await DbContext.Queryable<SysUser>()
           .Includes(u => u.Department).Includes(u => u.UserRoles)
           .Where(it => it.Id == request.Id).ToListAsync();

            var allUserRoleIds = userInfo.SelectMany(u => u.UserRoles ?? new List<SysUserRoleRelation>())
                .Select(ur => ur.RoleId)
                .Distinct()
                .ToList();
            var roles = await DbContext.Queryable<SysRole>()
                           .Where(r => allUserRoleIds.Contains(r.Id) && r.IsDeleted == 0)
                           .ToListAsync(cancellationToken);

            var roleDict = roles.ToDictionary(r => r.Id, r => r);

            // 转换为DTO
            var users = userInfo.Select(u => new AdminUserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                RealName = u.RealName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department?.Name,
                CreatedAt = u.CreatedAt,
                LastLoginTime = u.LastLoginTime,
                Description = u.Description,
                Status = u.Status,
                Sex = u.Sex,
                Roles = (u.UserRoles ?? new List<SysUserRoleRelation>())
                    .Where(ur => roleDict.ContainsKey(ur.RoleId))
                    .Select(ur => new SysUserRoleDto
                    {
                        UserId = u.Id,
                        RoleId = ur.RoleId,
                        Code = roleDict[ur.RoleId].Code,
                        Name = roleDict[ur.RoleId].Name
                    }).ToList()
            }).ToList();

            foreach (var item in users)
            {
                item.StatusName = EnumHelper.GetEnumText(new ApproveStatusEnum(), item.Status);
            }

            return new ResultObject<AdminUserDto>
            {
                code = 200,
                message = "查询成功",
                data = users?.FirstOrDefault(),
                success = true
            };
        }
    }
}
