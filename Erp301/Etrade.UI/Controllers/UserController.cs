using Etrade.Entities.Models.Identity;
using Etrade.Entities.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Etrade.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = await userManager.GetUsersInRoleAsync("admin");
            var users = new List<User>();
            foreach (var user in admins)
            {
                users = userManager.Users.Where(x => x.Id != user.Id).ToList();
            }
            return View(users);
        }
        public async Task<IActionResult> RoleAssign(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            var roles = roleManager.Roles.Where(x=> x.Name != "admin").ToList();
            var userRoles = await userManager.GetRolesAsync(user);
            var roleAssigns = new List<RoleAssignViewModel>();
            roles.ForEach(role => roleAssigns.Add(new RoleAssignViewModel()
            {
                Id = id,
                Name = role.Name,
                HasAssign = userRoles.Contains(role.Name)
            }));
            ViewBag.Username = user.UserName;
            return View(roleAssigns);
        }
        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> models,int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            foreach (var role in models)
            {
                if (role.HasAssign)
                    await userManager.AddToRoleAsync(user, role.Name);
                else
                    await userManager.RemoveFromRoleAsync(user, role.Name);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult>Delete(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            var result = await userManager.DeleteAsync(user);
            if(result.Succeeded)
                return RedirectToAction("Index");
            return NotFound("Kullanıcı Silinemedi!!!");

        }
    }
}
