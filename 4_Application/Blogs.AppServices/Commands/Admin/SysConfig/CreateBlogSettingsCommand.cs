using Blogs.AppServices.ModelValidator.Admin.Menu;
using Blogs.AppServices.ModelValidator.Admin.SysConfig;
using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysConfig
{
    public class CreateBlogSettingsCommand: BlogSettingsCommand
    {
        public CreateBlogSettingsCommand(SetBlogsConfigRequest request)
        {
            this.Title = request.Title;
            this.Content = request.Content;
            this.Summary = request.Summary;
            this.Url = request.Url;
            this.Tags = request.Tags;
            this.BusType = request.BusType;
            this.Status = 1;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateBlogSettingsValidatorCommand().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
