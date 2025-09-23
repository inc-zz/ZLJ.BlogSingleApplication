using System;
namespace Blogs.AppServices.Requests.Admin
{
    public class SearchLogRequest
    {
        public SearchLogRequest()
        {
        }

        /// <summary>
        /// 级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

    }
}
