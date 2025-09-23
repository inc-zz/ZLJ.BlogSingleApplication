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
        public UpdateUserCommand(long id,string trueName,int sex,string headPic,string mobile,string email,string summary)
        {
            Id = id;
            TrueName = trueName;
            Sex = sex;
            HeadPic = headPic;
            Mobile = mobile;
            Email = email;
            Summary = summary;
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
