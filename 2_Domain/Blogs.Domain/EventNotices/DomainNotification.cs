using System;
using MediatR;

namespace Blogs.Domain.EventNotices
{
    /// <summary>
    /// 领域通知模型， 
    /// </summary>
    public class DomainNotification : Event
    {
        /// <summary>
        /// 标识
        /// </summary>
        public Guid DomainNotificationId { get; private set; }
        /// <summary>
        /// 通知模块
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// 通知内容
        /// </summary>
        public string Value { get; private set; }
        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; private set; }
        /// <summary>
        /// 生成通知
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public DomainNotification(string key, string value)
        {
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
            Key = key;
            Value = value;
        }
    }
}