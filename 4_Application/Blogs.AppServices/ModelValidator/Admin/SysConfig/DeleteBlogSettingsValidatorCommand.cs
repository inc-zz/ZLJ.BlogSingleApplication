using Blogs.AppServices.Commands.Admin.SysConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.SysConfig
{
    public class DeleteBlogSettingsValidatorCommand : BlogSettingsValidatorCommand<BlogSettingsCommand>
    {
        /// <summary>
        /// 执行验证
        /// </summary>
        public DeleteBlogSettingsValidatorCommand()
        {
            ValidateId();
        }

    }
}
