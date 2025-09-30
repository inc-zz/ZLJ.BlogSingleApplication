using Blogs.Common.Models;
using Blogs.Core.Models;
using Blogs.Domain;
using Blogs.Domain.Entity.Admin;
using Blogs.Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenIddict.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blogs.Infrastructure.Services
{
    public class OpenIddictService : IOpenIddictService
    {
        private readonly IDatabase _redisDb;
        private readonly IConfiguration _configuration;
        private readonly JwtConfig _jwtConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SqlSugarClient _dbContext;
        private ILogger<OpenIddictService> _logger;

        public OpenIddictService(
            IConnectionMultiplexer redis,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtConfig> jwtConfigOptions,
            ILogger<OpenIddictService> logger)
        {
            _redisDb = redis.GetDatabase(1);
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _jwtConfig = jwtConfigOptions?.Value ?? throw new ArgumentNullException(nameof(jwtConfigOptions));
            _logger = logger;
            _dbContext = new SqlSugarDbContext().DbContext;
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<TokenResult> GenerateTokenAsync(SysUser user, string refreshToken = "")
        {
            try
            {
                // 创建身份主体
                var identity = new ClaimsIdentity(
                authenticationType: "Bearer", // 使用通用的认证类型
                nameType: OpenIddictConstants.Claims.Name,
                roleType: OpenIddictConstants.Claims.Role);

                // 添加标准声明
                identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id);
                identity.AddClaim(OpenIddictConstants.Claims.Email, user.Email);
                identity.AddClaim(OpenIddictConstants.Claims.Name, user.UserName);
                identity.AddClaim(OpenIddictConstants.Claims.PreferredUsername, user.UserName);

                // 添加角色声明
                identity.AddClaim(OpenIddictConstants.Claims.Role, "admin");

                var principal = new ClaimsPrincipal(identity);

                // 设置范围
                var scopes = new[]
                {
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Roles,
                    OpenIddictConstants.Scopes.OfflineAccess
                };

                principal.SetScopes(scopes);
                // 设置资源
                principal.SetResources("resource_server");

                // 生成访问令牌 - 使用 OpenIddict 7.0.0 的方式
                var accessToken = GenerateAccessTokenManually(principal, user);

                // 刷新令牌
                if (string.IsNullOrEmpty(refreshToken))
                    refreshToken = Guid.NewGuid().ToString("N");

                if (string.IsNullOrEmpty(accessToken))
                {
                    return TokenResult.Error("访问令牌生成失败");
                }

                // 将令牌存储到 Redis
                var userId = user.Id.ToString();
                await StoreTokensInRedisAsync(principal, userId, accessToken, refreshToken);

                return TokenResult.Success(accessToken, refreshToken, DateTime.UtcNow.AddHours(1));
            }
            catch (Exception ex)
            {
                return TokenResult.Error($"令牌生成失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证Token是否有效
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ValidateJwtToken(string token)
        {
            if (_redisDb == null) return true; // 如果没有 Redis，默认验证通过
            try
            {
                // 从令牌中提取用户ID
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(userId)) return false;

                var userTokenKey = $"user_tokens:{userId}";
                var hashEntries = await _redisDb.HashGetAllAsync(userTokenKey);
                if (hashEntries.Length == 0)
                {
                    return false;
                }

                // 将 HashEntry 数组转换为字典以便更容易访问
                var hashDict = hashEntries.ToDictionary(
                    entry => entry.Name.ToString(),
                    entry => entry.Value.ToString());

                // 检查必要字段是否存在
                if (!hashDict.ContainsKey("data") || string.IsNullOrEmpty(hashDict["data"]))
                {
                    return false; // 数据字段不存在或为空
                }

                // 反序列化 JSON 数据
                var userData = JsonConvert.DeserializeObject<UserTokenData>(hashDict["data"]);

                //检查访问令牌是否与当前请求令牌相同(防止多设备登录)
                return userData.AccessToken == token;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 注销Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> RevokeJwtToken(string token, string refreshToken)
        {
            if (_redisDb == null) return true;

            try
            {
                // 从令牌中提取用户ID
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(userId)) return false;

                var redisKey = $"user_tokens:{userId}";
                // 从 Redis 中删除令牌
                await _redisDb.KeyDeleteAsync(redisKey);

                var refrshTokenKey = $"refresh_token:{refreshToken}";
                await _redisDb.KeyDeleteAsync(refrshTokenKey);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserTokenModel JwtTokenParser(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("访问令牌为空");
            }
            var jwtConfig = _configuration.GetSection("JwtConfig");
            var issuer = jwtConfig.GetValue<string>("Issuer");
            var audience = jwtConfig.GetValue<string>("Audience");
            var securityKey = jwtConfig.GetValue<string>("SecurityKey");
            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(securityKey))
            {
                throw new ArgumentNullException("JwtConfig 配置不完整");
            }
            var parser = new JwtTokenParser(issuer, audience, securityKey);

            try
            {
                var descriptor = parser.ParseToken(token);
                Console.WriteLine("SecurityTokenDescriptor 创建成功");
                Console.WriteLine($"过期时间: {descriptor.Expires}");
                Console.WriteLine($"签发者: {descriptor.Issuer}");
                Console.WriteLine($"受众: {descriptor.Audience}");
                Console.WriteLine($"声明数量: {descriptor.Claims.Count()}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"解析失败: {ex.Message}");
            }

            var handler = new JwtSecurityTokenHandler();

            // 只解析Token，不验证
            var jwtToken = handler.ReadJwtToken(token);

            var tokenModel = new UserTokenModel
            {
                Id = GetClaimValue(jwtToken, "sub"),
                Name = GetClaimValue(jwtToken, "name"),
                Email = GetClaimValue(jwtToken, "email"),
                Role = GetClaimValue(jwtToken, "role"),
                Jti = GetClaimValue(jwtToken, "jti"),
                IssuedAt = UnixTimeStampToDateTime(GetClaimValue(jwtToken, "iat")),
                NotBefore = UnixTimeStampToDateTime(GetClaimValue(jwtToken, "nbf")),
                Expiration = UnixTimeStampToDateTime(GetClaimValue(jwtToken, "exp")),
                Issuer = jwtToken.Issuer,
                Audience = jwtToken.Audiences.FirstOrDefault()

            };
            Console.WriteLine("=====================================");
            Console.WriteLine(tokenModel);

            return tokenModel;
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        private string GenerateAccessTokenManually(ClaimsPrincipal principal, SysUser user)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:SecurityKey"]);
                var actualExpireMinutes = _jwtConfig.TokenExpires;
                var now = DateTime.UtcNow;
                var expires = now.AddMinutes(actualExpireMinutes);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(now).ToString(), ClaimValueTypes.Integer64),
                    // 后期扩展，根据用户多角色添加多个角色
                    new Claim("role", "admin")
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expires,
                    Issuer = _configuration["JwtConfig:Issuer"],
                    Audience = _configuration["JwtConfig:Audience"],
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = handler.CreateToken(tokenDescriptor);
                return handler.WriteToken(token);
            }
            catch (Exception e)
            {
                // 建议记录异常日志（如使用ILogger），而非直接抛出
                // _logger.LogError(e, "生成JWT令牌失败");
                throw;
            }
        }

        /// <summary>
        /// 存储用户Token信息到Redis
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task StoreTokensInRedisAsync(ClaimsPrincipal principal, string userId, string accessToken, string refreshToken)
        {
            // 构建数据对象
            var userData = new UserTokenData
            {
                UserId = userId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = principal.FindFirstValue(ClaimTypes.Name) ?? "",
                Role = principal.FindFirstValue(ClaimTypes.Role) ?? "user"
            };

            // 使用 JSON 序列化确保类型安全
            var jsonData = JsonConvert.SerializeObject(userData);

            // 存储到 Redis（使用 Hash 结构） 
            var refreshTokenExpires = _jwtConfig.RefreshTokenExpires;
            var userTokenKey = $"user_tokens:{userId}";
            await _redisDb.HashSetAsync(userTokenKey,
            [
                new HashEntry("data", jsonData),
                new HashEntry("refresh_token", refreshToken),
                new HashEntry("expires_at", DateTime.UtcNow.AddMinutes(refreshTokenExpires).ToString("o"))
            ]);

            // 设置整体过期时间
            await _redisDb.KeyExpireAsync(userTokenKey, TimeSpan.FromDays(7));

        }

        /// <summary>
        /// 获取声明值
        /// </summary>
        /// <param name="token"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        private static string GetClaimValue(JwtSecurityToken token, string claimType)
        {
            return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
        /// <summary>
        /// 时间戳转DateTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        private static DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            if (string.IsNullOrWhiteSpace(unixTimeStamp) || !long.TryParse(unixTimeStamp, out long seconds))
                return DateTime.MinValue;

            return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns>当前用户信息</returns>
        public async Task<CacheUserModel> GetCurrentUserAsync()
        {
            try
            {
                var token = ExtractTokenFromHeader();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("未找到有效的认证令牌");
                    return null;
                }

                // 从Redis获取用户信息
                var userTokenModel = JwtTokenParser(token);
                var userCacheKey = $"user_caches:{userTokenModel.Id}";

                var userInfo = await _redisDb.HashGetAsync(userCacheKey, "userInfo");
                if (!userInfo.HasValue)
                {
                    //首次读取如果缓存未命中则查询数据库
                    var user = await _dbContext.Queryable<SysUser>().Where(it => it.Id.ToString() == userTokenModel.Id).FirstAsync();
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    var userModel = new JwtUserModel
                    {
                        UserId = user.Id.ToString(),
                        Role = "admin",
                        UserName = user.UserName,
                        Email = user.Email,
                        Description = user.Description
                    };

                    var userTokenModelCache = new CacheUserModel
                    {
                        UserId = user.Id,
                        UserInfo = userModel,
                        AuthorizeJson = "", //权限配置
                        IsSuperAdmin = true,
                    };
                    await _redisDb.HashSetAsync(userCacheKey,
                     [
                        new HashEntry("userInfo", JsonConvert.SerializeObject(userTokenModelCache)),
                        new HashEntry("expires_at", DateTime.UtcNow.AddDays(30).ToString("o"))
                     ]);

                    return userTokenModelCache;
                }

                // 从Redis中获取Hash中的所有字段
                var hashEntries = await _redisDb.HashGetAllAsync(userCacheKey);
                if (hashEntries.Length == 0)
                    throw new UnauthorizedAccessException("获取登录信息失败");
                // 将HashEntries转换为字典
                var hashDict = hashEntries.ToStringDictionary();
                // 尝试获取userInfo字段
                if (!hashDict.ContainsKey("userInfo"))
                    throw new UnauthorizedAccessException("获取登录信息失败");
                // 反序列化userInfo字段
                var userInfoJson = hashDict["userInfo"];
                var userCacheData = JsonConvert.DeserializeObject<CacheUserModel>(userInfoJson);
                return userCacheData;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取当前用户信息时发生异常");
                return null;
            }
        }


        /// <summary>
        /// 从请求头中提取Token
        /// </summary>
        /// <returns>JWT令牌</returns>
        private string ExtractTokenFromHeader()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Request?.Headers == null)
                return null;

            if (!httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                return null;

            var tokenHeader = authHeader.ToString();
            if (string.IsNullOrEmpty(tokenHeader) || !tokenHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return null;

            return tokenHeader.Substring("Bearer ".Length).Trim();
        }

    }

    public class UserTokenData
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }





}