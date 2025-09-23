using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Entity.Blogs
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("blogs_user")]
    public class DbBlogsUser : BaseEntity
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 个人简介
        /// </summary>
        public string? Bio { get; private set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; private set; }
        /// <summary>
        /// 个人网站
        /// </summary>
        public string? Website { get; private set; }
        /// <summary>
        /// 文章数量
        /// </summary>
        public int ArticleCount { get; private set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public int FollowerCount { get; private set; }
        /// <summary>
        /// 关注数量
        /// </summary>
        public int FollowingCount { get; private set; } 

    }
}
