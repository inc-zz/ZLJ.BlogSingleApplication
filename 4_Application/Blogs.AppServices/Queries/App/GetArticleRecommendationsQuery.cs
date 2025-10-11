using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.App
{
    /// <summary>
    /// 
    /// </summary>
    public class GetArticleRecommendationsQuery : IRequest<ResultObject<List<ArticleCategoryDto>>>
    {
        public GetArticleRecommendationsQuery()
        {
            
        }

        public int TopCount { get; set; }

        public int RecommendationType { get; set; }

    }
}
