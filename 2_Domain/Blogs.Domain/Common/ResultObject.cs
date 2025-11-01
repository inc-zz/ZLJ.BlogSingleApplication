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
        /// 线程id
        /// </summary>
        public string? traceId { get; set; }
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
        public static ResultObject Error(string message, int code = 400)
        {
            return new ResultObject
            {
                success = false,
                code = code,
                message = message
            };
        }
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

        /// <summary>
        /// 从领域通知创建错误响应
        /// </summary>
        /// <param name="notifications"></param>
        /// <returns></returns>
        public static ResultObject FromDomainNotifications(IEnumerable<DomainNotification> notifications)
        {
            var notificationList = notifications?.ToList() ?? new List<DomainNotification>();

            if (!notificationList.Any())
            {
                return Error("未知错误");
            }

            // 获取第一个通知作为主要消息
            var firstNotification = notificationList.First();
            var errorMessage = firstNotification.Value;

            // 如果有多个通知，将其他通知作为数据返回
            var additionalErrors = notificationList.Skip(1).Select(n => n.Value).ToList();

            return new ResultObject
            {
                code = 400, // 业务错误码
                success = false,
                message = errorMessage
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
        public static ResultObject<T> Success(T data, string message = "")
        {
            return new ResultObject<T>
            {
                code = 200,
                message = string.IsNullOrWhiteSpace(message) ? "处理成功" : message,
                data = data,
                success = true
            };
        }

        public static ResultObject<T> Error(T data, string? message)
        {
            return new ResultObject<T>
            {
                code = 400,
                message = string.IsNullOrWhiteSpace(message) ? "处理失败" : message,
                success = true
            };
        }


    }
}
