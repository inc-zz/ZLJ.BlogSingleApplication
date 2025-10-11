using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.App
{
    public class GetArticleDetailQuery : IRequest<ResultObject<ArticleDto>>
    {
        public GetArticleDetailQuery(long id)
        {
            this.ArticleId = id;    
        }

        public long ArticleId { get; set; }


    }
}
