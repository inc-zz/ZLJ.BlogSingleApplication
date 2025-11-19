using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.Entity.Blogs;
using Newtonsoft.Json;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    /// <summary>
    /// 系统配置项
    /// </summary>
    public class SysConfigQueryHandler : SqlSugarDbContext,
        IRequestHandler<GetBlogsConfigQuery, PagedResult<BlogsSettingsDto>>
    {
        private readonly IDatabase _redisCache;

        public SysConfigQueryHandler(IConnectionMultiplexer redis)
        {
            _redisCache = redis.GetDatabase();
        }

        /// <summary>
        /// 获取配置列表（分页）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResult<BlogsSettingsDto>> Handle(GetBlogsConfigQuery request, CancellationToken cancellationToken)
        {
            RefAsync<int> total = 0;
            var queryResult = await DbContext.Queryable<BlogsSettings>()
                .WhereIF(!string.IsNullOrWhiteSpace(request.Name), it => it.Title == request.Name)
                .ToPageListAsync(request.PageIndex, request.PageSize, total);
            var list = queryResult.Adapt<List<BlogsSettingsDto>>();
            foreach (var item in list)
            {
                item.StatusName = item.Status == 1 ? "启用" : "禁用";
            }
            var result = new PagedResult<BlogsSettingsDto>
            {
                code = 200,
                message = "获取成功",
                success = true,
                Items = list,
                Total = total.Value,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return result;
        }
    }
}
