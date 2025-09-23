using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Blogs.AdminSite.Pages.Account
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }


        [Required(ErrorMessage = "用户名不能为空")]
        public string Username { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }
}
