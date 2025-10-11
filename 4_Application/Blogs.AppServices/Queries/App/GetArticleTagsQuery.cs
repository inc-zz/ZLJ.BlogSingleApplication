using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.App
{
    public class GetArticleTagsQuery : IRequest<ResultObject<List<ArticleTagDto>>>
    {
        public GetArticleTagsQuery()
        {
            
        }


        public int TopCount { get; set; } = 5;
    }
}
