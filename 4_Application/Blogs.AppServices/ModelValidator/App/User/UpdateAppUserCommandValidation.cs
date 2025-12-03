using Blogs.AppServices.Commands.Blogs.User;
using Blogs.AppServices.ModelValidator.App.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Blogs.User
{

    /// <summary>
    /// 新增用户数据验证
    /// </summary>
    public class UpdateAppUserCommandValidation : AppUserValidatorCommand<AppUserCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public UpdateAppUserCommandValidation()
        {
            ValidateId();

        }
    }
}
