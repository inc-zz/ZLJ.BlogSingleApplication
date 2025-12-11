using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.DtoModel.Admin
{
    /// <summary>
    /// 管理员列表DTO
    /// </summary>
    public class AdminUserDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? RealName { get; set; }
        /// <summary>
        ///     手机号
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }
        public long? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public List<SysUserRoleDto> Roles { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int? Sex { get; set; }

        public string? StatusName { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        public string? Description { get; set; }
    }

    public class SysUserRoleDto
    {
        public long? UserId { get; set; }
        public long RoleId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
