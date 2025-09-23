using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Common
{
    public interface IDomainEvent
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// 事件发生时间
        /// </summary>
        DateTime OccurredOn { get; }

        /// <summary>
        /// 聚合ID
        /// </summary>
        long AggregateId { get; }

        /// <summary>
        /// 聚合版本
        /// </summary>
        int AggregateVersion { get; }
    }
}
