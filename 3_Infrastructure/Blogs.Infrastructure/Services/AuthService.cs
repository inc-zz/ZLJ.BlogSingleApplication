using Blogs.Core;
using Blogs.Core.Entity.Admin;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.IRepositorys.Admin;
using Blogs.Domain.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blogs.Infrastructure.Services
{

    /// <summary>
    /// 
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly BlogsConfig _blogsConfig;
        private readonly JwtConfig _jwtConfig;

        public AuthService(
            IUserRepository userRepository,
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
        public async Task<SysUser> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetFirstAsync(it=>it.UserName == username);
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
            var user = await _userRepository.GetFirstAsync(it => it.UserName == username);
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
            var user = await _userRepository.GetFirstAsync(it => it.UserName == username);
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
        public string GenerateJwtToken(SysUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.SecurityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Admin")
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
