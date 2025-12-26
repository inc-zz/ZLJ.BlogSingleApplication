using Blogs.AppServices.Commands.Admin.SysMenu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.Menu
{

    /// <summary>
    /// 创建菜单数据验证
    /// </summary>
    public class CreateMenuCommandValidation: MenuValidatorCommand<CreateMenuCommand>
    {

        /// <summary>
        /// 数据验证
        /// </summary>
        public CreateMenuCommandValidation()
        {
            ValidateName();
            ValidateCode();
        }

    }
}
