using Blogs.Domain.IRepositorys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Repositorys
{
    /// <summary>
    /// 实现领域层基础仓储接口
    /// </summary>
    public class BaseRepository<T> : SimpleClient<T>, IBaseRepository<T> where T : class, new()
    {
        public BaseRepository()
        {

        }
       
    }
}
