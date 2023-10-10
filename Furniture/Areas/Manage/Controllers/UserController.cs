using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UserController : Controller
    {
        private readonly FurnutireContext _context;

        public UserController(FurnutireContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1,string search = null)
        {
            var query = _context.AppUsers.Where(x => x.IsStaff == false || x.IsStaff==null).AsQueryable();
            if (search!=null)
            {
                query=query.Where(x=>x.FullName.Contains(search));  
            }

            ViewBag.Search = search;

            return View(PaginatedList<AppUser>.Create(query,page,4));
        }
    }
}
