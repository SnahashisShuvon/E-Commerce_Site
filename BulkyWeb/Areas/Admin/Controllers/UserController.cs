using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            string roleId = _context.UserRoles.FirstOrDefault(userRole => userRole.UserId == userId).RoleId;

            RoleManagmentVM RoleVM = new RoleManagmentVM()
            {
                ApplicationUser = _context.ApplicationUsers.Include(user => user.Company).FirstOrDefault(user => user.Id == userId),
                RoleList = _context.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _context.Companies.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            RoleVM.ApplicationUser.Role = _context.Roles.FirstOrDefault(role => role.Id == roleId).Name;
            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagmentVM roleManagmentVM)
        {
            string roleId = _context.UserRoles.FirstOrDefault(role => role.UserId == roleManagmentVM.ApplicationUser.Id).RoleId;
            string oldRole = _context.Roles.FirstOrDefault(role => role.Id == roleId).Name;

            if (roleManagmentVM.ApplicationUser.Role != oldRole)
            {
                var applicationUser = _context.ApplicationUsers.FirstOrDefault(user => user.Id == roleManagmentVM.ApplicationUser.Id);

                if (roleManagmentVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }

                _context.SaveChanges();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }

            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var applicationUsers = _context.ApplicationUsers.Include(user => user.Company).ToList();
            var userRoles = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();

            foreach(var user in applicationUsers)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }

            return Json(new { data = applicationUsers });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return Json(new { success = false, message = "Error while Locking / Unlocking" });
            }

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddMonths(1);
            }
            _context.SaveChanges();

            return Json(new { success = true, message = "Operation Sussessful!" });
        }

        #endregion


    }
}
