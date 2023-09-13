using Furniture.DAL;
using Furniture.Models;
using Furniture.Services;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Controllers
{
    public class AccountController : Controller
    {
        private readonly FurnutireContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(FurnutireContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (await _userManager.Users.AnyAsync(x=>x.Email==userVM.Email))
            {
                ModelState.AddModelError("Email", "Email already taken");
                return View();
            }
            if (await _userManager.Users.AnyAsync(x=>x.UserName==userVM.UserName))
            {
                ModelState.AddModelError("Username", "Username already taken");
                return View();

            }
            AppUser user = new AppUser
            {
                IsAdmin=false,
                Email=userVM.Email,
                UserName=userVM.UserName,

            };
            var result  = await _userManager.CreateAsync(user,userVM.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Member");
            var token =await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("confirmemail", "account", new {token,email=userVM.Email},Request.Scheme);
            _emailSender.Send(userVM.Email, "Email confirmation Link", confirmationLink);
            return View("CheckEmail");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ?"SuccessRegister":"Error");
        }
        public IActionResult Login(string returnUrl=null)
        {
            if (returnUrl!=null)
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin,string returnUrl=null)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Email or password incorrect");
                return View(userLogin);
            }
            AppUser user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user == null || user.IsAdmin)
            {
                ModelState.AddModelError("", "Email or Password incorrect");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, userLogin.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password incorrect");
                return View();
            }
            if (returnUrl!=null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetVM)
        {
            if (!ModelState.IsValid) 
            { 
                return View();
            };
            AppUser user = _context.AppUsers.FirstOrDefault(x => x.Email == forgetVM.Email);
            if (user == null || user.IsAdmin)
            {
                ModelState.AddModelError("Email", "Email not found");
                return View(); 
            };
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = Url.Action("resetpassword", "account", new { email = forgetVM.Email, token = token }, Request.Scheme);
            _emailSender.Send(forgetVM.Email, "Reset Password", $" Click <a href=\"{url}\"> Here</a>");

            return View("CheckEmail");
        }
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.IsAdmin || !await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
            {
                return View("Error");
            }
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetVM)
        {
            ViewBag.Email = resetVM.Email;
            ViewBag.Token = resetVM.Token;

            AppUser user = await _userManager.FindByEmailAsync(resetVM.Email);

            if (user == null || user.IsAdmin)
            { 
                ModelState.AddModelError("Email", "Email not found");
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(user, resetVM.Token, resetVM.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Write correctly");
                return View();
            }
            return RedirectToAction("login");
        }
    }
}
