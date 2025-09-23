using System;
using SqlSugar;

namespace Blogs.Core.Entity.Admin
{
    /// <summary>
    /// 用户表
    /// </summary> 
    [SugarTable("sys_user")]
    public class DbSysUser : BaseEntity
    {

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public int AccessFailedCount { get; set; }

        ///<summary>
        ///  账号
        ///</summary>
        //public string? Account { set; get; }
        ///<summary>
        ///  密码
        ///</summary>
        public string? Password { set; get; }
        ///<summary>
        ///  名称
        ///</summary>
        public string? RealName { set; get; }
        ///<summary>
        ///  状态
        ///</summary>
        public int? Status { set; get; }
        ///<summary>
        ///  邮箱
        ///</summary>
        //public string? Email { set; get; }
        ///<summary>
        ///  描述
        ///</summary>
        public string? Description { set; get; }
        /// <summary>
        /// 最后登录
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

    }
}
