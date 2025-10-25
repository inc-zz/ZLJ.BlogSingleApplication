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
    public class GetArticleListQuery : PagedRequest, IRequest<PagedResult<ArticleDto>>
    {

        public int? CategoryId {  get; set; }  

        public int? TagId { get; set; } 

    }
}
