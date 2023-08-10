using Etrade.Entities.Models.Identity;
using Etrade.Entities.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Etrade.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Register()
        {
            if(User.Identity.Name != null)
             return  RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            var user = new User()
            {
                Name = register.Name,
                Surname = register.Surname,
                Email = register.Email,
                UserName=register.Username
            };
          var resault = await userManager.CreateAsync(user,register.Password);
            await roleManager.CreateAsync(new Role("user"));
            await userManager.AddToRoleAsync(user, "user");
            if (resault.Succeeded)
              return  RedirectToAction("Login");
            return View(register);
        }
        public async Task<IActionResult> Login()
        {
            if (User.Identity.Name != null)
             return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var user = new User(); 
            if (login.UsernameOrEmail.Contains("@"))
            {
                 user = await userManager.FindByEmailAsync(login.UsernameOrEmail);
            }
            else
            {
                user = await userManager.FindByNameAsync(login.UsernameOrEmail);
            }
            if(user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
                if(result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }
            return View(login);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
