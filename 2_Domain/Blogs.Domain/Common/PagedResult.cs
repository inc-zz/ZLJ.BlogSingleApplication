using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Models
{
    public class PagedResult<T> : ResultObject where T : class, new()
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<T> Items { get; set; }

        public PagedResult()
        {
            Items = new List<T>();
        }

        public PagedResult(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            this.code = 200;
            this.success = true;
            this.message = "处理成功";
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = totalCount;
            Items = items;
        }
    }
}
