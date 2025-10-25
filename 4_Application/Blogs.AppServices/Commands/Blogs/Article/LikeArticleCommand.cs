using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Article
{

    /// <summary>
    /// 点赞文章命令
    /// </summary>
    public class LikeArticleCommand : ArticleCommand
    {
        public LikeArticleCommand(long id, bool isLike)
        {
            var likeCount = isLike ? 1 : -1;
            this.SetLikeCount(id, LikeCount, CurrentAppUser.Instance.UserInfo.UserName);
        }

        public override bool IsValid()
        {
            return this.Id > 0;
        }
    }
}
