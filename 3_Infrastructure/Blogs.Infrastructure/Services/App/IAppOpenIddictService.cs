using Blogs.Common.Models;
using Blogs.Core.Entity.Blogs;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Services.App
{
    public interface IAppOpenIddictService
    { 
        /// <summary>
        /// 生成App登录Token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rereshToken"></param>
        /// <returns></returns>
        Task<TokenResult> GenerateTokenAsync(BlogsUser user, string rereshToken = "");
        /// <summary>
        /// 验证Token有效性
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ValidateJwtToken(string token);
        /// <summary>
        /// 撤销Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="refreshKey"></param>
        /// <returns></returns>
        Task<bool> RevokeJwtToken(string token, string refreshKey);
        /// <summary>
        /// 解析Token，返回
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserTokenModel JwtTokenParser(string token);
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns>当前用户信息</returns>
        Task<CacheUserModel> GetCurrentUserAsync();
    }
}
