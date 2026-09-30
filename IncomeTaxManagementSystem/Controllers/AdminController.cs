using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncomeTaxManagementSystem.Data;

namespace IncomeTaxManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserCount = await _context.Users.CountAsync();
            ViewBag.ApplicationCount = await _context.Applications.CountAsync();
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.OrderByDescending(u => u.CreatedDate).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Applications()
        {
            var applications = await _context.Applications.Include(a => a.User).OrderByDescending(a => a.SubmittedDate).ToListAsync();
            return View(applications);
        }
    }
}
