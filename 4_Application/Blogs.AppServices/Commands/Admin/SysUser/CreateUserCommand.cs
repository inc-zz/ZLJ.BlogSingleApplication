using Blogs.Core;
using Blogs.Domain.ValueValidator.User;
using NCD.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 创建用户
    /// </summary>
    public class CreateUserCommand : UserCommand
    {

        /// <summary>
        /// 初始化新增用户
        /// </summary>
        public CreateUserCommand(long departmentId, string account, string password, string trueName)
        {
            Id =  new IdWorkerUtils().NextId();
            Account = account;
            Password =  AESCryptHelper.Encrypt(password); //可选择非对称加密
            TrueName = trueName;
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new CreateUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
