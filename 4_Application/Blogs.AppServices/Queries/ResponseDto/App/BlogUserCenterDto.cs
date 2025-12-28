using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.App
{
    public class BlogUserCenterDto
    {
        public string Account { get; set; }

        public string Summary { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        public string Tags { get; set; }

        public int ArticleCount { get; set; }   

        public int FinsCount { get; set; }

        public int ViewCount { get; set; }

        public int LikeCount { get; set; }

    }
}
