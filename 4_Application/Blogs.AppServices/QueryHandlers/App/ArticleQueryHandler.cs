using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Blogs;
using Blogs.Infrastructure.Constant;
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
         IRequestHandler<GetArticleDetailQuery, ResultObject<ArticleInfoDto>>,
         IRequestHandler<GetArticleTagsQuery, ResultObject<List<ArticleTagDto>>>,
         IRequestHandler<GetHotArticlesQuery, ResultObject<List<ArticleCategoryDto>>>,
         IRequestHandler<GetArticleRecommendationsQuery, ResultObject<List<ArticleTagDto>>>,
         IRequestHandler<GetArticleListQuery, ResultObject<List<ArticleDto>>>,
         IRequestHandler<GetArticleCommentsQuery, ResultObject<List<BlogsCommentDto>>>
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

            var list = await DbContext.Queryable<BlogsCategory>()
                .Where(it => it.IsDeleted == 0 && it.BusType == CategoryBusType.ArticleType)
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
        public async Task<ResultObject<ArticleInfoDto>> Handle(GetArticleDetailQuery request, CancellationToken cancellationToken)
        {
            if (request.ArticleId == 0)
            {
                throw new Exception("参数错误");
            }

            var articleInfo = await DbContext.Queryable<BlogsArticle>()
                .Where(it => it.Id == request.ArticleId && it.IsDeleted == 0)
                .Select(it => new ArticleInfoDto
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

            return ResultObject<ArticleInfoDto>.Success(articleInfo, "");

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

        /// <summary>
        /// 获取技术板块
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<List<ArticleCategoryDto>>> Handle(GetHotArticlesQuery request, CancellationToken cancellationToken)
        {
            var list = await DbContext.Queryable<BlogsCategory>()
               .Where(it => it.IsDeleted == 0 && it.BusType == CategoryBusType.ArticleDomain) 
               .OrderByDescending(o => o.Sort)
               .Select(it => new ArticleCategoryDto
               {
                   Id = it.Id,
                   ArticleCount = 0,
                   Name = it.Name,
                   Description = it.Description,
                   Slug = it.Slug   
               })
               .Take(request.TopCount)
               .ToListAsync();

            return ResultObject<List<ArticleCategoryDto>>.Success(list, "");
        }

        /// <summary>
        /// 获取文章标签
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<List<ArticleTagDto>>> Handle(GetArticleRecommendationsQuery request, CancellationToken cancellationToken)
        {

            var list = await DbContext.Queryable<BlogsArticleTag>()
               .Where(it => it.IsDeleted == 0)
               .OrderByDescending(o => o.Sort)
               .Select(it => new ArticleTagDto
               {
                   Id = it.Id,
                   Name = it.Name,
                   StyleColor = it.Color
               })
               .Take(request.TopCount)
               .ToListAsync();

            return ResultObject<List<ArticleTagDto>>.Success(list, "");


        }
        /// <summary>
        ///首页文章分类列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<List<ArticleDto>>> Handle(GetArticleListQuery request, CancellationToken cancellationToken)
        {

            var list = await DbContext.Queryable<BlogsArticle>()
              .Where(it => it.IsDeleted == 0 && it.IsTop == true)
              .OrderByDescending(o => o.ViewCount)
              .Select(it => new ArticleDto
              {
                  Id = it.Id,
                  Title = it.Title,
                  Summary = it.Summary,
                  CoverImage = it.CoverImage,
                  ViewCount = it.ViewCount,
                  LikeCount = it.LikeCount,
                  CategoryName = SqlFunc.Subqueryable<BlogsCategory>().Where(c => c.Id == it.CategoryId).Select(c => c.Name),
                  CreatedBy = it.CreatedBy,
                  CreatedAt = it.CreatedAt
              })
              .ToListAsync();

            return ResultObject<List<ArticleDto>>.Success(list, "");

        }

        /// <summary>
        /// 文章评论列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<List<BlogsCommentDto>>> Handle(GetArticleCommentsQuery request, CancellationToken cancellationToken)
        {

            var list = await DbContext.Queryable<BlogsComment>()
                .Where(it => it.IsDeleted == 0 && it.ArticleId == request.ArticleId)
                .OrderBy(it => it.CreatedAt, OrderByType.Desc)
                .Select(it => new BlogsCommentDto
                {
                    Id = it.Id,
                    ArticleId = it.ArticleId,
                    Content = it.Content,
                    CreatedBy = it.CreatedBy,
                    CreatedAt = it.CreatedAt,
                    LikeCount = it.LikeCount,
                    Replies = SqlFunc.Subqueryable<BlogsComment>()
                    .Where(c => c.ParentId == it.Id).ToList()
                    .Select(c=>new BlogsCommentDto
                    {
                        Id = c.Id,
                        ArticleId=c.ArticleId,
                        Content =c.Content,
                        CreatedBy = c.CreatedBy,
                        CreatedAt = c.CreatedAt
                    }).ToList()
                }).ToListAsync();
            return ResultObject<List<BlogsCommentDto>>.Success(list, "");
        }
    }
}
