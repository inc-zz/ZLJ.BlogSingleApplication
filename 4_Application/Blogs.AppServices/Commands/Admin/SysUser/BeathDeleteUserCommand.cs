using Blogs.AppServices.ModelValidator.Admin.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysUser
{
    /// <summary>
    /// 批量删除
    /// </summary>
    public class BeathDeleteUserCommand : UserCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public BeathDeleteUserCommand(long[] id)
        {
            Ids = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new BatchDeleteUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
