using Blogs.Core.Entity.Admin;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;
using Blogs.Infrastructure.Responses;

namespace Blogs.Infrastructure.Repositorys.Admin
{
    /// <summary>
    /// 菜单操作
    /// </summary>
    public class MenuRepository : SimpleClient<SysMenu>, IMenuRepository
    {
        /// <summary>
        /// 获取菜单按钮权限
        /// </summary>
        /// <param name="id">菜单Id</param>
        /// <returns></returns>
        public async Task<ResultObject<SysMenuButtonsDto>> GetMenuButtonsAsync(long id)
        {
            //实际执行...
            throw new NotImplementedException();
        }

        /// <summary>
        /// 角色菜单
        /// </summary>
        /// <param name="roleIdRequest"></param>
        /// <returns></returns>
        public async Task<ResultObject<List<SysMenuPermissionsDto>>> GetMenuByRoleAsync(long roleId)
        {
            throw new NotImplementedException();
        }
    }
}
