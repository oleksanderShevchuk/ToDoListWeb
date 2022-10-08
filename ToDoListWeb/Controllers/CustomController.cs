using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoListWeb.Models;

namespace ToDoListWeb.Controllers
{
    public class CustomController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CustomController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name};
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
                
            }
            return View(model);
        }

        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    //    User user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync
        //    //        (u => u.Email == model.Email && u.Password == model.Password);
        //    //    if (user != null)
        //    //    {
        //    //        await Authenticate(user); // Authentication

        //    //        return RedirectToAction("Index", "Home");
        //    //    }
        //    //    ModelState.AddModelError("", "Incorrect user name and/or password");
        //    //}
        //    //return View(model);
        //    return View();
        //}
        ////[HttpGet]
        ////public IActionResult Register()
        ////{
        ////    return View();
        ////}
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> Register(RegisterModel model)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        ////        if (user == null)
        ////        {
        ////            // add user to database
        ////            user = new User { Email = model.Email, Password = model.Password };
        ////            Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
        ////            if (userRole != null)
        ////                user.Role = userRole;

        ////            db.Users.Add(user);
        ////            await db.SaveChangesAsync();

        ////            await Authenticate(user); // authentication

        ////            return RedirectToAction("Index", "Home");
        ////        }
        ////        else
        ////            ModelState.AddModelError("", "Incorrect user name and/or password");
        ////    }
        ////    return View(model);
        ////}

        ////private async Task Authenticate(User user)
        ////{
        ////    // create one claim
        ////    var claims = new List<Claim>
        ////    {
        ////        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
        ////        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
        ////    };
        ////    // create object ClaimsIdentity
        ////    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
        ////        ClaimsIdentity.DefaultRoleClaimType);
        ////    // setting authentication cookies
        ////    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        ////}

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
