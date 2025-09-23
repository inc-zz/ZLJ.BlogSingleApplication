using System;
using Blogs.Domain.EventBus;
using FluentValidation.Results;
using MediatR;

namespace Blogs.Domain
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class Command : Message
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; private set; }
        /// <summary>
        /// 验证结果
        /// </summary>
        public ValidationResult ValidationResult { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected Command()
        {
            Timestamp = DateTime.Now;
        }
        /// <summary>
        /// 数据校验是否通过
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

    }
}
