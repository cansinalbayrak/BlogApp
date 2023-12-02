using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Roles Actions

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Role()
        {
            List<AppRole> roles = await _roleManager.Roles.ToListAsync();
            List<RoleViewModel> rolesViewModelList = new List<RoleViewModel>();

            foreach (var role in roles)
            {
                RoleViewModel roleViewModel = new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                };

                rolesViewModelList.Add(roleViewModel);
            }

            return View(rolesViewModelList);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole()
        {
            RoleViewModel roleViewModel = new RoleViewModel();

            return View(roleViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                AppRole roles = new AppRole()
                {
                    Name = roleViewModel.Name,
                    Description = roleViewModel.Description
                };

                IdentityResult result = await _roleManager.CreateAsync(roles);

                if (result.Succeeded)
                {
                    return RedirectToAction("Role");
                }
                else
                {
                    result.Errors.ToList().ForEach(error => { ModelState.AddModelError(string.Empty, error.Description); });
                }
            }


            return View(roleViewModel);
        }

        #endregion

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserRole(string Id)
        {
            string userId = Id;

            AppUser user = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();

            UserViewModel userViewModel = new UserViewModel()
            {
                Id = userId,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = await _roleManager.Roles.Select(s => new AssignRoleViewModel
            {

                RoleId = s.Id,
                RoleName = s.Name,
                IsAssigned = userRoles.Any(u => u == s.Name)

            }).ToListAsync();



            return View(new UserRolesViewModel { Id = user.Id, Roles = roles, User = userViewModel });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAssignRoles(UserRolesAddViewModel userRolesViewModel)
        {
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByEmailAsync(userRolesViewModel.User.Email);
                var user = await _userManager.Users.Where(u => u.Id == userRolesViewModel.Id).FirstOrDefaultAsync();

                foreach (var role in userRolesViewModel.Roles)
                {
                    if (role.IsAssigned)
                    {
                        await _userManager.AddToRoleAsync(user, role.RoleName);
                    }
                    else
                    {
                        if (role.RoleName != "Admin")
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                        }
                    }
                }

                return RedirectToAction("User");
            }

            return View(userRolesViewModel);
        }

    }
}

