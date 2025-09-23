//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;

//namespace  Blogs.Core
//{

//    /// <summary>
//    /// 读取配置文件
//    /// </summary>
//    public class ConfigHelper
//    {
//        public static IConfiguration Instance { get; set; }
//        static ConfigHelper()
//        {
//            //ReloadOnChange = true 当appsettings.json被修改时重新加载            
//            Instance = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();
//        }
//        /// <summary>
//        /// 获得配置文件的对象值
//        /// </summary>
//        /// <param name="jsonPath"></param>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public static string GetJson(string key, string configPath)
//        {
//            IConfiguration config = new ConfigurationBuilder().AddJsonFile(configPath).Build(); //json文件地址
//            string s = config.GetSection(key).Value; //json某个对象
//            return s;
//        }

//        /// <summary>
//        /// 根据配置文件和Key获得对象
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key">节点Key</param>
//        /// <param name="configPath">文件名称</param>
//        /// <returns></returns>
//        public static T GetAppSettings<T>(string key, string configPath) where T : class, new()
//        {
//            var baseDir = AppContext.BaseDirectory;
//            var currentClassDir = baseDir;

//            IConfiguration config = new ConfigurationBuilder()
//                .SetBasePath(currentClassDir)
//                .Add(new JsonConfigurationSource
//                {
//                    Path = configPath,
//                    Optional = false,
//                    ReloadOnChange = true
//                })
//                .Build();

//            var s = config.GetSection(key);
//            var appconfig = new ServiceCollection().AddOptions()
//                .Configure<T>(s)
//                .BuildServiceProvider()
//                .GetService<IOptions<T>>()
//                .Value;
//            return appconfig;
//        }

//        /// <summary>
//        /// 获取自定义配置文件类
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="configKey">根节点</param>
//        /// <param name="configPath">配置文件名称</param>
//        /// <returns></returns>
//        public static T GetAppSettingsByFile<T>(string configKey, string configPath) where T : class, new()
//        {
//            if (string.IsNullOrWhiteSpace(configKey) || string.IsNullOrWhiteSpace(configPath))
//                return null;

//            IConfiguration config = new ConfigurationBuilder()
//                .Add(new JsonConfigurationSource { Path = configPath, ReloadOnChange = true }).Build();

//            var appconfig = new ServiceCollection()
//                .AddOptions()
//                .Configure<T>(config.GetSection(configKey))
//                .BuildServiceProvider()
//                .GetService<IOptions<T>>()
//                .Value;

//            return appconfig;
//        }

//        /// <summary>
//        /// 获取自定义配置文件类
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="configKey">根节点</param>
//        /// <param name="configPath">配置文件名称</param>
//        /// <returns></returns>
//        public static Task<T> GetAppSettingsByFileAsync<T>(string configKey, string configPath) where T : class, new()
//        {
//            if (string.IsNullOrWhiteSpace(configKey) || string.IsNullOrWhiteSpace(configPath))
//                return null;

//            IConfiguration config = new ConfigurationBuilder()
//                .Add(new JsonConfigurationSource { Path = configPath, ReloadOnChange = true }).Build();

//            var appconfig = new ServiceCollection()
//                .AddOptions()
//                .Configure<T>(config.GetSection(configKey))
//                .BuildServiceProvider()
//                .GetService<IOptions<T>>()
//                .Value;

//            return Task.Run(() => appconfig);
//        }

//        /// <summary>
//        /// 获取自定义配置文件配置
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="configKey"></param>
//        /// <param name="configPath"></param>
//        /// <returns></returns>
//        public static List<T> GetListAppSettings<T>(string configKey, string configPath) where T : class, new()
//        {
//            if (string.IsNullOrWhiteSpace(configKey) || string.IsNullOrWhiteSpace(configPath))
//                return null;

//            IConfiguration config = new ConfigurationBuilder()
//                .Add(new JsonConfigurationSource { Path = configPath, ReloadOnChange = true }).Build();

//            var appconfig = new ServiceCollection()
//               .AddOptions()
//               .Configure<List<T>>(config.GetSection(configKey))
//               .BuildServiceProvider()
//               .GetService<IOptions<List<T>>>()
//               .Value;

//            return appconfig;

//        }


//    }
//}
