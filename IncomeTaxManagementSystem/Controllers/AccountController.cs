using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncomeTaxManagementSystem.Data;
using IncomeTaxManagementSystem.Models;
using IncomeTaxManagementSystem.ViewModels;

namespace IncomeTaxManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ApplicationDbContext context,
            IPasswordHasher<User> passwordHasher,
            ILogger<AccountController> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "User");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var input = model.EmailOrPAN.Trim();

            // Find user by Email or PAN (case-insensitive)
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Email.ToLower() == input.ToLower() || u.PAN.ToUpper() == input.ToUpper());

            if (user == null)
            {
                // Generic error message to prevent user enumeration
                ModelState.AddModelError(string.Empty, "Invalid email/PAN or password.");
                return View(model);
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email/PAN or password.");
                return View(model);
            }

            // Check if user status is Active
            if (!string.Equals(user.Status, "Active", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "Your account is not active. Please contact the administrator.");
                return View(model);
            }

            // Create claims using standard ASP.NET ClaimTypes
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("PAN", user.PAN)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(14)
                    : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            _logger.LogInformation("User {UserId} logged in successfully with role {Role}.", user.Id, user.Role);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) 
                && returnUrl != "/" 
                && !returnUrl.Equals("/Home", StringComparison.OrdinalIgnoreCase) 
                && !returnUrl.Equals("/Home/Index", StringComparison.OrdinalIgnoreCase))
            {
                return Redirect(returnUrl);
            }

            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "User");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var normalizedPAN = model.PAN.Trim().ToUpper();
            var normalizedEmail = model.Email.Trim().ToLower();

            // Check if PAN already exists
            bool panExists = await _context.Users.AnyAsync(u => u.PAN == normalizedPAN);
            if (panExists)
            {
                ModelState.AddModelError("PAN", "An account with this PAN already exists.");
            }

            // Check if Email already exists
            bool emailExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create new User entity
            var user = new User
            {
                FullName = model.FullName.Trim(),
                PAN = normalizedPAN,
                DateOfBirth = model.DateOfBirth,
                Mobile = model.Mobile.Trim(),
                Email = normalizedEmail,
                Address = model.Address?.Trim(),
                Role = "User",
                Status = "Active",
                CreatedDate = DateTime.Now
            };

            // Securely hash password
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New user registered successfully with ID {UserId} and PAN {PAN}.", user.Id, user.PAN);

            TempData["SuccessMessage"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
