using Blogs.AppServices.Requests.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.User
{
    public class UpdateAppUserCommand : AppUserCommand
    {
        public UpdateAppUserCommand(UpdateAppUserRequest param)
        {
            Id = param.Id;
            Email = param.Email;
            PhoneNumber = param.PhoneNumber;

        }

        public override bool IsValid()
        {
            return this.Id > 0;
        }
    }
}
