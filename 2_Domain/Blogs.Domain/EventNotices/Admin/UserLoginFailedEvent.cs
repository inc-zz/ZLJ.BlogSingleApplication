using Blogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.EventNotices.Admin
{
    /// <summary>
    /// 登录失败事件
    /// </summary>
    public class UserLoginFailedEvent : DomainEvent
    {
        public string UserName { get; }
        public string IpAddress { get; }
        public DateTime AttemptTime { get; }
        public string Reason { get; }
        public string DeviceInfo { get; }

        public UserLoginFailedEvent(string userName, string ipAddress, string reason, string deviceInfo = null)
        {
            UserName = userName;
            IpAddress = ipAddress;
            AttemptTime = DateTime.Now;
            Reason = reason;
            DeviceInfo = deviceInfo;
        }

    }
}
