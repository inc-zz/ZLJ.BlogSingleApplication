using Blogs.AppServices.Commands.Admin.SysConfig;
using Blogs.AppServices.Commands.Blogs.Settings;
using Blogs.Infrastructure.Constant;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.SysConfig
{
    public class BlogSettingsValidatorCommand<T> : AbstractValidator<T> where T : BlogSettingsCommand
    {

        protected void ValidateId()
        {
            RuleFor(it => it.Id).NotEmpty().WithName("配置项Id");
        }

        /// <summary>
        /// Title
        /// </summary>
        protected void ValidateTitle()
        {
            RuleFor(x => x.Title).NotEmpty().WithName("配置项[标题]");
        }
        /// <summary>
        /// Summary
        /// </summary>
        protected void ValidateSummary()
        {
            RuleFor(x => x.Summary).NotEmpty().WithName("配置项[简介]");
        }
        /// <summary>
        /// 存储路径
        /// </summary>
        protected void ValidateUrl()
        {
            RuleFor(x => x.Url).NotEmpty().WithName("配置项[存储路径]")
                .When(it => it.BusType == BlogsSettingBusType.FileStoreDictionary);
        }

    }
}
