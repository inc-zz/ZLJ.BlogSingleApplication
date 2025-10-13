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
    /// 获取热门文章
    /// </summary>
    public class GetHotArticlesQuery : IRequest<ResultObject<List<ArticleCategoryDto>>>
    {
        public int TopCount { get; set; } = 4;

    }
}
