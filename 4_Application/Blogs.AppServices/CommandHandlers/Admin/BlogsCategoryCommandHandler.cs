using Blogs.AppServices.Commands.Admin.Category;
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
    public class BlogsCategoryCommandHandler : CommandHandler,
     IRequestHandler<CreateBlogsCategoryCommand, bool>,
     IRequestHandler<UpdateBlogsCategoryCommand, bool>,
     IRequestHandler<DeleteBlogsCategoryCommand, bool>
    {
        public BlogsCategoryCommandHandler(
            DomainNotificationHandler notificationHandler,
            ILogger<BlogsCategoryCommandHandler> logger)
            : base(notificationHandler, logger)
        {
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateBlogsCategoryCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            // 检查分类名称是否重复
            var exists = await DbContext.Queryable<BlogsCategory>().Where(c => c.Name == command.Name && c.IsDeleted == 0).AnyAsync();
            if (exists)
            {
                await NotifyError("分类名称已存在");
                return false;
            }
            // 创建分类
            var category = new BlogsCategory(command.Name, command.Description, command.Sort);
            category.MarkAsCreated(CurrentUser.Instance.UserInfo.UserName);
            var result =  await DbContext.Insertable(category).ExecuteCommandAsync();
            _logger.LogInformation("文章分类创建成功: {CategoryName}(ID:{CategoryId})", category.Name, category.Id);
            return true;

        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateBlogsCategoryCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            return await ExecuteDbOperationAsync(async () =>
            {
                // 检查分类是否存在
                var category = await DbContext.Queryable<BlogsCategory>()
                    .Where(c => c.Id == command.Id && c.IsDeleted == 0)
                    .FirstAsync();

                if (category == null)
                {
                    await NotifyError("文章分类不存在");
                    return false;
                }

                // 检查分类名称是否重复（排除自身）
                var exists = await DbContext.Queryable<BlogsCategory>()
                    .Where(c => c.Name == command.Name && c.Id != command.Id && c.IsDeleted == 0)
                    .AnyAsync();

                if (exists)
                {
                    await NotifyError("分类名称已存在");
                    return false;
                }

                // 更新分类
                category.Update(command.Name, command.Description, command.Sort);
                category.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);

                var result = await DbContext.Updateable(category).ExecuteCommandAsync();

                _logger.LogInformation("文章分类更新成功: {CategoryName}(ID:{CategoryId})", category.Name, category.Id);
                return result > 0;
            }, "更新文章分类");
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteBlogsCategoryCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command))
                return false;

            return await ExecuteDbOperationAsync(async () =>
            {
                // 检查分类是否存在
                var category = await DbContext.Queryable<BlogsCategory>()
                    .Where(c => c.Id == command.Id && c.IsDeleted == 0)
                    .FirstAsync();

                if (category == null)
                {
                    await NotifyError("文章分类不存在");
                    return false;
                }

                // 检查是否有文章使用该分类
                var articleCount = await DbContext.Queryable<BlogsArticle>()
                    .Where(a => a.CategoryId == command.Id && a.IsDeleted == 0)
                    .CountAsync();

                if (articleCount > 0)
                {
                    await NotifyError($"该分类已被 {articleCount} 篇文章使用，无法删除");
                    return false;
                }

                // 软删除分类
                category.SoftDelete();
                category.MarkAsModified(CurrentUser.Instance.UserInfo.UserName);

                var result = await DbContext.Updateable(category)
                    .UpdateColumns(c => new { c.IsDeleted, c.ModifiedAt, c.ModifiedBy })
                    .ExecuteCommandAsync();

                _logger.LogInformation("文章分类删除成功: {CategoryName}(ID:{CategoryId})", category.Name, category.Id);
                return result > 0;
            }, "删除文章分类");
        }
    }
}
