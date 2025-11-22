using Blogs.AppServices.Requests.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Commands.WebSite
{
    /// <summary>
    /// 创建建议命令
    /// </summary>
    public class CreateSuggestCommand: WebSiteCommand
    {

        public CreateSuggestCommand(AppSuggestRequest request)
        {
            this.UserName = request.UserName;
            this.Contact = request.Contact;
            this.Content = request.Content;
            
        }

        public override bool IsValid()
        {
            return true;
        }

    }
}
