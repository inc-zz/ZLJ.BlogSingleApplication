using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Blogs;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.App
{
    /// <summary>
    /// 文章模块查询处理
    /// </summary>
    public class ArticleQueryHandler : SqlSugarDbContext,
         IRequestHandler<GetArticleCategoriesQuery, ResultObject<List<ArticleCategoryDto>>>,
         IRequestHandler<GetArticleDetailQuery, ResultObject<ArticleDto>>,
         IRequestHandler<GetArticleTagsQuery, ResultObject<List<ArticleTagDto>>>
    {

        /// <summary>
        /// 查询分类+统计
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<List<ArticleCategoryDto>>> Handle(GetArticleCategoriesQuery request, CancellationToken cancellationToken)
        {

            var list = await DbContext.Queryable<BlogsCategory>().Where(it => it.IsDeleted == 0)
                .Select(it => new ArticleCategoryDto
                {
                    Id = it.Id,
                    Name = it.Name,
                    ArticleCount = SqlFunc.Subqueryable<BlogsArticle>().Where(a => a.CategoryId == it.Id && a.IsDeleted == 0).Count()
                }).ToListAsync();

            return ResultObject<List<ArticleCategoryDto>>.Success(list, "");

        }
        /// <summary>
        /// 文章详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<ArticleDto>> Handle(GetArticleDetailQuery request, CancellationToken cancellationToken)
        {
            if (request.ArticleId == 0)
            {
                throw new Exception("参数错误");
            }

            var articleInfo = await DbContext.Queryable<BlogsArticle>()
                .Where(it => it.Id == request.ArticleId && it.IsDeleted == 0)
                .Select(it => new ArticleDto
                {
                    Id = it.Id,
                    Title = it.Title,
                    Summary = it.Summary,
                    CoverImage = it.CoverImage,
                    Content = it.Content,
                    CategoryId = it.CategoryId,
                    CategoryName = SqlFunc.Subqueryable<BlogsCategory>().Where(c => c.Id == it.CategoryId).Select(c => c.Name),
                    CreatedBy = it.CreatedBy,
                    CreatedAt = it.CreatedAt,
                    ViewCount = it.ViewCount,
                    LikeCount = it.LikeCount
                }).FirstAsync();

            return ResultObject<ArticleDto>.Success(articleInfo, "");

        }

        /// <summary>
        /// 获取文章标签列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<List<ArticleTagDto>>> Handle(GetArticleTagsQuery request, CancellationToken cancellationToken)
        {

            var tagList = await DbContext.Queryable<BlogsTag>()
                .Where(it => it.IsDeleted == 0)
                .OrderBy(it => it.UsageCount, OrderByType.Desc)
                .Take(request.TopCount)
                .Select(it => new ArticleTagDto
                {
                    Id = it.Id,
                    Name = it.Name,
                    UsageCount = it.UsageCount
                }).ToListAsync();

            return ResultObject<List<ArticleTagDto>>.Success(tagList, "");

        }


    }
}
