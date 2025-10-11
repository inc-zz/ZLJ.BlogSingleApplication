using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.User
{
    public class ResetAppUserPasswordCommand: AppUserCommand
    {

        public ResetAppUserPasswordCommand(long id, string newPassword)
        {
            this.Id = id;
            this.NewPassword = newPassword;
        }   
        public string NewPassword { get; set; }
        public override bool IsValid()
        {
            return this.Id > 0 && !string.IsNullOrEmpty(this.NewPassword);
        }
    }
}
