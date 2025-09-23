//using Blogs.Infrastructure.JwtAuthorize;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Blogs.Infrastructure.Context
//{
//   /// <summary>
//   /// 服务扩展方法
//   /// </summary>
//    public static class ServiceCollectionExtensions
//    {
//        /// <summary>
//        /// 添加用户上下文服务
//        /// </summary>
//        /// <param name="services">服务集合</param>
//        /// <returns>服务集合</returns>
//        public static IServiceCollection AddCurrentUser(this IServiceCollection services)
//        {
//            services.AddScoped<ICurrentUserContext, CurrentUserContext>();
//            return services;
//        }
//    }
//}
