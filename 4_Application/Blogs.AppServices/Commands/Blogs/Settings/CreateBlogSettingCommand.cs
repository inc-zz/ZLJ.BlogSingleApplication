using Blogs.AppServices.Requests.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Settings
{
    public class CreateBlogSettingCommand:BlogsSettingCommand
    {
        public CreateBlogSettingCommand(CreateBlogSettingRequest request)
        {
            this.Title = request.Title;
            this.Content = request.Content;
            this.Url = request.Url;
            this.Tags = request.Tags;
            this.BusType = request.BusType;
        }

        public override bool IsValid()
        {
           return this.Url != null && this.Content != null && this.Title !=null;
        }
    }
}
