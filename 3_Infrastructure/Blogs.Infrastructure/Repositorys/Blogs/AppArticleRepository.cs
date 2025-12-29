using Blogs.Core.Entity.Blogs;
using Blogs.Domain.Entity.Blogs;
using Blogs.Domain.IRepositorys.Blogs;
using Blogs.Infrastructure.Context;
using NCD.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Repositorys.Blogs
{
    /// <summary>
    /// 应用文章仓储实现
    /// </summary>
    public class AppArticleRepository : SimpleClient<BlogsArticle>, IAppArticleRepository
    {

        private readonly SqlSugarDbContext dbContext;
        public AppArticleRepository()
        {
            dbContext = new SqlSugarDbContext();
            base.Context = dbContext.DbContext;
        }

        /// <summary>
        /// 写入文章数据
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> InsertArticleAsync(BlogsArticle model)
        {
            var result = await Context.Insertable(model).ExecuteCommandAsync();
            return result > 0;

        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateArticleAsync(BlogsArticle article)
        {
            var articleInfo = await Context.Queryable<BlogsArticle>().Where(it => it.Id == article.Id).FirstAsync();
            if(articleInfo == null)
            {
                throw new Exception("文章不存在");
            }
            articleInfo.SetArticleInfo(article.Id, article.CategoryId, article.Title, article.CoverImage, article.Summary, article.Tags, article.Content, article.ModifiedBy);
            var result = await Context.Updateable(articleInfo).ExecuteCommandAsync();
            return result > 0;
        }
    }
}
