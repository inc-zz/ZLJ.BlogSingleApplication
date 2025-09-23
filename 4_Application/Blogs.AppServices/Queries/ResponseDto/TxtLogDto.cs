using System;
namespace Blogs.Core.DtoModel.ResponseDto
{
    public class TxtLogDto
    {
        public TxtLogDto()
        {
        }

        /// <summary>
        /// 报错时间
        /// </summary>
        public string logTime { get; set; }
        /// <summary>
        /// 报错级别
        /// </summary>
        public string logLevel { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception { get; set; }
    }
}
