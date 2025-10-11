using System;
namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    public class SysLogDto
    {
        public SysLogDto()
        {
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 执行动作
        /// </summary>
        public string Operating { get; set; }
        /// <summary>
        /// 产生时间
        /// </summary>
        public DateTime OperatingTime { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 请求Action
        /// </summary>
        public string Action { get; set; }
        ///<summary>
        ///  请求地址
        ///</summary>
        public string RequestUrl { set; get; }
        ///<summary>
        ///  请求参数
        ///</summary>
        public string RequestParam { set; get; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatingUserName { get; set; }
        /// <summary>
        /// 请求IP
        /// </summary>
        public string IP { get; set; }


    }
}
