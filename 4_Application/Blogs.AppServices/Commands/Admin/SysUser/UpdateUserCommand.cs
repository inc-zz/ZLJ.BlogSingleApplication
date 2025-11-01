using Blogs.AppServices.Requests.Admin;
using Blogs.AppServices.ModelValidator.Admin.User;
using System;
using System.Collections.Generic;
using System.Text;
using Blogs.Core;
using NCD.Common;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 修改用户
    /// </summary>
    public class UpdateUserCommand : UserCommand
    {
        /// <summary>
        /// 初始化编辑字段
        /// </summary>
        public UpdateUserCommand(UpdateUserRequest param)
        {
            Id = param.Id;
            RealName = param.RealName;
            Description = param.Description;
            DepartmentJson = param.DepartmentJson;
            UserRoleJson = param.UserRoleJson;
            PhoneNumber = param.PhoneNumber;
            Email = param.Email;
            Sex = param.Sex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new UpdateUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
