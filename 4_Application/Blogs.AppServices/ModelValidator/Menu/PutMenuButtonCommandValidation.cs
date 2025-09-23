using Blogs.AppServices.Commands.Admin.SysMenu;
using Blogs.Domain.ModelValidator.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ModelValidator
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
