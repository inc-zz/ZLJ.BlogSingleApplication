using Blogs.AppServices.Queries.Admin;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Models;
using Blogs.Domain.IRepositorys.Admin;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    /// <summary>
    /// 查询用户数据列表的处理器
    /// </summary>
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PagedResult<AdminUserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserListQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedResult<AdminUserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            // 使用领域仓储的专门方法
            var (users, totalCount) = await _userRepository.GetUsersForListingAsync(
                request.PageIndex,
                request.PageSize,
                request.SearchTerm,
                request.IsActive,
            cancellationToken);

            //var userDtos = _mapper.Map<List<AdminUserDto>>(users);
            var userDtos = users.Adapt<List<AdminUserDto>>();
            return new PagedResult<AdminUserDto>(userDtos, totalCount, request.PageIndex, request.PageSize);
        }
    }
}
