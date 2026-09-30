using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncomeTaxManagementSystem.Data;
using IncomeTaxManagementSystem.ViewModels;

namespace IncomeTaxManagementSystem.Controllers
{
    [Authorize(Roles = "User")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var user = await _context.Users
                    .Include(u => u.TaxDetails)
                    .Include(u => u.Documents)
                    .Include(u => u.Applications)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    var dashboardVm = new UserDashboardViewModel
                    {
                        User = user,
                        LatestTaxDetails = user.TaxDetails.OrderByDescending(t => t.CreatedDate).FirstOrDefault(),
                        TotalDocuments = user.Documents.Count,
                        LatestApplication = user.Applications.OrderByDescending(a => a.Id).FirstOrDefault()
                    };
                    return View(dashboardVm);
                }
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Profile()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    return View(user);
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> TaxInformation()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var taxDetails = await _context.TaxDetails
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToListAsync();

                return View(taxDetails);
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Documents()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var documents = await _context.Documents
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.UploadedDate)
                    .ToListAsync();

                return View(documents);
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Applications()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var applications = await _context.Applications
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.SubmittedDate)
                    .ToListAsync();

                return View(applications);
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
