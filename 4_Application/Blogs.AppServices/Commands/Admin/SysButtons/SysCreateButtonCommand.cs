using Blogs.AppServices.ModelValidator.Admin.Buttons;
using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysButtons
{
    /// <summary>
    /// 创建按钮命令
    /// </summary>
    public class SysCreateButtonCommand : SysButtonsCommand
    {

        public SysCreateButtonCommand(CreateSysButtonRequest request)
        {
            this.Name = request.Name;
            this.Code = request.Code;
            this.Position = request.Positoin;
            this.Icon = request.Icon;
            this.Description = request.Description;
            this.SortOrder = request.Sort;
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(this.Code)
                && !string.IsNullOrWhiteSpace(this.Position);
        }
    } 
}
