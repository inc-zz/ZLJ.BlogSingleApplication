using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blogs.Domain.EventBus
{

    /// <summary>
    /// 事件总线（中介者）
    /// </summary>
    public interface IMediatorHandler
    {
         
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T"> 泛型 继承 Event：INotification</typeparam>
        /// <param name="event"> 事件模型</param>
        /// <returns></returns>
        Task RaiseEvent<T>(T @event) where T : Event;

    }
}
