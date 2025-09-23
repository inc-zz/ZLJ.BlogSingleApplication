using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.UniOfWork
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 开始一个事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns>是否提交成功</returns>
        bool Commit();

        /// <summary>
        /// 异步提交事务
        /// </summary>
        /// <returns>是否提交成功</returns>
        Task<bool> CommitAsync();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback(); 

        /// <summary>
        /// 检查是否有活跃的事务
        /// </summary>
        bool HasActiveTransaction { get; }
    }
}
