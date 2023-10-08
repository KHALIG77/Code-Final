using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
    public class StaffController : Controller
    {
        private readonly FurnutireContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roles;

        public StaffController(FurnutireContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roles)
        {
            _context = context;
            _userManager = userManager;
            _roles = roles;
        }
        public IActionResult Index()
        {
            List<AppUser> staffUsers = _context.AppUsers.Where(x => x.IsSuperAdmin == false && x.IsStaff == true).ToList();


            return View(staffUsers);
        }
    }
}
