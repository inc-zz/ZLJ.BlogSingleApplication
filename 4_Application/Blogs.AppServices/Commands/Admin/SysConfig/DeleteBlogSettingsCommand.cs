using Blogs.AppServices.ModelValidator.Admin.SysConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysConfig
{
    public class DeleteBlogSettingsCommand : BlogSettingsCommand
    {

        public DeleteBlogSettingsCommand(long id)
        {
            this.Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteBlogSettingsValidatorCommand().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
