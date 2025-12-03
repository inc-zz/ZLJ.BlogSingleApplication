using Blogs.AppServices.ModelValidator.Blogs.User;
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
        public UpdateAppUserCommand(long id,string email,string avatar, string phoneNumber,string remark)
        {
            Id = id;
            Email = email;
            Avatar = avatar;
            PhoneNumber = phoneNumber;
            Description = remark;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateAppUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
