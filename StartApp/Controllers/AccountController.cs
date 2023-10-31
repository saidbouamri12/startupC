using EmailSender.Service;
using ManagmentError;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using StarApp.Core.Models;
using StarApp.Core.ModelsView;
using StartApp.EF.DBContext;
using System.Reflection;
using System.Security.Claims;

namespace StartApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                FactoryError Log = new FactoryError("Login", ex.InnerException!.ToString());
                Log.Errorfile();
                return View();
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Login(Loginview model)
        {
            
            try
            {

                if(ModelState.IsValid)
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(model.email);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "User and password incorect");
                        return View(model);
                    }
                    var result = await _signInManager.PasswordSignInAsync(user, model.password, model.rememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Connexion Non Réussie");
                }
                return View(model);
            }
            catch(Exception ex)
            {
                FactoryError Log = new FactoryError("Login", ex.InnerException!.ToString());
                Log.Errorfile();
                return View(model);
            }
        }
        public IActionResult register()
        {
            return View();
        }


        

        public async Task<IActionResult> Confirmemail(string userId,string token)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                if(user==null|| string.IsNullOrEmpty(token))
                {
                    
                    return BadRequest();
                }
                var emailConfirmed = await _userManager.ConfirmEmailAsync(user, token);
                if(emailConfirmed.Succeeded)
                {
                    var claim = new Claim("RequireConfirmedEmail", "true");
                    var result = await _userManager.AddClaimAsync(user, claim);
                    return View();
                }
                return BadRequest();

            }
            catch(Exception ex)
            {
                FactoryError Log = new FactoryError("ConfirmemailView", ex.InnerException!.ToString());
                Log.Errorfile();
                return BadRequest();
            }
           
        }
        [HttpPost]
        public async  Task<IActionResult> register(registerview model)
        {
            EmailSender.Service.EmailSender Sender = new EmailSender.Service.EmailSender();
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.Username, Email = model.Email ,fullname = model.Fullname };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        if (token == null)
                        {
                            return BadRequest();
                        }
                        var confirmationLink = Url.Action("Confirmemail", "Account", new { userId = user.Id, token }, Request.Scheme);
                        bool resultmsg = Sender.SendEmail(model.Email, "Confirm your email", $"<h3>Please confirm your account by clicking this</h3> <br/> <a href='{confirmationLink}'>link</a>.");
                        if(!resultmsg)
                        {
                            return BadRequest();

                        }
                        return RedirectToAction("EmailNotConfirmed", "Account");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                FactoryError Log = new FactoryError("regiser", ex.InnerException!.ToString());
                Log.Errorfile();
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                FactoryError Log = new FactoryError("Logout", ex.InnerException!.ToString());
                Log.Errorfile();
                return RedirectToAction("Index", "Home");
            }
            
        }

        public IActionResult ForgotPassword()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                FactoryError Log = new FactoryError("ForgotPasswordview", ex.InnerException!.ToString());
                Log.Errorfile();
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {

            try
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Invalid email format.");
                    return View(model);
                }
                else
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    if(token == null)
                    {
                        return BadRequest();
                    }
                    EmailSender.Service.EmailSender Sender = new EmailSender.Service.EmailSender();
                    var confirmationLink = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, Request.Scheme);
                    Sender.SendEmail(model.Email,"Reset Password", $"<h3>Please Reset your Password by clicking this</h3> <br/> <a href='{confirmationLink}'>link</a>.");
                    return View(model);
                }
               
            }
            catch(Exception ex)
            {
                FactoryError Log = new FactoryError("ForgotPassword", ex.InnerException!.ToString());
                Log.Errorfile();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token , string Userid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Userid);
                ResetPasswordViewModel rest = new ResetPasswordViewModel();
                rest.token = token;
                rest.email = user.Email;
                if(user == null)
                {
                    return BadRequest();
                }
                return View(rest);
            }
            catch(Exception ex)
            {
                FactoryError Log = new FactoryError("ResetPasswordview", ex.InnerException!.ToString());
                Log.Errorfile();
                return RedirectToAction("Login");
            }
            
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            // Implement logic to reset the password using the provided code.
            try
            {
                if(ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.email);
                    if(user == null)
                    {
                        return BadRequest();
                    }
                    var result = await _userManager.ResetPasswordAsync(user, model.token, model.password);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation");
                    }
                }
                return View(model);
            }
            catch(Exception ex)
            {
                FactoryError Log = new FactoryError("ForgotPassword", ex.InnerException.ToString());
                Log.Errorfile();
                return View(model);
            }
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                FactoryError Log = new FactoryError("ChangePassword", ex.InnerException.ToString());
                Log.Errorfile();
                return View(model);
            }
        }

        public async Task<IActionResult> Usermanage()
        {
            try
            {
                //  var user = _context.Users.Include(u=>u.Role)
                var users = await _userManager.Users.ToListAsync();

                var userRoles = new List<UserRole>();

                foreach (var user in users)
                {
                    var roleIds = await _userManager.GetRolesAsync(user);

                    var roles = new List<IdentityRole>();
                    foreach (var roleId in roleIds)
                    {
                        var role = await _roleManager.FindByIdAsync(roleId);
                        if (role != null)
                        {
                            roles.Add(role);
                        }
                    }

                    userRoles.Add(new UserRole
                    {
                        Iduser = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Fullname = user.fullname,
                        Role = roles
                    });
                }
                return View(userRoles);
            }
            catch( Exception ex )
            {

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Usermanage(AssignRoleViewModel model)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public IActionResult RoleManager()
        {
            try
            {
                var role = _roleManager.Roles;
                return View(role);
            }
            catch(Exception ex)
            {
                FactoryError Log = new FactoryError("ChangePassword", ex.InnerException.ToString());
                Log.Errorfile();
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> RoleManager(CreateRoleview roleview)
        {
            if(!string.IsNullOrEmpty(roleview.Name))
            {
                var roleExists= await _roleManager.RoleExistsAsync(roleview.Name);
                if(!roleExists)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole { Name = roleview.Name, });
                    if(result.Succeeded)
                    {
                        return RedirectToAction("RoleManager",_roleManager.Roles);
                    }
                }
            }
            ModelState.AddModelError("", "Role Creation Failed");
            return View(roleview);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
       
        public IActionResult EmailNotConfirmed()
        {
            return View();
        }

        //public async Task<IActionResult> SendEmailConfirm(string userid)
        //{
        //    EmailSender.Service.EmailSender Sender = new EmailSender.Service.EmailSender();
        //    try
        //    {
        //        var user = await _userManager.FindByNameAsync(userid);
        //        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        if (token == null)
        //        {
        //            return BadRequest();
        //        }
        //        var confirmationLink = Url.Action("Confirmemail", "Account", new { userId = user.Id, token }, Request.Scheme);
        //        bool resultmsg = Sender.SendEmail(user.Email, "Confirm your email", $"<h3>Please confirm your account by clicking this</h3> <br/> <a href='{confirmationLink}'>link</a>.");
        //        if (!resultmsg)
        //        {
        //            return BadRequest();

        //        }
        //        return RedirectToAction("EmailNotConfirmed", "Account");
        //    }
        //    catch( Exception ex )
        //    {
        //        FactoryError Log = new FactoryError("ChangePassword", ex.InnerException.ToString());
        //        Log.Errorfile();
        //        return BadRequest();
        //    }

        //}
        [HttpPost]
        public IActionResult Lockuser()
        {
            try
            {

            }
            catch(Exception ex)
            {

            }
            return View();
        }
        [HttpPost]
        public IActionResult Unlockuser()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public IActionResult DeleteUser()
        {
            return View();
        }

        public IActionResult EditUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EditUser(EdituserView model)
        {
            return View();
        }

        public IActionResult ChangePassworduser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult changePassworduser(ChangePasswordViewModel model)
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendEmailtochangePassword()
        {
            return View();
        }

    }
}
