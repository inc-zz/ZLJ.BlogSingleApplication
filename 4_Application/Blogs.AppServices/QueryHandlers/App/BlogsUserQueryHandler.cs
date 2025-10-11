using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Entity.Blogs;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
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
        IRequestHandler<GetAppUserInfoQuery, ResultObject<BlogsUserDto>>
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

            return new ResultObject<BlogsUserDto>
            {
                code = 200,
                message = "查询成功",
                data = userInfo.Adapt<BlogsUserDto>(),
                success = true
            };

        }
    }
}
