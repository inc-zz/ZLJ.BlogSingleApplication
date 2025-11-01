using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysButtons
{
    /// <summary>
    /// 按钮基础命令参数
    /// </summary>
    public abstract class SysButtonsCommand : Command
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string ButtonType { get; set; } = "default";
        public string Position { get; set; } = "toolbar";
        public int SortOrder { get; set; }
        public int Status { get; set; } = 1;
    }

}
