using Blogs.AppServices.Commands.Blogs.User;
using Blogs.AppServices.ModelValidator.Blogs.User;
using Blogs.AppServices.Requests.Admin;
using Blogs.Core;
using NCD.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.AppUser
{
    public class CreateAppUserCommand : AppUserCommand
    {
        /// <summary>
        /// 初始化新增用户
        /// </summary>
        public CreateAppUserCommand(CreateUserRequest param)
        {
            Id = new IdWorkerUtils().NextId();
            Account = param.Account;
            Password = AESCryptHelper.Encrypt(param.Password); //可选择非对称加密
            Email = param.Email;
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new CreateAppUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }


    }
}
