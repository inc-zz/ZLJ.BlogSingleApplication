using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.ResponseDto.App;
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
    public class BlogsCommentQueryHandler : SqlSugarDbContext,
    IRequestHandler<GetBlogsCommentListQuery, PagedResult<BlogsCommentDto>>
    {
        public async Task<PagedResult<BlogsCommentDto>> Handle(GetBlogsCommentListQuery request, CancellationToken cancellationToken)
        {
            // 构建查询条件
            var query = DbContext.Queryable<BlogsComment>()
                .LeftJoin<BlogsArticle>((c, a) => c.ArticleId == a.Id)
                .LeftJoin<BlogsComment>((c, a, p) => c.ParentId == p.Id)
                .WhereIF(!string.IsNullOrWhiteSpace(request.SearchTerm), c =>
                    c.Content.Contains(request.SearchTerm) || c.CreatedBy.Contains(request.SearchTerm))
                .WhereIF(request.Status.HasValue, c => c.Status == request.Status)
                .WhereIF(request.ArticleId.HasValue, c => c.ArticleId == request.ArticleId.Value)
                .Where(c => c.IsDeleted == 0);

            // 获取分页数据
            RefAsync<int> totalCount = new RefAsync<int>();
            var comments = await query
                .OrderBy(c => c.CreatedAt, OrderByType.Desc)
                .Select((c, a, p) => new BlogsCommentDto
                {
                    Id = c.Id,
                    ArticleId = c.ArticleId,
                    Content = c.Content,
                    CreatedBy = c.CreatedBy,
                    LikeCount = c.LikeCount,
                    CreatedAt = c.CreatedAt
                })
                .ToPageListAsync(request.PageIndex, request.PageSize, totalCount, cancellationToken);

            return new PagedResult<BlogsCommentDto>(comments, totalCount.Value, request.PageIndex, request.PageSize);
        }
    }
}
