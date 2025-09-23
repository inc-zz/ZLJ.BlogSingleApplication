using Blogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.EventNotices.Admin
{
    /// <summary>
    /// 用户登录成功事件
    /// </summary>
    public class UserLoginSuccessEvent : DomainEvent
    {
        public long UserId { get; }
        public string UserName { get; }
        public string IpAddress { get; }
        public DateTime LoginTime { get; }
        public string DeviceInfo { get; }

        public UserLoginSuccessEvent(long userId, string userName, string ipAddress, string deviceInfo = null)
        {
            UserId = userId;
            UserName = userName;
            IpAddress = ipAddress;
            LoginTime = DateTime.Now;
            DeviceInfo = deviceInfo;
        }

    }
}
