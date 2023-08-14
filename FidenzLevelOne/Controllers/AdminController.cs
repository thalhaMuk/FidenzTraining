using Microsoft.AspNetCore.Mvc;
using FidenzLevelOne.Data;
using Microsoft.AspNetCore.Authorization;

namespace FidenzLevelOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Admin")]
        public IActionResult Admin()
        {
            var customers = _context.customers.ToList();
            return View(customers);
        }

    }
}
