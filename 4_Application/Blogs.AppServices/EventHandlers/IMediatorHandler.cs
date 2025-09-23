//using Blogs.Domain.EventBus;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Blogs.Domain
//{

//    /// <summary>
//    /// 事件总线（中介者）
//    /// </summary>
//    public interface IMediatorHandler
//    {

//        /// <summary>
//        /// 发送事件
//        /// </summary>
//        /// <typeparam name="T"> 泛型 </typeparam>
//        /// <param name="command"> 命令模型</param>
//        /// <returns></returns>
//        Task SendCommand<T>(T command) where T : Command;
//        /// <summary>
//        /// 发布事件
//        /// </summary>
//        /// <typeparam name="T"> 泛型 继承 Event：INotification</typeparam>
//        /// <param name="event"> 事件模型</param>
//        /// <returns></returns>
//        Task RaiseEvent<T>(T @event) where T : Event;

//    }
//}
