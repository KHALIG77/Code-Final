using Furniture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
          _userManager = userManager;
           _signInManager = signInManager;
           _roleManager = roleManager;
        }

        public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser
            {
                UserName = "xaliq",
                IsAdmin = true,
            };

            var result = await _userManager.CreateAsync(admin, "xaliq123");

            await _userManager.AddToRoleAsync(admin, "Admin");
            return Json(result);

        }
        public async Task<IActionResult> AddRole()
        {
            AppUser admin = _userManager.Users.FirstOrDefault();
           var result =   await  _userManager.AddToRoleAsync(admin, "Admin");
            return Content(result.ToString());
        }

        public async Task<ActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Member"));

            return Content("Correct");
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
