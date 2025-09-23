using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Models
{
    /// <summary>
    /// 领域层分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DomainPageResult<T>
    {
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }
}
