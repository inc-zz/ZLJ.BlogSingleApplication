using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.AppServices.Queries.ResponseDto.App;
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
        IRequestHandler<GetAllRolesQuery, ResultObject<List<DropDownlistDto>>>,
        IRequestHandler<GetRoleInfoQuery, ResultObject<RoleDto>>,
        IRequestHandler<GetRoleModulePermissionsQuery, ResultObject<List<RoleMenuPermissionDto>>>,
        IRequestHandler<GetUserRolesQuery, ResultObject<List<UserRoleDto>>>,
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
            // 构建查询条件
            var query = DbContext.Queryable<SysRole>()
                .WhereIF(!string.IsNullOrWhiteSpace(request.SearchTerm), r =>
                    r.Name.Contains(request.SearchTerm) || r.Code.Contains(request.SearchTerm))
                .WhereIF(request.Status.HasValue, r => r.Status == request.Status.Value)
                .Where(r => r.IsDeleted == 0);

            // 获取分页数据
            RefAsync<int> totalCount = new RefAsync<int>();
            var roles = await query
                .OrderBy(r => r.CreatedAt, OrderByType.Desc)
                .ToPageListAsync(request.PageIndex, request.PageSize, totalCount, cancellationToken);

            // 转换为DTO并补充额外信息
            var roleDtos = roles.Select(role => new RoleDto
            {
                Id = role.Id,
                IsSystem = role.IsSystem, 
                Name = role.Name,
                Code = role.Code,
                Remark = role.Remark,
                Status = role.Status,
                CreatedAt = role.CreatedAt,
                CreatedBy = role.CreatedBy,
                ModifiedAt = role.ModifiedAt,
                ModifiedBy = role.ModifiedBy,
                IsDeleted = role.IsDeleted == 1,
                StatusName = role.Status == 1 ? "启用" : "禁用", 
            }).ToList();

            return new PagedResult<RoleDto>(roleDtos, totalCount.Value, request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// 获取所有启用的角色（用于下拉选择）
        /// </summary>
        /// <param name="request">查询请求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色下拉列表数据</returns>
        public async Task<ResultObject<List<DropDownlistDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roleList = await DbContext.Queryable<SysRole>()
                .Where(x => x.Status == 1 && x.IsDeleted == 0)
                .OrderBy(x => x.CreatedAt, OrderByType.Desc)
                .Select(it => new DropDownlistDto
                {
                    Id = it.Id,
                    Code = it.Code,
                    Name = it.Name
                })
                .ToListAsync(cancellationToken);

            return ResultObject<List<DropDownlistDto>>.Success(roleList, "获取成功");
        }
        /// <summary>
        /// 获取角色详细信息
        /// </summary>
        /// <param name="request">查询请求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色详细信息</returns>
        public async Task<ResultObject<RoleDto>> Handle(GetRoleInfoQuery request, CancellationToken cancellationToken)
        {
            var role = await DbContext.Queryable<SysRole>()
                .Where(x => x.Id == request.Id && x.IsDeleted == 0)
                .FirstAsync();

            if (role == null)
            {
                return ResultObject<RoleDto>.Error(null, "角色不存在");
            }

            // 获取上级角色名称
            var parentName = string.Empty;
            if (role.ParentId > 0)
            {
                parentName = await DbContext.Queryable<SysRole>()
                    .Where(r => r.Id == role.ParentId)
                    .Select(r => r.Name)
                    .FirstAsync() ?? string.Empty;
            }
            var roleInfo = new RoleDto
            {
                Id = role.Id,
                IsSystem = role.IsSystem,
                Name = role.Name,
                Code = role.Code,
                Remark = role.Remark,
                Status = role.Status,
                CreatedAt = role.CreatedAt,
                CreatedBy = role.CreatedBy,
                ModifiedAt = role.ModifiedAt,
                ModifiedBy = role.ModifiedBy,
                IsDeleted = role.IsDeleted == 1,
                StatusName = role.Status == 1 ? "启用" : "禁用"
            };

            return ResultObject<RoleDto>.Success(roleInfo, "获取成功");
        }

        /// <summary>
        /// 获取角色模块权限树
        /// 构建树形结构的菜单权限数据，包含按钮级权限
        /// </summary>
        /// <param name="request">查询请求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色权限树形数据</returns>
        public async Task<ResultObject<List<RoleMenuPermissionDto>>> Handle(GetRoleModulePermissionsQuery request, CancellationToken cancellationToken)
        {
            // 获取所有菜单
            var allMenus = await DbContext.Queryable<SysMenu>()
                .Where(x => x.Status == 1)
                .OrderBy(x => x.Sort)
                .ToListAsync();

            // 获取角色已有的权限
            var rolePermissions = await DbContext.Queryable<SysRoleMenuAuth>()
                .Where(x => x.RoleId == request.RoleId)
                .ToListAsync();

            var result = new List<RoleMenuPermissionDto>();

            foreach (var menu in allMenus)
            {
                var rolePermission = rolePermissions.FirstOrDefault(x => x.MenuId == menu.Id);
                var buttonPermissions = new List<string>();

                if (rolePermission != null && !string.IsNullOrEmpty(rolePermission.ButtonPermissions))
                {
                    buttonPermissions = rolePermission.ButtonPermissions.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                result.Add(new RoleMenuPermissionDto
                {
                    MenuId = menu.Id,
                    MenuName = menu.Name,
                    MenuUrl = menu.Url,
                    ParentId = menu.ParentId,
                    HasPermission = rolePermission != null,
                    ButtonPermissions = buttonPermissions
                });
            }
            return ResultObject<List<RoleMenuPermissionDto>>.Success(result, "");
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        public async Task<ResultObject<List<UserRoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            // 获取所有角色
            var allRoles = await DbContext.Queryable<SysRole>()
                .Where(x => x.Status == 1)
                .ToListAsync();

            // 获取用户已分配的角色
            var userRoles = await DbContext.Queryable<SysUserRoleRelation>()
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.RoleId)
                .ToListAsync();

            var result = allRoles.Select(role => new UserRoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name,
                RoleCode = role.Code,
                IsAssigned = userRoles.Contains(role.Id)
            }).ToList();

            return ResultObject<List<UserRoleDto>>.Success(result, "");
        }

        /// <summary>
        /// 获取角色用户列表
        /// </summary>
        public async Task<PagedResult<RoleUserDto>> Handle(GetRoleUsersQuery request, CancellationToken cancellationToken)
        {
            var query = DbContext.Queryable<SysUser>()
            .LeftJoin<SysUserRoleRelation>((u, ur) => u.Id == ur.UserId)
            .Where((u, ur) => ur.RoleId == request.RoleId && u.IsDeleted == false)
            .Select((u, ur) => new RoleUserDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                RealName = u.RealName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                DepartmentName = SqlFunc.Subqueryable<SysDepartment>()
                    .Where(d => d.Id == u.DepartmentId)
                    .Select(d => d.Name)
            });

            RefAsync<int> total = 0;
            var users = await query.OrderBy(u => u.CreatedAt, OrderByType.Desc)
                .ToPageListAsync(request.PageIndex, request.PageSize, total, cancellationToken);

            return new PagedResult<RoleUserDto>
            {
                Items = users,
                Total = total.Value,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }
    }
}