using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneDirectoryApi.Data;
using PhoneDirectoryApi.Models.Domain;
using PhoneDirectoryApi.Models.ViewModels;
using System.Security.Claims;

namespace PhoneDirectoryApi.Controllers
{
    
    
        [Authorize(Roles = "Admin")]
        public class UsersController : Controller
        {
        private readonly PhoneDirectoryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

            public UsersController(PhoneDirectoryContext context, UserManager<ApplicationUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public async Task<IActionResult> Index()
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var users = await _context.Users
                                          .Where(x => x.Id != currentUserId)
                                          .ToListAsync();

                var viewModels = new List<UserWithRolesViewModel>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (!roles.Any())
                    {
                        await _userManager.AddToRoleAsync(user, "Client");
                        roles = await _userManager.GetRolesAsync(user);
                    }

                    viewModels.Add(new UserWithRolesViewModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Address = user.Address,
                        Roles = roles.ToList(),
                        LockoutEnd = user.LockoutEnd
                    });
                }

                return View(viewModels);
            }

            public async Task<IActionResult> Lock(string id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                user.LockoutEnd = DateTime.Now.AddYears(100);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            public async Task<IActionResult> UnLock(string id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                user.LockoutEnd = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            [HttpGet]
            public async Task<IActionResult> ManageRoles(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound();

                var allRoles = await _context.Roles.Select(r => r.Name).ToListAsync();
                var userRoles = await _userManager.GetRolesAsync(user);

                var model = new ManageRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = allRoles.Select(role => new RoleSelection
                    {
                        RoleName = role,
                        IsSelected = userRoles.Contains(role)
                    }).ToList()
                };

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> ManageRoles(ManageRolesViewModel model)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return NotFound();

                var currentRoles = await _userManager.GetRolesAsync(user);
                var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName).ToList();

                var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to remove existing roles");
                    return View(model);
                }

                result = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to add selected roles");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
        }
    }
