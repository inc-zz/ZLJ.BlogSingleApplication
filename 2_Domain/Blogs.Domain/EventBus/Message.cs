using System;
using MediatR;

namespace Blogs.Domain.EventBus
{
    /// <summary>
    /// 事件模型
    /// </summary>
    public abstract class Message : IRequest<bool>
    {
        /// <summary>
        /// MessageType
        /// </summary>
        public string MessageType { get; protected set; }
        /// <summary>
        /// AggregateId
        /// </summary>
        public long AggregateId { get; protected set; }
        /// <summary>
        /// 通知
        /// </summary>
        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}