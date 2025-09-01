using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StajProjesi.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        public string? ReturnUrl { get; set; }

        public void OnGet(string? email = null, string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            // email parametresi istenirse ViewData/Model ile ekranda gösterilebilir.
        }
    }
}