using MediatR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.EventBus
{

    /// <summary>
    /// 事件总线（中介者）
    /// </summary>
    public sealed class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private static readonly ConcurrentDictionary<Type, object> _requestHandlers = new ConcurrentDictionary<Type, object>();
       
        /**
         当命令执行完成后，通过发布事件来通知系统中的其他模块
         */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="serviceFactory"></param> 
        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        ///// <summary>
        ///// 发送执行命令
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //public Task SendCommand<T>(T command) where T : Command
        //{
        //    return _mediator.Send(command);
        //}

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <returns></returns>
        public Task RaiseEvent<T>(T command) where T : Event
        {
            return _mediator.Publish(command);
        }

    }
}
