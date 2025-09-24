using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.DtoModel.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminUserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
