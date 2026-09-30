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
            ViewBag.DocumentCount = await _context.Documents.CountAsync();
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

        public async Task<IActionResult> Documents()
        {
            var documents = await _context.Documents.Include(d => d.User).OrderByDescending(d => d.UploadedDate).ToListAsync();
            return View(documents);
        }

        public async Task<IActionResult> Reports()
        {
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalApplications = await _context.Applications.CountAsync();
            ViewBag.TotalDocuments = await _context.Documents.CountAsync();
            ViewBag.TotalTaxRecords = await _context.TaxDetails.CountAsync();
            return View();
        }
    }
}
