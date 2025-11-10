using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 菜单按钮数据传输对象
    /// </summary>
    public class MenuButtonDto
    {
        public long MenuId { get; set; }

        public long ButtonId { get; set; }
        /// <summary>
        /// 按钮代码
        /// </summary>
        public string? ButtonCode { get; set; }
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string? ButtonName { get; set; }
        /// <summary>
        /// 是否拥有权限
        /// </summary>
        public bool HasPermission { get; set; }
    }
}
