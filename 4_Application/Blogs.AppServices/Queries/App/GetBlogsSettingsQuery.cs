using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.AppServices.Requests.App;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.App
{
    public class GetBlogsSettingsQuery : PagedRequest, IRequest<ResultObject<List<BlogsSettingDto>>>
    {

        public GetBlogsSettingsQuery(GetBlogsSettingRequest request)
        {
            this.SearchTerm = request.Where;
            this.PageIndex = request.PageIndex;
            this.PageSize = request.PageSize;
        }

    }
}
