using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Entity.Admin
{
    [SugarTable("sys_menu_button")]
    public class SysMenuButton : BaseEntity
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 按钮Id
        /// </summary>
        public long ButtonId { get; set; }
        /// <summary>
        /// 按钮排序
        /// </summary>
        public int SortOrder { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(SysMenuButton.ButtonId))]
        public List<SysButtons> MenuButtons { get; set; } = new List<SysButtons>();
    }
}
