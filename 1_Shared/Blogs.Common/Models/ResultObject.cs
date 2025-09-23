using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.Models
{

    /// <summary>
    /// 基础返回类
    /// </summary>
    public class ResultObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public ResultObject(string message = "", int? code = null)
        {
            this.code = code == null ? 404 : code.Value;
            this.message = !string.IsNullOrWhiteSpace(message) ? message : "Not Found";
        }

        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool success { get; set; } = true;
        /// <summary>
        /// 信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 处理成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess() => code == 200;

    }

    /// <summary>
    /// API 返回JSON字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultObject<T> : ResultObject where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public ResultObject(string message = "", int? code = null)
        {
            this.code = code == null ? 404 : code.Value;
            this.message = !string.IsNullOrWhiteSpace(message) ? message : "Not Found";
        }

        /// <summary>
        /// 数据集
        /// </summary>
        public T data { get; set; }
    }
}
