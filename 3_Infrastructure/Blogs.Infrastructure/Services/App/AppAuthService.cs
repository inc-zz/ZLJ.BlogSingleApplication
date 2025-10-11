using Blogs.Core.Entity.Blogs;
using Blogs.Domain.IRepositorys.Blogs;
using Blogs.Domain.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blogs.Infrastructure.Services.App
{

    /// <summary>
    /// 认证服务
    /// </summary>
    public class AppAuthService : IAppAuthService
    {
        private readonly IAppUserRepository _userRepository;
        private readonly BlogsConfig _blogsConfig;
        private readonly JwtConfig _jwtConfig;

        public AppAuthService(
            IAppUserRepository userRepository,
            IOptions<JwtConfig> jwtConfig)
        {
            _userRepository = userRepository;
            _jwtConfig = jwtConfig.Value;
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<BlogsUser> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetFirstAsync(it=>it.Account == username);
            return user;
        }

        /// <summary>
        /// 验证用户凭据
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _userRepository.GetFirstAsync(it => it.Account == username);
            if (user == null)
            {
                return false;
            }
            return user.Password == AESCryptHelper.Encrypt(password);
        }

        /// <summary>
        /// 验证剩余登录尝试次数
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<int> GetRemainingLoginAttemptsAsync(string username)
        {
            var user = await _userRepository.GetFirstAsync(it => it.Account == username);
            if (user == null)
            {
                return 0;
            }
            // 这里假设最大尝试次数为5，实际应用中可以从配置中获取
            return 1;
        }

        /// <summary>
        /// 检查密码是否过期
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async  Task<bool> CheckPasswordExpiredAsync(long userId)
        {
            return false;
        }

        /// <summary>
        /// 生成JWT令牌
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateJwtToken(BlogsUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.SecurityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Account),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenExpires),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
