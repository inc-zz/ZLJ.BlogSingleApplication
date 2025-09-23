using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Context
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUserDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        ///用户角色Id 
        /// </summary>
        public string? RoleIds { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string? Account { get; set; }
        /// <summary>
        /// Email 
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string? HeadPic { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public string? DepartmentId { get; set; }
        /// <summary>
        /// 是否超级管理员 超管控制器不鉴权 防止权限错误之后无法恢复
        /// </summary>
        public bool IsSuperAdmin { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public string? AuthorizeJson { get; set; }
    }
}
