using Blogs.Domain;
using Blogs.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 用户信息
    /// </summary>
    public abstract class UserCommand : Command
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; protected set; }
        /// <summary>
        /// 租户
        /// </summary>
        public long TenantId { get; protected set; }
        /// <summary>
        /// 租户名称
        /// </summary>
        public string? TenantName { get; protected set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; protected set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; protected set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; protected set; }
        /// <summary>
        /// TrueName
        /// </summary>
        public string TrueName { get; protected set; }
        /// <summary>
        /// Sex
        /// </summary>
        public int Sex { get; protected set; }
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; protected set; }
        /// <summary>
        /// 头像路径格式
        /// </summary>
        public string? HeadPic { get; protected set; }
        /// <summary>
        /// Mobile
        /// </summary>
        public string? Mobile { get; protected set; }
        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; protected set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Summary { get; protected set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string? UpdateUser { get; protected set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string? UpdateUserName { get; protected set; }
        /// <summary>
        /// 用户角色菜单权限
        /// 1角色-多菜单
        /// </summary>
        public List<UserRoleMenu>? RoleMenus { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public List<UserRoleModel>? RoleList { get; set; }
        /// <summary>
        /// 用户部门
        /// </summary>
        public UserDepartment? Department { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public AuthorizeJson? Authorizes { get; set; }

    }

}
