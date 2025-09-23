using Blogs.Domain.ModelValidator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysMenu
{ 

    /// <summary>
    /// 设置菜单按钮命令
    /// </summary>
    public class PutMenuButtonCommand : MenuCommand
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="buttons"></param>
        public PutMenuButtonCommand(long menuId,List<MenuButtonCommand> buttons)
        {
            Id = menuId;
            Buttons = buttons;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new PutMenuButtonCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
