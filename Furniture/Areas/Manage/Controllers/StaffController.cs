//using AspNetCore;
using Furniture.Areas.Manage.ViewModels.Admin;
using Furniture.Areas.Manage.ViewModels.Staff;
using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
        public IActionResult Create()
        {
          StaffCreateViewModel vm =new StaffCreateViewModel
          {
              Roles=_context.Roles.Where(x=>x.Name!="SuperAdmin" && x.Name!="Member").ToList()
          };
            return View(vm);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Create(StaffCreateViewModel admin)
        {
            if (!ModelState.IsValid)
            {
                return View(admin);
            }
            if (admin.Password == null)
            {
                ModelState.AddModelError("Password", "Password is required");
                return View(admin);
            }
            AppUser newStaff = new AppUser
            {
                IsStaff = true,
                FullName = admin.FullName,
                UserName = admin.UserName,
                Email = admin.Email,
                PhoneNumber = admin.Phone,
                EmailConfirmed = true,

            };

            var result = await _userManager.CreateAsync(newStaff, admin.Password);
            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View(admin);
            }
            await _userManager.AddToRoleAsync(newStaff, (admin.Role == "Admin" ? "Admin" : "Staff"));

            return RedirectToAction("index");
        }
        
        public async Task<IActionResult> Delete(string Id)
        {
            var admin=await _userManager.FindByIdAsync(Id);
            if (admin == null) return BadRequest();

            var result = _userManager.DeleteAsync(admin).Result;
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }
        public IActionResult Edit(string Id)
        {
            AppUser admin =_userManager.FindByIdAsync(Id).Result;
            if (admin==null)
            {
                return View("Error");
            }

          

            StaffCreateViewModel adminVM=new StaffCreateViewModel()
            {
                Email = admin.Email,
                FullName=admin.FullName,
                Phone=admin.PhoneNumber,
                UserName=admin.UserName,
                Role=GetRole(Id),
                Roles = _context.Roles.Where(x => x.Name != "SuperAdmin" && x.Name != "Member").ToList()


            };
            ViewBag.Id = Id;

            return View(adminVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public  IActionResult Edit(StaffCreateViewModel adminVM)
        {
            ViewBag.Id=adminVM.Id;
            AppUser admin = _context.AppUsers.FirstOrDefault(x => x.Id == adminVM.Id);
            if (admin == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View(adminVM);
            }
            if (admin.UserName!=adminVM.UserName && _context.AppUsers.Any(x=>x.UserName==adminVM.UserName))
            {
                ModelState.AddModelError("UserName", "Username already used");
                return View(adminVM);
            }
            if (adminVM.NewPassword!=null && adminVM.Password !=null)
            {
                var result = _userManager.ChangePasswordAsync(admin, adminVM.Password, adminVM.NewPassword).Result;
                if(!result.Succeeded) 
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", "Current password is incorrect");
                    }
                    return View(adminVM);
                }

            }
            if (adminVM.Role == null)
            {
                ModelState.AddModelError("Role", "Please choose Role");
                return View(adminVM);
            }
            


            admin.Email = adminVM.Email;
            admin.UserName = adminVM.UserName;
            admin.PhoneNumber = adminVM.Phone;
            admin.FullName = adminVM.FullName;

            var updateResult = _userManager.UpdateAsync(admin).Result;

            if (adminVM.Role!=GetRole(adminVM.Id)) 
            { 
                var role = _context.UserRoles.FirstOrDefault(x=>x.UserId==adminVM.Id);
                role.RoleId = RoleId(adminVM.Id);
                _context.SaveChanges();


            }


            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(adminVM);
            }


            return RedirectToAction("index");


        }
        private string GetRole(string Id)
        {
            var roleId = _context.UserRoles.FirstOrDefault(x => x.UserId == Id).RoleId;
            var role = _context.Roles.FirstOrDefault(x => x.Id == roleId).Name;
            return role;

        }
        private string RoleId(string  Id)
        {
            var roleId = _context.UserRoles.FirstOrDefault(x => x.UserId == Id).RoleId;
            return roleId;
        }
    }
}
