using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.App
{
    public class BlogsUserDto
    {
        /// <summary>
        ///  用户ID
        /// </summary>
        public long Id { set; get; }
        /// <summary>
        ///  登录账号
        /// </summary>
        public string? Account { set; get; }
        /// <summary>
        ///  真实姓名
        /// </summary>
        public string? TrueName { set; get; }
        /// <summary>
        ///  邮箱
        /// </summary>
        public string? Email { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreatedAt{ set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }
    }
}
