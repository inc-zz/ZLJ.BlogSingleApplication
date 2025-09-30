using Blogs.AppServices.Responses;
using Blogs.Core.Models;
using Blogs.Domain.ValueValidator.User;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysUser
{

    /// <summary>
    /// 用户登录命令
    /// </summary>
    public class UserLoginCommand : UserCommand, IRequest<ResultObject>
    {

        public UserLoginCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public override bool IsValid()
        {
            ValidationResult = new UserLoginCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
