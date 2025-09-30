using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Blogs.AdminSite.Pages.Permissions
{
    public class RoleAuth : PageModel
    {
        private readonly ILogger<RoleAuth> _logger;

        public RoleAuth(ILogger<RoleAuth> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}