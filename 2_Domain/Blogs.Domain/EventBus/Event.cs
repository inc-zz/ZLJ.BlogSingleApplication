using System;
using MediatR;

namespace Blogs.Domain.EventBus
{
    /// <summary>
    /// 事件模型
    /// </summary>
    public abstract class Event : Message, INotification
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// 事件状态
        /// </summary>
        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
