using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; } = new Credential();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            //Verify the credential
            if (Credential.UserName == "admin" && Credential.Password == "1234")
            {
                //Creating a security context
                List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, "admin"),
                                                       new Claim(ClaimTypes.Email, "admin@gmail.com"),
                                                       new Claim("Department", "HR"),
                                                       new Claim("Admin", "true"),
                                                       new Claim("Manager", "true"),
                                                       new Claim("EmploymentDate", "2023-05-01")
                };


                //Add claims to a identity
                ClaimsIdentity identity = new ClaimsIdentity(claims, "ReggyCookieAuth");

                //Claims Principal which makes up the security context
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };
                //Encript and serialize the security context so that it can go into a cookie
                await HttpContext.SignInAsync("ReggyCookieAuth", claimsPrincipal, authProperties);
                return RedirectToPage("/Index");
            }
            return Page();

        }
    }

    public class Credential
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
