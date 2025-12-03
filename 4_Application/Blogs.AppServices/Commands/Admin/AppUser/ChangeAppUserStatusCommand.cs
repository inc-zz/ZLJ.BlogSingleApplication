using Blogs.AppServices.Commands.Blogs.User;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.AppUser
{
    public class ChangeAppUserStatusCommand : AppUserCommand, IRequest<bool>
    {
        public ChangeAppUserStatusCommand(long id)
        {
            Id = id;
        }
        public override bool IsValid()
        {
            return Id > 0;
        }
    }
}
