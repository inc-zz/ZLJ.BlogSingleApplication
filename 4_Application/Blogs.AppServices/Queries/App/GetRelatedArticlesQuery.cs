using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.App
{
    public class GetRelatedArticlesQuery : IRequest<ResultObject<List<ArticleDto>>>
    {

        public GetRelatedArticlesQuery(long articleId, int topCount)
        {
            this.ArticleId = articleId;
            this.TopCount = topCount;
        }

        public long ArticleId { get; private set; }
        public int TopCount { get;  private set; }

    }
}
