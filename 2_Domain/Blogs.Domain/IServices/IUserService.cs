using Blogs.Domain.Entity.Admin;
using Blogs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.IServices
{
    /// <summary>
    /// 用户领域服务
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 读取分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //Task<DomainPageResult<SysUser>> GetListAsync(GetUserListQuery param, CancellationToken cancellationToken);


    }
}
