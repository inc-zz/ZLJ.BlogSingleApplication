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


        [Required(ErrorMessage = "�û�������Ϊ��")]
        public string Username { get; set; }

        [Required(ErrorMessage = "���벻��Ϊ��")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "��ס��")]
        public bool RememberMe { get; set; }
    }
}
