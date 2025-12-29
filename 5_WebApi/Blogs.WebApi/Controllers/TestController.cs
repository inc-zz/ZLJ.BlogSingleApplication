using Blogs.Common;
using Blogs.Domain;
using Blogs.Domain.IRepositorys.Blogs;
using Blogs.Infrastructure;
using Blogs.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCD.Common;
using Newtonsoft.Json;
using SQLitePCL;

namespace Blogs.WebApi.Controllers
{
    /// <summary>
    /// 测试控制器 1
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("api/admin/[controller]")]
    public class TestController : ControllerBase
    {
        private ILogger<TestController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAppUserRepository _appUserRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="appUserRepository"></param>
        /// <param name="configuration"></param>
        public TestController(ILogger<TestController> logger, IAppUserRepository appUserRepository, IConfiguration configuration)
        {
            _logger = logger;
            _appUserRepository = appUserRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GenerateId")]
        [AllowAnonymous]
        public string GenerateId()
        {
            IdWorkerUtils idWorker = new IdWorkerUtils();
            long id = idWorker.NextId();
            return id.ToString();
        }

        /// <summary>
        /// 图片加水印测试
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        [HttpPost("imageWatermarkTest")]
        public async Task ImageWatermarkTest(string inputPath, string outputPath)
        {
            var watermarkService = new ImageWatermarkHelper();

            using var inputStream = System.IO.File.OpenRead(inputPath);
            using var outputStream = System.IO.File.Create(outputPath);
            await watermarkService.AddRandomWatermarksAsync(
               inputStream,
               outputStream,
               "水印测试",4,70,200,-45);
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

        /// <summary>
        /// 读取配置项目
        /// </summary>
        /// <returns></returns>
        [HttpGet("user1")]
        public string GetUser()
        {
            var conn = _configuration.GetSection("CorsConfig").Value;
            return conn;

        }
        /// <summary>
        /// 运行环境
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        [HttpGet("env")]
        public string GetEnvironment([FromServices] IWebHostEnvironment env)
        {
            return $"Current Environment: {env.EnvironmentName}";
        }


    }
}
