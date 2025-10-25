using Blogs.Core.Entity.Blogs;
using Blogs.Domain.Entity.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.IRepositorys.Blogs
{
    /// <summary>
    /// 应用文章仓储接口
    /// </summary>
    public interface IAppArticleRepository : IBaseRepository<BlogsArticle>
    {
        /// <summary>
        /// 写入文章
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task<bool> InsertArticleAsync(BlogsArticle article);

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task<bool> UpdateArticleAsync(BlogsArticle article);

    }
}
