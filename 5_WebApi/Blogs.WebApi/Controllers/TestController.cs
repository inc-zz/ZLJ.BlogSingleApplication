using Blogs.Domain;
using Blogs.Infrastructure;
using Blogs.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCD.Common;
using SQLitePCL;

namespace Blogs.WebApi.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GenerateId")]
        [AllowAnonymous]
        public string GenerateId()
        {
            IdWorkerUtils idWorker = new IdWorkerUtils(1, 1);
            long id = idWorker.NextId();
            return id.ToString();
        }

        /// <summary>
        /// Put Logger
        /// </summary>
        /// <param name="message"></param>
        [HttpPost]
        public void WriteLoggerText(string message)
        {
            var id = CurrentUser.Instance.UserId;
            var userName = CurrentUser.Instance.UserInfo.UserName;
            _logger.LogDebug($"=======userId={id}============userName={userName}==========");
            _logger.LogInformation(message);
            _logger.LogDebug(message);
            _logger.LogError(message);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("userInfo")]
        public CacheUserModel GetCurrentUser()
        {
            var id = CurrentUser.Instance.UserId;
            var userInfo = CurrentUser.Instance;
            return userInfo;
        }


    }
}
