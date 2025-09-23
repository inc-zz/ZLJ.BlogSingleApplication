//using Blogs.Infrastructure.JwtAuthorize;
//using Blogs.Infrastructure.Services;
//using Newtonsoft.Json;

//namespace Blogs.Infrastructure.Context
//{
//    /// <summary>
//    /// 用户上下文服务实现
//    /// </summary>
//    public class CurrentUserContext : ICurrentUserContext
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly ILogger<CurrentUserContext> _logger;
//        private readonly JwtConfig _jwtConfig;
//        private readonly IConnectionMultiplexer _redisConnection;
//        private readonly IOpenIddictService _openiddictService;

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="httpContextAccessor"></param>
//        /// <param name="redisConnection"></param>
//        /// <param name="jwtConfigOptions"></param>
//        /// <param name="logger"></param>
//        public CurrentUserContext(
//            IHttpContextAccessor httpContextAccessor,
//            IConnectionMultiplexer redisConnection,
//            IOptions<JwtConfig> jwtConfigOptions,
//            ILogger<CurrentUserContext> logger,
//            IOpenIddictService openiddictService)
//        {
//            _httpContextAccessor = httpContextAccessor ;
//            _redisConnection = redisConnection ;
//            _jwtConfig = jwtConfigOptions?.Value ;
//            _logger = logger;
//            _openiddictService = openiddictService;

//            // 注册静态访问器
//            CurrentUser.SetProvider(this);
//        }

//        /// <summary>
//        /// 获取当前用户信息
//        /// </summary>
//        /// <returns>当前用户信息</returns>
//        public async Task<CurrentUserDto> GetCurrentUserAsync()
//        {
//            try
//            {
//                var token = ExtractTokenFromHeader();
//                if (string.IsNullOrEmpty(token))
//                {
//                    _logger.LogWarning("未找到有效的认证令牌");
//                    return null;
//                }

//                // 从Redis获取用户信息
//                var userInfo = _openiddictService.JwtTokenParser(token);


//                if (userInfo == null)
//                {
//                    _logger.LogWarning("Redis中未找到用户信息，令牌可能已失效");
//                    return null;
//                }

//                return userInfo;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "获取当前用户信息时发生异常");
//                return null;
//            }
//        }

//        /// <summary>
//        /// 验证Token有效性
//        /// </summary>
//        /// <param name="token">JWT令牌</param>
//        /// <returns>验证结果</returns>
//        public async Task<bool> ValidateTokenAsync(string token)
//        {
//            if (string.IsNullOrEmpty(token))
//                return false;

//            try
//            {
              
//                var user = await GetCurrentUserAsync();
//                var userId = user.Id;
//                var redis = _redisConnection.GetDatabase();
//                var userKey = $"user_tokens:{userId}";
                 
//                var exists = await redis.KeyExistsAsync(userKey);

//                if (!exists)
//                {
//                    _logger.LogWarning("Token在Redis中不存在或已过期");
//                    return false;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "验证Token时发生异常");
//                return false;
//            }
//        }
 
//        /// <summary>
//        /// 从请求头中提取Token
//        /// </summary>
//        /// <returns>JWT令牌</returns>
//        private string ExtractTokenFromHeader()
//        {
//            var httpContext = _httpContextAccessor.HttpContext;
//            if (httpContext?.Request?.Headers == null)
//                return null;

//            if (!httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
//                return null;

//            var tokenHeader = authHeader.ToString();
//            if (string.IsNullOrEmpty(tokenHeader) || !tokenHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
//                return null;

//            return tokenHeader.Substring("Bearer ".Length).Trim();
//        }

       
//    }
//}