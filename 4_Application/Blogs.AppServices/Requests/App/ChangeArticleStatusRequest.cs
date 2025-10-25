using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    public class ChangeArticleStatusRequest
    {
        public long ArticleId { get; set; }
        /// <summary>
        /// 是否隐藏：1隐藏，2显示
        /// </summary>
        public int IsHide { get; set; }

    }
}
