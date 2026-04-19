using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using NgoDonationSystem.DTOs.Requests;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter both email and password.";
                return View();
            }

            var (user, roleName, errorMessage) = await _service.AuthenticateAsync(email, password);
            if (user == null)
            {
                ViewBag.Error = errorMessage ?? "Geçersiz e-posta veya şifre.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (!string.IsNullOrEmpty(roleName))
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserRequest model)
        {
            if (ModelState.IsValid)
            {
                var success = await _service.RegisterAsync(model);
                if (!success)
                {
                    ViewBag.Error = "Bu e-posta adresi zaten kayıtlı.";
                    return View(model);
                }
                TempData["SuccessMessage"] = "Kayıt talebiniz başarıyla alındı. Yönetici onayının ardından giriş yapabilirsiniz.";
                return RedirectToAction(nameof(Login));
            }
            return View(model);
        }
    }
}
