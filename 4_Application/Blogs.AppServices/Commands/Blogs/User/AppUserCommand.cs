using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.User
{
    public abstract class AppUserCommand : Command
    {
        public long Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; protected set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; protected set; }
        /// <summary>
        /// Sex
        /// </summary>
        public int Sex { get; protected set; }
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; protected set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        public string? Avatar { get; protected set; }
        /// <summary>
        /// Mobile
        /// </summary>
        public string? PhoneNumber { get; protected set; }
        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; protected set; }


    }
}
