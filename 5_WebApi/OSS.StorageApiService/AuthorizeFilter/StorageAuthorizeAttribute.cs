using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OSS.StorageApiService
{

    /// <summary>
    /// 自定义存储权限校验
    /// </summary>
    public class StorageAuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 你的自定义权限验证逻辑
            if (!IsAuthorized(context))
            {
                // 如果验证失败，则拒绝访问
                context.Result = new ForbidResult();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool IsAuthorized(AuthorizationFilterContext context)
        {

            var user = context.HttpContext.User;


            // 这里实现你的权限验证逻辑
            // 例如，检查用户是否具有特定的权限
            // 你可以使用Abp.Authorization.AuthorizationHelper进行权限检查
            return true; // 或者根据你的逻辑返回true或false
        }

    }
}
