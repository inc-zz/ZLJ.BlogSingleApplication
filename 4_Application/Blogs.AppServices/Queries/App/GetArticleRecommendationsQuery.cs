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
    /// 获取推荐榜单
    /// </summary>
    public class GetArticleRecommendationsQuery : IRequest<ResultObject<List<BlogsSettingDto>>>
    {
        public GetArticleRecommendationsQuery()
        {
            
        }

        public int TopCount { get; set; }

        public string RecommendationType { get; set; }

    }
}
