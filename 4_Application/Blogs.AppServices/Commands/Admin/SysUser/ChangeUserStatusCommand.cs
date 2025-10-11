using Blogs.AppServices.Requests.Admin;
using Blogs.Core.Enums;
using Blogs.AppServices.ModelValidator.Admin.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 禁用用户
    /// </summary>
    public class ChangeUserStatusCommand : UserCommand
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        public ChangeUserStatusCommand(ChangeStatusRequest request)
        {
            Id = request.Id;
            Status = (int)request.Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new DeleteUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
