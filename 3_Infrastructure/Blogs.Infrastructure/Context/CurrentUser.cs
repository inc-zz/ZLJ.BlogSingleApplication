using Blogs.Domain;
using Blogs.Infrastructure.JwtAuthorize;
using Blogs.Infrastructure.Services.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Context
{
    /// <summary>
    /// 静态用户上下文访问器
    /// </summary>
    public static class CurrentUser
    {
        private static IOpenIddictService _openiddictService;
        private static readonly AsyncLocal<CacheUserModel> _currentUser = new AsyncLocal<CacheUserModel>();

        /// <summary>
        /// 设置提供者（应在应用程序启动时调用）
        /// </summary>
        /// <param name="provider">用户上下文提供者</param>
        public static void SetProvider(IOpenIddictService provider)
        {
            _openiddictService = provider;
        }

        /// <summary>
        /// 获取当前用户信息（便捷访问方式）
        /// </summary>
        public static CacheUserModel Instance
        {
            get
            {
                if (_currentUser.Value != null)
                    return _currentUser.Value;

                if (_openiddictService == null)
                    throw new InvalidOperationException("OpeniddictService服务未注册");

                // 异步方法同步获取（注意：这可能在某些场景下导致死锁）
                // 更好的做法是使用异步方法，但静态属性不能是异步的
                var task = Task.Run(async () => await _openiddictService.GetCurrentUserAsync());
                _currentUser.Value = task.Result;

                return _currentUser.Value;
            }
        }

        /// <summary>
        /// 异步获取当前用户信息
        /// </summary>
        /// <returns>当前用户信息</returns>
        public static async Task<CacheUserModel> GetAsync()
        {
            if (_openiddictService == null)
                throw new InvalidOperationException("CurrentUser提供程序尚未初始化。在启动中添加CurrentUser。配置服务.");

            var user = await _openiddictService.GetCurrentUserAsync();
            _currentUser.Value = user;
            return user;
        }

        /// <summary>
        /// 清除当前用户缓存（用于测试或特定场景）
        /// </summary>
        public static void Clear()
        {
            _currentUser.Value = null;
        }
    }
}
