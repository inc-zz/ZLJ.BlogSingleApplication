using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blogs.Domain.Notices
{
    /// <summary>
    /// 处理领域通知
    /// </summary>
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        
        private List<DomainNotification> _notifications;

        /// <summary>
        /// 
        /// </summary>
        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        /// <summary>
        /// 处理通知，新增到内存
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            _notifications.Add(message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 获取通知
        /// </summary>
        /// <returns></returns>
        public virtual List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        /// <summary>
        /// 检查总线对象周期是否存在通知
        /// </summary>
        /// <returns></returns>
        public virtual bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        /// <summary>
        /// 手动回收（清空通知）
        /// </summary>
        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }
    }
}
