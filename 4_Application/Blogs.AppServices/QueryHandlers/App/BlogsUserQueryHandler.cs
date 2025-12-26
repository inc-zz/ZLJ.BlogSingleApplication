using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Entity.Blogs;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.Entity.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.App
{
    /// <summary>
    /// App用户查询处理
    /// </summary>
    public class BlogsUserQueryHandler : SqlSugarDbContext,
        IRequestHandler<GetAppUserInfoQuery, ResultObject<BlogsUserDto>>,
        IRequestHandler<GetUserHomePageQuery, ResultObject<BlogUserCenterDto>>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<BlogsUserDto>> Handle(GetAppUserInfoQuery request, CancellationToken cancellationToken)
        {
            var userInfo = await DbContext.Queryable<BlogsUser>()
                 .Where(u => u.Id == request.Id)
                 .FirstAsync();

            var userData = userInfo.Adapt<BlogsUserDto>();
            userData.Status = userInfo.IsDeleted == 1 ? 0 : 1;
            userData.StatusName = userData.Status == 0 ? "禁用" : "启用";
            return new ResultObject<BlogsUserDto>
            {
                code = 200,
                message = "查询成功",
                data = userData,
                success = true
            };
        }

        /// <summary>
        /// 个人主页信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<BlogUserCenterDto>> Handle(GetUserHomePageQuery request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(request.userName))
            {
                return ResultObject<BlogUserCenterDto>.Success(new BlogUserCenterDto());
            }
            var userInfo = await DbContext.Queryable<BlogsUser>()
                 .Where(u => u.Account == request.userName)
                 .FirstAsync();

            var userData = userInfo.Adapt<BlogUserCenterDto>();
            userData.Tags = ""; // 个人标签，后续完善    
            userData.Summary = userInfo.Description;
            userData.Tags = userInfo.Tags;

            var userArticleList = await DbContext.Queryable<BlogsArticle>()
                .Where(a => a.CreatedBy == userInfo.Account && a.IsDeleted == 0)
                .ToListAsync();

            var articleTotle = userArticleList.Count;
            var likeTotal = userArticleList.Sum(a => a.LikeCount);
            var viewTotal = userArticleList.Sum(a => a.ViewCount);  
            userData.LikeCount = likeTotal;
            userData.ViewCount = viewTotal;
            userData.ArticleCount = articleTotle;
            userData.FinsCount = 0; // 关注人数，后续完善
            return new ResultObject<BlogUserCenterDto>
            {
                code = 200,
                message = "查询成功",
                data = userData,
                success = true
            };

        }
    }
}
