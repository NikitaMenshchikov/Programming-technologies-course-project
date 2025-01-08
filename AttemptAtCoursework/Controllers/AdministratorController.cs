using AttemptAtCoursework.Data;
using AttemptAtCoursework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;

namespace AttemptAtCoursework.Controllers
{
     [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly ApplicationDbContext _context;


        public AdministratorController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<string>> roleManager,
 ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SearchUserForEmail(string? userEmail)
        {
            var users = _userManager.Users.ToList();
            if (userEmail == null)
                return PartialView("_partialUsersTable", users);
            users = _userManager.Users.Where(e => e.Email.Contains(userEmail)).ToList();

            var allRoles = new List<Role>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    allRoles.Add(new Role
                    {
                        UserId = user.Id,
                        RoleName = role,
                    });
                }
            }
            ViewBag.Roles = allRoles;
            return PartialView("_partialUsersTable", users);
        }

        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            var allRoles = new List<Role>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    allRoles.Add(new Role
                    {
                        UserId = user.Id,
                        RoleName = role,
                    });
                }

            }
            ViewBag.Roles = allRoles;

            return View(users);
        }

        public async Task<IActionResult> DeleteUser(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            return RedirectToAction("Users");
        }

    }
}
