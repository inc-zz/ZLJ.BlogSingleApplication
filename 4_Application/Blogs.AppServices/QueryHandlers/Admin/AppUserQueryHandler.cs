using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using Blogs.Domain.IRepositorys.Blogs;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    public class AppUserQueryHandler: SqlSugarDbContext,
        IRequestHandler<GetAppUserListQuery, PagedResult<BlogsUserDto>>
    {
        private readonly IAppUserRepository _appUserRepository;
        public AppUserQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        /// <summary>
        /// 执行查询App用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResult<BlogsUserDto>> Handle(GetAppUserListQuery request, CancellationToken cancellationToken)
        {
            var (list, totalCount) = await _appUserRepository.GetAppUserListAsync(
                request.PageIndex,
                request.PageSize,
                request.SortBy,
                request.SortDescending,
                cancellationToken);

            var resultList = list.Adapt<List<BlogsUserDto>>();
            return new PagedResult<BlogsUserDto>(resultList, totalCount, request.PageIndex, request.PageSize);

        }
    }
}
