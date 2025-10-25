using Blogs.AppServices.Requests.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Article
{
    /// <summary>
    /// 创建文章命令
    /// </summary>
    public class CreateArticleCommand : ArticleCommand
    {
        public CreateArticleCommand(CreateArticleRequest param)
        {
            this.SetArticleInfo(param.Id, param.Title, param.Summary, param.Content, param.Tags, param.CategoryId, null);
        }

        public override bool IsValid()
        {
            // 可修改为验证器验证
            return !string.IsNullOrEmpty(this.Title) && !string.IsNullOrEmpty(this.Content);
        }

    }
}
