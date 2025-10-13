using Blogs.AppServices.Requests.App;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.Blogs.Settings
{
    public class DeleteBlogSettingCommand : BlogsSettingCommand
    {
        public DeleteBlogSettingCommand(IdParam request)
        {
            this.Id = request.Id;
        }

        public override bool IsValid()
        {
            return this.Url != null && this.Content != null && this.Title != null;
        }
    }
}
