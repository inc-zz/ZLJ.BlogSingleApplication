using Blogs.Core.Entity.Blogs;

namespace Blogs.Domain.IServices
{
    /// <summary>
    /// 用户模块领域服务接口
    /// </summary>
    public interface IAppAuthService
    {
        Task<BlogsUser> ValidateUserAsync(string username, string password);
        Task<bool> ValidateCredentialsAsync(string username, string password);

        Task<int> GetRemainingLoginAttemptsAsync(string username);

        Task<bool> CheckPasswordExpiredAsync(long userId);
        string GenerateJwtToken(BlogsUser user);
    }
}
