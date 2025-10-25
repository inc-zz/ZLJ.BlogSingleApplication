using Blogs.Domain;
using Blogs.Infrastructure.Services.App;
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
    public static class CurrentAppUser
    {
        private static IAppOpenIddictService _appOpeniddictService;
        private static readonly AsyncLocal<CacheUserModel> _appCurrentUser = new AsyncLocal<CacheUserModel>();

        /// <summary>
        /// 设置提供者（应在应用程序启动时调用）
        /// </summary>
        /// <param name="provider">用户上下文提供者</param>
        public static void SetProvider(IAppOpenIddictService provider)
        {
            _appOpeniddictService = provider;
        }

        /// <summary>
        /// 获取当前用户信息（便捷访问方式）
        /// </summary>
        public static CacheUserModel Instance
        {
            get
            {
                if (_appCurrentUser.Value != null)
                    return _appCurrentUser.Value;

                if (_appOpeniddictService == null)
                    throw new InvalidOperationException("OpeniddictService服务未注册");

                var task = Task.Run(async () => await _appOpeniddictService.GetCurrentUserAsync());
                _appCurrentUser.Value = task.Result;

                return _appCurrentUser.Value;
            }
        }

        /// <summary>
        /// 异步获取当前用户信息
        /// </summary>
        /// <returns>当前用户信息</returns>
        public static async Task<CacheUserModel> GetAsync()
        {
            if (_appOpeniddictService == null)
                throw new InvalidOperationException("CurrentUser提供程序尚未初始化。在启动中添加CurrentUser。配置服务.");

            var user = await _appOpeniddictService.GetCurrentUserAsync();
            _appCurrentUser.Value = user;
            return user;
        }

        /// <summary>
        /// 清除当前用户缓存（用于测试或特定场景）
        /// </summary>
        public static void Clear()
        {
            _appCurrentUser.Value = null;
        }
    }
}
