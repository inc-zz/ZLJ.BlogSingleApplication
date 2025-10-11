using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.App
{
    public class GetArticleCategoriesQuery: IRequest<ResultObject<List<ArticleCategoryDto>>>
    {
        public GetArticleCategoriesQuery(int count)
        {

            this.TopCount = count;
        }

        public int TopCount { get; protected set; }

    }
}
