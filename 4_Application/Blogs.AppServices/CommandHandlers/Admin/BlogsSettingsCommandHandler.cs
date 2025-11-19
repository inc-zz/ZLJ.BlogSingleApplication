using Blogs.AppServices.Commands.Admin.SysConfig;
using Blogs.AppServices.Commands.Admin.SysDepartment;
using Blogs.Domain.Entity.Blogs;
using Blogs.Domain.Notices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.CommandHandlers.Admin
{
    /// <summary>
    /// 配置命令执行器
    /// </summary>
    public class BlogsSettingsCommandHandler : CommandHandler,
        IRequestHandler<CreateBlogSettingsCommand, bool>,
        IRequestHandler<UpdateBlogSettingsCommand, bool>,
        IRequestHandler<DeleteBlogSettingsCommand, bool>
    {

        public BlogsSettingsCommandHandler(DomainNotificationHandler notificationHandler,
            ILogger<BlogsSettingsCommandHandler> logger)
            : base(notificationHandler, logger)
        {
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(UpdateBlogSettingsCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            var blogConfig = await DbContext.Queryable<BlogsSettings>().Where(it => it.Id == command.Id).FirstAsync();
            blogConfig.SetEntity(command.Title, command.Summary, command.Url, command.Tags, command.BusType, command.Content, command.Status);
            blogConfig.CreatedAt = DateTime.Now;
            var result = await DbContext.Insertable(blogConfig).ExecuteCommandAsync();
            return result > 0;
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(DeleteBlogSettingsCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;
            var blogConfig = await DbContext.Queryable<BlogsSettings>().Where(it => it.Id == command.Id).FirstAsync();
            var result = await DbContext.Deleteable<BlogsSettings>().Where(it => it.Id == command.Id).ExecuteCommandAsync();
            return result > 0;
        }

        /// <summary>
        /// 新增配置
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Handle(CreateBlogSettingsCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;
            var blogConfig = command.Adapt<BlogsSettings>();
            blogConfig.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);
            var result = await DbContext.Insertable(blogConfig).ExecuteCommandAsync();
            return result > 0;
        }
    }
}
