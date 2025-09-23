using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ValueValidator.User
{

    /// <summary>
    /// 用户登录
    /// </summary>
    public class UserLoginCommandValidation : UserValidatorCommand<UserCommand>
    {
        /// <summary>
        /// 调用用户登录时需要验证的字段
        /// </summary>
        public UserLoginCommandValidation()
        {
            ValidateAccount();
        }

    }
}
