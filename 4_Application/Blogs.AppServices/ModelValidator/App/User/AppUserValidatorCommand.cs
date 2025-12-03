using Blogs.AppServices.Commands.Blogs.User;
using Blogs.Domain.ServiceValidator.Admin;
using Blogs.Domain.ServiceValidator.App;
using Blogs.Infrastructure.ValidationServer.Admin;
using Blogs.Infrastructure.ValidationServer.App;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.App.User
{
    public class AppUserValidatorCommand<T> : AbstractValidator<T> where T : AppUserCommand
    {
        private readonly IAppUserValidatorService _userValidatorService;
        public AppUserValidatorCommand( )
        {
            _userValidatorService = new AppUserValidatorService();
        }

        protected void ValidateId()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("用户Id不能为空");
        }

        /// <summary>
        /// 验证账号
        /// </summary>
        protected void ValidateAccount()
        {
            RuleFor(x => x.Account).NotEmpty().WithMessage("账号不能为空");
        }
        /// <summary>
        /// 验证账号是否存在
        /// </summary>
        protected void ValidateAccountExists()
        {
            RuleFor(x => x).Must((x, cancellation) =>
            {
                var exists =  _userValidatorService.ExistsUser(x.Account);
                return !exists;
            }).WithMessage("账号已存在");
        }
        /// <summary>
        /// 验证密码
        /// </summary>
        protected void ValidatePassword()
        {
            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("密码不能为空")
            .MinimumLength(6).WithMessage("密码长度不能少于6位") 
            .Matches(@"^(?=.*[a-zA-Z])(?=.*\d)").WithMessage("密码必须包含字母和数字")
            .Matches(@"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]*$").WithMessage("密码包含非法字符")
            .Must(password => !IsCommonPassword(password)).WithMessage("密码过于简单，请使用更复杂的密码");
        }

        protected void ValidatePassword2()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("旧密码不能为空");
            RuleFor(x => x).Must((x, cancellation) =>
            {
                return x.Password == x.OldPassword;
            }).WithMessage("新密码与旧密码不能相同");
        }


        /// <summary>
        /// 验证简单密码模式
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool IsCommonPassword(string password)
        {
            var commonPasswords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "123456", "password", "12345678", "qwerty", "123456789",
                "12345", "1234", "111111", "1234567", "dragon",
                "123123", "admin", "welcome", "monkey", "password1"
            };

            return commonPasswords.Contains(password);
        }
    }
}
