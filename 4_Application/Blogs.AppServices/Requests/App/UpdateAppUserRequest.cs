using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateAppUserRequest: EditAppUserRequest
    {
        public long Id { get; set; }
    }
}
