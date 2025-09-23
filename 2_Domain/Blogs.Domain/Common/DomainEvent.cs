using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid EventId { get; }
        public DateTime OccurredOn { get; }
        public long AggregateId { get; }
        public int AggregateVersion { get; }

        protected DomainEvent()
        {
            
        }

        protected DomainEvent(long aggregateId)
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            AggregateId = aggregateId;
            AggregateVersion = 1; // 默认版本
        }

        protected DomainEvent(long aggregateId, int aggregateVersion)
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            AggregateId = aggregateId;
            AggregateVersion = aggregateVersion;
        }
    }
}
