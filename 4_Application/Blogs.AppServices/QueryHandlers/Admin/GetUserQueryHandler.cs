using Blogs.AppServices.Queries.Admin;
using Blogs.Core.DtoModel.Admin;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    /// <summary>
    /// 用户模块查询处理器
    /// </summary>
    public class GetUserQueryHandler : SqlSugarDbContext,
        IRequestHandler<GetUserListQuery, PagedResult<AdminUserDto>>,
        IRequestHandler<GetUserInfoQuery, ResultObject<AdminUserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 查询管理员列表
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

        /// <summary>
        /// 查询管理员用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<AdminUserDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var userInfo = await DbContext.Queryable<SysUser>()
                .Where(u => u.Id == request.Id)
                .FirstAsync();

            return new ResultObject<AdminUserDto>
            {
                code = 200,
                message = "查询成功",
                data = userInfo.Adapt<AdminUserDto>(),
                success = true
            };
        }
    }
}
