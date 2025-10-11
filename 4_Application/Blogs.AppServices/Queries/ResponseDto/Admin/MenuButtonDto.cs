using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{

    /// <summary>
    /// 菜单功能按钮
    /// </summary>
    public class MenuButtonDto
    {
        /// <summary>
        /// BtnId
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 按钮Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string Name { get; set; } 
        /// <summary>
        /// 授权
        /// </summary>
        public bool Auth { get; set; }
    }
}
