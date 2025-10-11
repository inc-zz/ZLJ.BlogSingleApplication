using Blogs.AppServices.Commands.Admin.SysMenu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.Menu
{

    /// <summary>
    /// 设置菜单按钮数据验证
    /// </summary>
    public class PutMenuButtonCommandValidation : MenuValidatorCommand<PutMenuButtonCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public PutMenuButtonCommandValidation()
        {
            ValidateId();
            ValidateButton();
        }
    }
}
