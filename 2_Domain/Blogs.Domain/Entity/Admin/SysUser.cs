using Microsoft.AspNetCore.Identity;
using System;

namespace Blogs.Domain.Entity.Admin
{
    /// <summary>
    /// 用户表
    /// </summary> 
    [SugarTable("sys_user")]
    public class SysUser
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        public int AccessFailedCount { get; set; }
        public string? Email { get; set; }
        /////<summary>
        /////  账号
        /////</summary>
        public string? UserName { set; get; }
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
        public int?  Status { set; get; } 
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
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; protected set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; protected set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? ModifiedAt { get; protected set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? ModifiedBy { get; protected set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; protected set; }

        /// <summary>
        /// 创建信息
        /// </summary>
        /// <param name="byUser"></param>
        public void MarkAsCreated(string byUser)
        {
            CreatedAt = DateTime.Now;
            CreatedBy = byUser;
            IsDeleted = false;
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="byUser"></param>
        public void MarkAsModified(string byUser)
        {
            ModifiedAt = DateTime.Now;
            ModifiedBy = byUser;
        }
        /// <summary>
        /// 软删除
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = true;
        }


    }
}
