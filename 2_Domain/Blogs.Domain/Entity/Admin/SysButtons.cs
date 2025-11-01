using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Entity.Admin
{
    /// <summary>
    /// 操作按钮
    /// </summary>  
    [SugarTable("sys_buttons")]
    public partial class SysButtons : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public override long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string ButtonType { get; set; } = "default";
        public string Position { get; set; } = "toolbar";
        public int SortOrder { get; set; } = 0;

    }
}
