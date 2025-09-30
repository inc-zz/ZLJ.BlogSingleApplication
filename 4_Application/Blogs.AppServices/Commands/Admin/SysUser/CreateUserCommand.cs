using Blogs.AppServices.Requests.Admin;
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
        public CreateUserCommand(AddUserRequest param)
        {
            Id =  new IdWorkerUtils().NextId();
            UserName = param.UserName;
            Password =  AESCryptHelper.Encrypt(param.Password); //可选择非对称加密
            RealName = param.RealName;
            Description = param.Description;
            Email = param.Email;
            
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
