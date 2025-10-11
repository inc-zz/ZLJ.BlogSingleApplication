using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.User
{
    public class ChangeAppUserStatusCommand : AppUserCommand, IRequest<bool>
    {
        public ChangeAppUserStatusCommand(long id)
        {
            this.Id = id;
        }
        public override bool IsValid()
        {
            return this.Id > 0;
        }
    }
}
