using Blogs.AppServices.Commands.Admin.SysMenu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ModelValidator.Menu
{

    /// <summary>
    /// 修改菜单数据验证
    /// </summary>
    public class UpdateMenuCommandValidation : MenuValidatorCommand<UpdateMenuCommand>
    {
       
        /// <summary>
        /// 数据验证
        /// </summary>
        public UpdateMenuCommandValidation()
        {
            ValidateId();
            ValidateName();
            ValidateCode();
            ValidateButton();
            ValidateUrl();
        }

    }
}
