using Blogs.Domain.ModelValidator.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysMenu
{

    /// <summary>
    /// 
    /// </summary>
    public class DeleteMenuCommand : MenuCommand
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public DeleteMenuCommand(long id)
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new DeleteMenuCommandValidation().Validate(this);
            return ValidationResult.IsValid;

        }
    }
}
