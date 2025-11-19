using Blogs.AppServices.ModelValidator.Admin.SysConfig;
using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Admin.SysConfig
{
    /// <summary>
    /// 更新配置命令
    /// </summary>
    public class UpdateBlogSettingsCommand: BlogSettingsCommand
    {
        public UpdateBlogSettingsCommand(SetBlogsConfigRequest request)
        {
            this.Id = request.Id;
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
            ValidationResult = new UpdateBlogSettingsValidatorCommand().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
