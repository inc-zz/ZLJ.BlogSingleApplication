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
        /// Id(批量删除)
        /// </summary>
        public long[] Ids { get; protected set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; protected set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string? UserName { get; protected set; }

        public string? OldPassword { get; protected set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; protected set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? RealName { get; protected set; }
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
        public string? PhoneNumber { get; protected set; }
        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; protected set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Description { get; protected set; }
        /// <summary>
        /// 用户角色菜单权限
        /// 1角色-多菜单
        /// </summary>
        public string? RoleMenuJson { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public string? UserRoleJson { get; set; }
        /// <summary>
        /// 用户部门
        /// </summary>
        public string? DepartmentJson { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public string? Authorizes { get; set; }

    }

}
