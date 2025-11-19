using Blogs.AppServices.Commands.Admin.SysConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.SysConfig
{
   public class CreateBlogSettingsValidatorCommand : BlogSettingsValidatorCommand<BlogSettingsCommand>
    {
        public CreateBlogSettingsValidatorCommand()
        {
            ValidateUrl();
            ValidateSummary();
            ValidateTitle();
        }
    }
}
