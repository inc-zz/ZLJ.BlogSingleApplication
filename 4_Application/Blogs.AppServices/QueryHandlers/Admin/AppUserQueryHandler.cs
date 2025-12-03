using Blogs.AppServices.Queries.App;
using Blogs.AppServices.Queries.ResponseDto.App;
using Blogs.Core.Models;
using Blogs.Domain.IRepositorys.Blogs;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    public class AppUserQueryHandler : SqlSugarDbContext,
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
            var isDeleted = request.Status == 1 ? 0 : 1;
            var (list, totalCount) = await _appUserRepository.GetAppUserListAsync(
                request.PageIndex,
                request.PageSize,
                request.Where,
                isDeleted,
                cancellationToken);

            var resultList = list.Select(it => new BlogsUserDto
            {
                Id = it.Id,
                Account = it.Account,
                RealName = it.Account,
                Status = it.IsDeleted,
                Bio = it.Bio,
                Email = it.Email,
                LastLoginIp = it.LastLoginIp,
                LastLoginTime = it.LastLoginTime,
                CreatedAt = it.CreatedAt,
                CreatedBy = it.CreatedBy
            }).ToList();
            foreach (var item in resultList)
            {
                item.Status = item.Status == 1 ? 0 : 1;
                item.StatusName = item.Status == 0 ? "禁用" : "启用";
            }
            return new PagedResult<BlogsUserDto>(resultList, totalCount, request.PageIndex, request.PageSize);

        }
    }
}
