using Blogs.AppServices.Commands.Admin.SysMenu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ModelValidator.Menu
{

    /// <summary>
    /// 
    /// </summary>
    public class DeleteMenuCommandValidation: MenuValidatorCommand<DeleteMenuCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteMenuCommandValidation()
        {
            ValidateId();
        }

    }
}
