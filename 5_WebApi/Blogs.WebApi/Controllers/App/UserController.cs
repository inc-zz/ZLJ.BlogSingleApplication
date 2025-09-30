using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Controllers.App
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
