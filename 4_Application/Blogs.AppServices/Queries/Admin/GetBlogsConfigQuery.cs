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
    public class GetBlogsSettingsQuery : IRequest<PagedResult<BlogsSettingsDto>>
    {
        /// <summary>
        /// 配置项
        /// </summary>
        public string? Name { get; set; }
    }
}
