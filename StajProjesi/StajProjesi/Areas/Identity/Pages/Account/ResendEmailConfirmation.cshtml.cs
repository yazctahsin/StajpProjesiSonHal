using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StajProjesi.Models;

namespace StajProjesi.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender? _emailSender;

        public ResendEmailConfirmationModel(UserManager<AppUser> userManager, IEmailSender? emailSender = null)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "E-posta zorunludur.")]
            [EmailAddress(ErrorMessage = "Ge�erli bir e-posta giriniz.")]
            [Display(Name = "E-posta")]
            public string Email { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);

            // Kullan�c� var/yok demeden ayn� mesaj (enumeration �nleme)
            if (user != null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId, code },
                    protocol: Request.Scheme);

                if (_emailSender != null)
                {
                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "E-posta Do�rulama",
                        $"Hesab�n�z� do�rulamak i�in l�tfen <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>buraya t�klay�n</a>."
                    );
                }
            }

            TempData["StatusMessage"] = "Do�rulama e-postas� g�nderildi. L�tfen e-postan�z� kontrol edin.";
            return RedirectToPage("./Login");
        }
    }
}