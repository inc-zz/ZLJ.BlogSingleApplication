using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Models
{
    public class PagedRequest
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
        public string SearchTerm { get; set; }
    }
}
