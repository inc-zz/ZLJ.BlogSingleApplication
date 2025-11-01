using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Responses
{
    /// <summary>
    /// 角色菜单授权数据传输对象
    /// </summary>
    public class RoleMenuAuthDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 授权的按钮代码列表
        /// </summary>
        public List<string> ButtonCodes { get; set; } = new List<string>();
    }
}
