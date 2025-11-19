using Blogs.AppServices.Commands.Admin.SysConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.SysConfig
{
    /// <summary>
    /// 更新配置验证器
    /// </summary>
    public class UpdateBlogSettingsValidatorCommand: BlogSettingsValidatorCommand<BlogSettingsCommand>
    {
        /// <summary>
        /// 执行验证
        /// </summary>
        public UpdateBlogSettingsValidatorCommand()
        {
            ValidateId();
            ValidateUrl();
            ValidateSummary();
            ValidateTitle();
        }

    }
}
