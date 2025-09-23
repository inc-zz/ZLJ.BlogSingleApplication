using Blogs.Domain.AggregateRoot;
using Blogs.Domain.IRepositorys.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.IRepositorys
{
    /// <summary>
    /// 领域仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> : ISimpleClient<T> where T : class, new()
    {
    }
}
