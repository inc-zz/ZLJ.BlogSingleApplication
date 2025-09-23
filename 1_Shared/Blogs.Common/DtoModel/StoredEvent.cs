using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel
{
    /// <summary>
    /// 事件持久化对象
    /// </summary>
    public class StoredEvent
    {
        /// <summary>
        /// 构造方式实例化
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="messageType"></param>
        /// <param name="data"></param>
        /// <param name="user"></param>
        public StoredEvent(long aggregateId,string messageType, string data, string user)
        {
            Id = Guid.NewGuid();
            AggregateId = aggregateId;
            MessageType = messageType;
            Data = data;
            User = user;
        }
        /// <summary>
        /// 事件存储Id
        /// </summary>
        public Guid Id { get; private set; }
        /// <summary>
        /// 存储的数据
        /// </summary>
        public string Data { get; private set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public string User { get; private set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public string MessageType { get; private set; }
        /// <summary>
        /// 聚合根Id
        /// </summary>
        public long AggregateId { get; private set; }

    }
}
