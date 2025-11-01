using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    public class HomeStatisticsDto
    {
        public int UserCount { get; set; }

        public int ArticleCount { get; set; }   

        public int ArticleCommentCount { get; set; }    

        public int ArticleViewCount { get; set; }   

    }
}
