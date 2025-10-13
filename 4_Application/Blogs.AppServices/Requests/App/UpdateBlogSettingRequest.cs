using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.App
{
    public class UpdateBlogSettingRequest: CreateBlogSettingRequest
    {
        public long Id { get; set; }

    }
}
