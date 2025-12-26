using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.Admin;
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
         IRequestHandler<GetArticleRecommendationsQuery, ResultObject<List<BlogsSettingDto>>>,
         IRequestHandler<GetArticleListQuery, PagedResult<ArticleDto>>,
         IRequestHandler<GetArticleCommentsQuery, PagedResult<BlogsCommentDto>>,
        IRequestHandler<GetRelatedArticlesQuery, ResultObject<List<ArticleDto>>>,
        IRequestHandler<GetMyArticleListQuery, PagedResult<ArticleDto>>
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
                    Tags = it.Tags,
                    CreatedAt = it.CreatedAt,
                    ViewCount = it.ViewCount,
                    LikeCount = it.LikeCount
                }).FirstAsync();

            _ = SetArticleViewCountAsync(request.ArticleId);

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
        /// 获取推荐榜单
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<List<BlogsSettingDto>>> Handle(GetArticleRecommendationsQuery request, CancellationToken cancellationToken)
        {
            var list = await DbContext.Queryable<BlogsSettings>()
               .Where(it => it.IsDeleted == 0 && it.BusType == request.RecommendationType)
               .OrderBy(o => o.CreatedAt)
               .Select(it => new BlogsSettingDto
               {
                   Id = it.Id,
                   Tags = it.Tags,
                   Title = it.Title,
                   Content = it.Content,
                   Summary = it.Summary,
                   Url = it.Url
               })
               .Take(request.TopCount)
               .ToListAsync();

            return ResultObject<List<BlogsSettingDto>>.Success(list, "");
        }

        /// <summary>
        ///首页文章列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResult<ArticleDto>> Handle(GetArticleListQuery request, CancellationToken cancellationToken)
        {
            RefAsync<int> total = 0;
            var list = await DbContext.Queryable<BlogsArticle>()
              .Where(it => it.IsDeleted == 0 && it.IsTop == true)
              .WhereIF(request.CategoryId > 0, it => it.CategoryId == request.CategoryId)
              .WhereIF(!string.IsNullOrWhiteSpace(request.SearchTerm), wt => wt.Title.Contains(request.SearchTerm)
              || wt.Summary.Contains(request.SearchTerm) || wt.CreatedBy == request.SearchTerm)
              .WhereIF(!string.IsNullOrWhiteSpace(request.CreatedBy), it => it.CreatedBy == request.CreatedBy)
              .OrderByDescending(o => o.CreatedAt)
              .Select(it => new ArticleDto
              {
                  Id = it.Id,
                  Title = it.Title,
                  Summary = it.Summary,
                  CoverImage = it.CoverImage,
                  ViewCount = it.ViewCount,
                  LikeCount = it.LikeCount,
                  CommentCount = it.CommentCount,
                  CategoryName = SqlFunc.Subqueryable<BlogsCategory>().Where(c => c.Id == it.CategoryId).Select(c => c.Name),
                  CreatedBy = it.CreatedBy,
                  CreatedAt = it.CreatedAt,
                  Tags = it.Tags
              })
              .ToPageListAsync(request.PageIndex, request.PageSize, total, cancellationToken);
            var result = new PagedResult<ArticleDto>(list, total.Value, request.PageIndex, request.PageSize);

            return result;
        }

        /// <summary>
        /// 文章评论列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResult<BlogsCommentDto>> Handle(GetArticleCommentsQuery request, CancellationToken cancellationToken)
        {
            RefAsync<int> total = 0;
            var list = await DbContext.Queryable<BlogsComment>()
                .Where(it => it.IsDeleted == 0 && it.ParentId == 0 && it.ArticleId == request.ArticleId)
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
                    .Where(c => c.ParentId == it.Id && c.ParentId > 0).ToList()
                }).ToPageListAsync(request.PageIndex, request.PageSize, total);
            var result = new PagedResult<BlogsCommentDto>(list, total.Value, request.PageIndex, request.PageSize);
            return result;
        }

        /// <summary>
        /// 获取推荐文章
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<List<ArticleDto>>> Handle(GetRelatedArticlesQuery request, CancellationToken cancellationToken)
        {
            var articleInfo = await DbContext.Queryable<BlogsArticle>().Where(it => it.Id == request.ArticleId).FirstAsync();
            if (articleInfo == null)
            {
                return ResultObject<List<ArticleDto>>.Success(new List<ArticleDto>(), "");
            }

            var list = await DbContext.Queryable<BlogsArticle>().Where(BlogsArticle => BlogsArticle.IsDeleted == 0
                && BlogsArticle.Id != request.ArticleId
                && (BlogsArticle.CategoryId == articleInfo.CategoryId || BlogsArticle.Tags.Contains(articleInfo.Tags)))
                .OrderBy(it => it.ViewCount, OrderByType.Desc)
                .Take(request.TopCount)
                .Select(it => new ArticleDto
                {
                    Id = it.Id,
                    Title = it.Title,
                    Summary = it.Summary,
                    CoverImage = it.CoverImage,
                    ViewCount = it.ViewCount,
                    LikeCount = it.LikeCount,
                    CommentCount = it.CommentCount,
                    CategoryName = SqlFunc.Subqueryable<BlogsCategory>().Where(c => c.Id == it.CategoryId).Select(c => c.Name),
                    CreatedBy = it.CreatedBy,
                    CreatedAt = it.CreatedAt,
                    Tags = it.Tags
                }).ToListAsync();
            return ResultObject<List<ArticleDto>>.Success(list, "");
        }

        /// <summary>
        /// 获取我的文章列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResult<ArticleDto>> Handle(GetMyArticleListQuery request, CancellationToken cancellationToken)
        {
            RefAsync<int> total = 0;
            var list = await DbContext.Queryable<BlogsArticle>()
              .Where(it => it.CreatedBy == CurrentAppUser.Instance.UserInfo.UserName)
              .WhereIF(!string.IsNullOrWhiteSpace(request.SearchTerm), wt => wt.Title.Contains(request.SearchTerm)
              || wt.Summary.Contains(request.SearchTerm) || wt.CreatedBy == request.SearchTerm)
              .OrderByDescending(o => o.CreatedAt)
              .Select(it => new ArticleDto
              {
                  Id = it.Id,
                  Title = it.Title,
                  Summary = it.Summary,
                  CoverImage = it.CoverImage,
                  ViewCount = it.ViewCount,
                  LikeCount = it.LikeCount,
                  CommentCount = it.CommentCount,
                  CategoryName = SqlFunc.Subqueryable<BlogsCategory>().Where(c => c.Id == it.CategoryId).Select(c => c.Name),
                  CreatedBy = it.CreatedBy,
                  CreatedAt = it.CreatedAt,
                  Tags = it.Tags
              })
              .ToPageListAsync(request.PageIndex, request.PageSize, total, cancellationToken);
            var result = new PagedResult<ArticleDto>(list, total.Value, request.PageIndex, request.PageSize);

            return result;
        }

        /// <summary>
        /// 增加文章浏览量
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task SetArticleViewCountAsync(long articleId)
        {
            var articleInfo = await DbContext.Queryable<BlogsArticle>().Where(it => it.Id == articleId).FirstAsync();
            articleInfo.ViewCountPush();
            await DbContext.Updateable<BlogsArticle>(articleInfo)
                .ExecuteCommandAsync();
        }
    }
}
