using Blogs.AppServices.Commands.Blogs.User;
using Blogs.AppServices.ModelValidator.Blogs.User;
using Blogs.Core;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.AppUser
{
    /// <summary>
    /// 重置密码命令
    /// </summary>
    public class ResetAppUserPasswordCommand: AppUserCommand
    {

        public ResetAppUserPasswordCommand(long id,string password)
        {
            Id = id;
            NewPassword = AESCryptHelper.Encrypt(password);
        }   
        public string NewPassword { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new ResetAppUserPasswordCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
