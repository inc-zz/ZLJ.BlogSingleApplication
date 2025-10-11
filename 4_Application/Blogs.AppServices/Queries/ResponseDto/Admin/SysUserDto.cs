using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{

    /// <summary>
    /// 用户实体返回
    /// </summary>
    public class SysUserDto
    {

        /// <summary>
        ///  用户ID
        /// </summary>
        public long Id { set; get; }
        /// <summary>
        ///  登录账号
        /// </summary>
        public string Account { set; get; }
        /// <summary>
        ///  真实姓名
        /// </summary>
        public string TrueName { set; get; }
        /// <summary>
        /// 用户角色    
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        ///  角色
        /// </summary>
        public string RoleName { set; get; }
        /// <summary>
        ///  邮箱
        /// </summary>
        public string Email { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreateTime { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }

    }
}
