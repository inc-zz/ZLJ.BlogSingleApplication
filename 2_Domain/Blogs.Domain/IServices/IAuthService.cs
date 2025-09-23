using Blogs.Domain.Entity.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.IServices
{
    /// <summary>
    /// 用户模块领域服务接口
    /// </summary>
    public interface IAuthService
    {
        Task<SysUser> ValidateUserAsync(string username, string password);
        Task<bool> ValidateCredentialsAsync(string username, string password);

        Task<int> GetRemainingLoginAttemptsAsync(string username);

        Task<bool> CheckPasswordExpiredAsync(long userId);
        string GenerateJwtToken(SysUser user);
    }
}
