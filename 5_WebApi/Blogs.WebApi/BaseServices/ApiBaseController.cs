using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.BaseServices
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ApiBaseController<IServiceBase> : ControllerBase
    {
        protected IServiceBase _serviceBase;
        public ApiBaseController(IServiceBase serviceBase)
        {
            _serviceBase = serviceBase;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost, Route("Upload")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult Upload(IEnumerable<IFormFile> fileInput)
        {
            return Ok(InvokeService("Upload", new object[] { fileInput }));
        }

        /// <summary>
        /// 动态调用服务方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object InvokeService(string methodName, object[] parameters)
        {
            return _serviceBase.GetType().GetMethod(methodName).Invoke(_serviceBase, parameters);
        }

    }
}
