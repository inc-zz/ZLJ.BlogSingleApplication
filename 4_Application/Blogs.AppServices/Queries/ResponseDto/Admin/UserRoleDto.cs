using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 用户角色信息数据传输对象
    /// </summary>
    public class UserRoleDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 是否已分配给用户
        /// </summary>
        public bool IsAssigned { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Remark { get; set; }
    }
}
