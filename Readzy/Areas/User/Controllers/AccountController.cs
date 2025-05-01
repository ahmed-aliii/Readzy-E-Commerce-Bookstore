using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Readzy.Models.Entities;
using Readzy.Utility;
using Readzy.ViewModels;

namespace Readzy.Areas.User.Controllers
{
    [Area("User")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController
            (
            UserManager<ApplicationUser> userMannger , 
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            this.userManager = userMannger;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        //-------- REGISTER ---------------------------------------------------------------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if(ModelState.IsValid)
            {
                //Create a new user object
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    StreetAddress = registerVM.StreetAddress,
                    City = registerVM.City,
                    PostalCode = registerVM.PostalCode,
                    State = registerVM.State,
                };
                //save User to DB with hash password
                IdentityResult result = await userManager.CreateAsync(applicationUser, registerVM.Password);

                //User Creation successed
                if (result.Succeeded)
                {

                    //Add user to role
                    await userManager.AddToRoleAsync(applicationUser, registerVM.Role);

                    //Sign in the user => Add cookie
                    await signInManager.SignInAsync(applicationUser, isPersistent: false);

                    // For example, save user to the database
                    TempData["success"] = "Registration successful!";
                    return RedirectToAction("Index", "Home", new { area = "Customer" });

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            //ModelState is not valid or user creation failed
            return View("Register", registerVM);
        }


        //-------- LOGIN ---------------------------------------------------------------

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                //Check if user exists
                ApplicationUser applicationUser
                    = await userManager.FindByNameAsync(loginVM.UserName);

                //if user exist => not null
                if (applicationUser != null)
                {
                    //check password
                    bool IsFound = await userManager.CheckPasswordAsync(applicationUser, loginVM.Password);

                    if(IsFound == true)
                    {
                        //Sign in the user => Add cookie
                        await signInManager.SignInAsync(applicationUser, isPersistent: loginVM.RememberMe);
                        //Redirect to the home page
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid UserName Or Password.");
                    }
                }
            }
            //ModelState is not valid or user creation failed
            return View("Login", loginVM);
        }

        //--------LOGOUT--------------------------------------------------------


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            // Optional: clear TempData or session if needed
            TempData["success"] = "You have been logged out.";

            return RedirectToAction("Login", "Account", new { area = "User" });
        }

        //--------Access Deni--------------------------------------------------------
        [AllowAnonymous] //no need to be authenticated
        public IActionResult AccessDenied()
        {
           
            return View();
        }

    }
}
