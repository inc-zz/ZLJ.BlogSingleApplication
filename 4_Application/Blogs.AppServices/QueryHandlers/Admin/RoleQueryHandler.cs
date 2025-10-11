using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.DtoModel.ResponseDto;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;
using Newtonsoft.Json;
using SqlSugar;
using System.Linq.Expressions;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    /// <summary>
    /// 
    /// </summary>f
    public class RoleQueryHandler : SqlSugarDbContext,
        IRequestHandler<GetRoleListQuery, PagedResult<RoleDto>>,
        IRequestHandler<GetAllRolesQuery, Dictionary<string, string>>,
        IRequestHandler<GetRoleInfoQuery, RoleDto>,
        IRequestHandler<GetRoleModulePermissionsQuery, List<RoleModulePermissionDto>>,
        IRequestHandler<GetUserRolesQuery, List<UserRoleDto>>,
        IRequestHandler<GetRoleUsersQuery, PagedResult<RoleUserDto>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RoleQueryHandler(
            IRoleRepository roleRepository,
            IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取角色列表（分页）
        /// </summary>
        public async Task<PagedResult<RoleDto>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            RefAsync<int> total = 0;
            var list = await DbContext.Queryable<SysRole>()
                .ToPageListAsync(request.PageIndex, request.PageSize, total, cancellationToken);
            var dtoList = list.Adapt<List<RoleDto>>();
            var result = new PagedResult<RoleDto>
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
        /// 获取所有角色（用于下拉选择）
        /// </summary>
        public async Task<Dictionary<string, string>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roleList = await DbContext.Queryable<SysRole>()
                .Where(x => x.Status == 1) // 只获取启用状态的角色
                .OrderBy(x => x.CreatedAt, OrderByType.Desc)
                .ToListAsync(cancellationToken);
            var result = roleList.ToDictionary(k => k.Code, v => v.Name);
            return result;
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        public async Task<RoleDto> Handle(GetRoleInfoQuery request, CancellationToken cancellationToken)
        {
            var role = await DbContext.Queryable<SysRole>()
                .Where(x => x.Id == request.Id)
                .FirstAsync();

            if (role == null)
            {
                throw new Exception("角色不存在");
            }

            return role.Adapt<RoleDto>();
        }

        /// <summary>
        /// 获取角色模块权限
        /// </summary>
        public async Task<List<RoleModulePermissionDto>> Handle(GetRoleModulePermissionsQuery request, CancellationToken cancellationToken)
        {
            // 获取所有模块
            var allModules = await DbContext.Queryable<SysMenu>()
                .Where(x => x.Status == 1)
                .OrderBy(x => x.Sort)
                .ToListAsync();

            var result = new List<RoleModulePermissionDto>();

            foreach (var module in allModules)
            {

            }

            return result;
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        public async Task<List<UserRoleDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            // 获取所有角色
            var allRoles = await DbContext.Queryable<SysRole>()
                .Where(x => x.Status == 1)
                .ToListAsync();

            // 获取用户已分配的角色


            return null;
        }

        /// <summary>
        /// 获取角色用户列表
        /// </summary>
        public async Task<PagedResult<RoleUserDto>> Handle(GetRoleUsersQuery request, CancellationToken cancellationToken)
        {
            RefAsync<int> total = 0;


            var result = new PagedResult<RoleUserDto>
            {
                code = 200,
                message = "获取成功",
                success = true,
                Items = null,
                Total = total.Value,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };

            return result;
        }
    }
}