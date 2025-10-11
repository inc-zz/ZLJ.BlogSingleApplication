using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Blogs.AdminSite.Pages.Permissions
{
    public class MenuModel : PageModel
    {
        private readonly ILogger<MenuModel> _logger;

        public MenuModel(ILogger<MenuModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}