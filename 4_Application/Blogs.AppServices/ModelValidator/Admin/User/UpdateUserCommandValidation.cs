using Blogs.AppServices.Commands.Admin.SysUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.User
{

    /// <summary>
    /// 修改用户数据验证
    /// </summary>
    public class UpdateUserCommandValidation : UserValidatorCommand<UserCommand>
    {

        /// <summary>
        /// 
        /// </summary>
        public UpdateUserCommandValidation()
        {
            ValidateId();
            ValidateName();
            ValidateMobile();
            ValidateSex();
            ValidateHeadPic();
        }

    }
}
