//using System;
//using Blogs.Core;
//using Blogs.Infrastructure.Context;
//using Mapster;
//using Microsoft.AspNetCore.Http;

//namespace Blogs.Infrastructure.JwtAuthorize
//{

//    /// <summary>
//    /// 用户上下文服务接口
//    /// </summary>
//    public interface ICurrentUserContext
//    {
//        /// <summary>
//        /// 获取当前用户信息
//        /// </summary>
//        /// <returns>当前用户信息</returns>
//        Task<CurrentUserDto> GetCurrentUserAsync();

//        /// <summary>
//        /// 验证Token有效性
//        /// </summary>
//        /// <param name="token">JWT令牌</param>
//        /// <returns>验证结果</returns>
//        Task<bool> ValidateTokenAsync(string token);
//    }
//}