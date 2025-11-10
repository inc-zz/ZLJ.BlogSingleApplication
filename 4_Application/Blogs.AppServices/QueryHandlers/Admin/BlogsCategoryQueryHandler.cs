using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Blogs;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    /// <summary>
    /// 文章分类查询处理器
    /// </summary>
    public class BlogsCategoryQueryHandler : SqlSugarDbContext,
       IRequestHandler<GetBlogsCategoryListQuery, PagedResult<BlogsCategoryDto>>,
       IRequestHandler<GetBlogsCategoryByIdQuery, BlogsCategoryDto>
    {
        public async Task<PagedResult<BlogsCategoryDto>> Handle(GetBlogsCategoryListQuery request, CancellationToken cancellationToken)
        {
            // 构建查询条件
            var query = DbContext.Queryable<BlogsCategory>()
                .WhereIF(!string.IsNullOrWhiteSpace(request.SearchTerm), c =>
                    c.Name.Contains(request.SearchTerm) || c.Description.Contains(request.SearchTerm))
                .Where(c => c.IsDeleted == 0);

            // 获取分页数据
            RefAsync<int> totalCount = new RefAsync<int>();
            var categories = await query
                .OrderBy(c => c.Sort)
                .OrderBy(c => c.CreatedAt, OrderByType.Desc)
                .Select(c => new BlogsCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Sort = c.Sort,
                    CreatedAt = c.CreatedAt,
                    CreatedBy = c.CreatedBy,
                    ModifiedAt = c.ModifiedAt,
                    ModifiedBy = c.ModifiedBy
                })
                .ToPageListAsync(request.PageIndex, request.PageSize, totalCount, cancellationToken);

            return new PagedResult<BlogsCategoryDto>(categories, totalCount.Value, request.PageIndex, request.PageSize);
        }

        public async Task<BlogsCategoryDto> Handle(GetBlogsCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await DbContext.Queryable<BlogsCategory>()
                .Where(c => c.Id == request.Id && c.IsDeleted == 0)
                .Select(c => new BlogsCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Sort = c.Sort,
                    CreatedAt = c.CreatedAt,
                    CreatedBy = c.CreatedBy,
                    ModifiedAt = c.ModifiedAt,
                    ModifiedBy = c.ModifiedBy
                })
                .FirstAsync(cancellationToken);

            return category ?? throw new Exception("文章分类不存在");
        }
    }
}
