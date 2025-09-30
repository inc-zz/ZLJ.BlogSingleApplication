using Blogs.AppServices.Requests.Admin;
using Blogs.Domain.ValueValidator.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 修改用户
    /// </summary>
    public class UpdateUserCommand:UserCommand
    {
        /// <summary>
        /// 初始化编辑字段
        /// </summary>
        public UpdateUserCommand(UpdateUserRequest param)
        {
            Id = param.Id;
            RealName = param.RealName;
            Sex = param.Sex;
            HeadPic = param.HeadPic;
            PhoneNumber = param.PhoneNumber;
            Email = param.Email;
            Status = param.Status;
            Description = param.Description;
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
