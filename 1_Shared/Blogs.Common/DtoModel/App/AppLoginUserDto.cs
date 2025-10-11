using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Common.DtoModel.App
{
    public class AppLoginUserDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? Account { get; set; }
        /// <summary>
        ///     手机号
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        public string? Description { get; set; }
    }
}
