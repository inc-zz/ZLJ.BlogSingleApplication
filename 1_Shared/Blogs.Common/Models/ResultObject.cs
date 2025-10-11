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
        /// 请求是否成功
        /// </summary>
        public bool success { get; set; } = true;
        /// <summary>
        /// 信息
        /// </summary>
        public string? message { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 处理成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess() => code == 200;

        /// <summary>
        /// 失败返回格式
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultObject Error(string message) => new ResultObject { success = false, code = 400, message = message };
        /// <summary>
        /// 成功返回格式
        /// </summary>
        /// <param name="message">自定义Message</param>
        /// <returns></returns>
        public static ResultObject Success(string? message)
        {
            return new ResultObject
            {
                code = 200,
                message = message ?? "处理成功",
                success = true
            };
        }
    }

    /// <summary>
    /// API 返回JSON字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultObject<T> : ResultObject where T : class
    {
        /// <summary>
        /// 数据集
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 成功返回格式
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public static ResultObject<T> Success(T data, string? message)
        {
            return new ResultObject<T>
            {
                code = 200,
                message = string.IsNullOrWhiteSpace(message) ? "处理成功" : message,
                data = data,
                success = true
            };
        }
    }
}
