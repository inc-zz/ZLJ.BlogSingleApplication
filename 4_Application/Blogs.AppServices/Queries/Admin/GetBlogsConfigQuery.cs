using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.Admin
{
    public class GetBlogsConfigQuery : IRequest<PagedResult<BlogsSettingsDto>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Where { get; set; }
        /// <summary>
        /// 配置项
        /// </summary>
        public string? Name { get; set; }
    }
}
