using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.App
{
    public class BlogsSettingQueryHandler : SqlSugarDbContext,
        IRequestHandler<GetBlogsSettingsQuery, ResultObject<List<BlogsSettingDto>>>
    {

        public BlogsSettingQueryHandler()
        {

        }

        /// <summary>
        /// 查看配置列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultObject<List<BlogsSettingDto>>> Handle(GetBlogsSettingsQuery request, CancellationToken cancellationToken)
        {
            var list = await DbContext.Queryable<BlogsSettings>()
                .WhereIF(!string.IsNullOrEmpty(request.SearchTerm), it => it.Url == request.SearchTerm
                || it.Title == request.SearchTerm || it.Summary == request.SearchTerm
                || it.Content == request.SearchTerm)
                .Where(it => it.IsDeleted == 0)
                .Select(it => new BlogsSettingDto
                {
                    Id = it.Id,
                    Title = it.Title,
                    Content = it.Content,
                    Summary = it.Summary,
                    Url = it.Url,
                    Status = it.Status,
                    Tags = it.Tags
                }).ToListAsync();

            var result =  new ResultObject<List<BlogsSettingDto>>
            {
                code = 200,
                message = "查询成功",
                data = list,
                success = true
            };
            return result;
        }
    }
}
