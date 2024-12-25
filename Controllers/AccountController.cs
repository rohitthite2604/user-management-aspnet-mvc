using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Models;
using Application.ViewModels;
using Application.Identity;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using System.Data.Entity;
using PagedList;
using System.IO;

namespace Application.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext appDbContext;
        private ApplicationUserManager userManager;

        public AccountController()
        {
            appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            userManager = new ApplicationUserManager(userStore);
        }
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                var appDbContext = new ApplicationDbContext();
                var userStore = new ApplicationUserStore(appDbContext);
                var userManager = new ApplicationUserManager(userStore);
                var passwordHash = Crypto.HashPassword(rvm.Password);
                var user = new ApplicationUser() { Email = rvm.Email, UserName = rvm.Username, PasswordHash = passwordHash, PhoneNumber = rvm.Mobile };
                if (Request.Files.Count >= 1)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            // Convert to Base64 string (if still needed)
                            var imgBytes = reader.ReadBytes(file.ContentLength);
                            var base64String = Convert.ToBase64String(imgBytes, 0, imgBytes.Length);
                            user.ProfileImage = base64String;

                            // Save to folder
                            string folderPath = Server.MapPath("~/Images/");
                            if (!System.IO.Directory.Exists(folderPath))
                            {
                                System.IO.Directory.CreateDirectory(folderPath);
                            }

                            // Generate a unique filename
                            string fileName = Path.GetFileName(file.FileName);
                            string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                            string filePath = Path.Combine(folderPath, uniqueFileName);

                            // Save the file
                            file.SaveAs(filePath);

                            // Store the file path in ViewBag
                            ViewBag.ProfileImagePath = $"~/Images/{uniqueFileName}";
                        }
                    }
                }

                IdentityResult result = userManager.Create(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                
            }
            else
            {
                ModelState.AddModelError("MyError", "Invalid Data");
                return View();
            }
            return View(rvm);
        }
       
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel lvm)
        {
            if (!ModelState.IsValid)
            {
                return View(lvm);
            }
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            ApplicationUser user = null;
            if (lvm.Email.Contains("@"))
            {
                user = userManager.FindByEmail(lvm.Email);
            }
            else
            {
                user = userManager.Users.FirstOrDefault(u => u.PhoneNumber == lvm.Email);
            }

            if (user != null && userManager.CheckPassword(user, lvm.Password))
            {
                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "This account has been deactivated.");
                    ViewBag.Error = "Your account has been deactivated. Please contact Admin.";
                    return View(lvm);
                }

                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties(), userIdentity);

                if (userManager.IsInRole(user.Id, "Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("myerror", "Invalid email/mobile number or password");
                return View(lvm);
            }
        }
        public ActionResult Logout()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index","Home");
        }
        public ActionResult MyProfile()
        {
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            return View(user);
        }
        public ActionResult EditProfile()
        {
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            ApplicationUser appUser = userManager.FindById(User.Identity.GetUserId());
            var model = new RegisterViewModel
            {
                Username = appUser.UserName,
                Email = appUser.Email,
                Mobile = appUser.PhoneNumber,
                ProfileImage = appUser.ProfileImage
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(RegisterViewModel model)
        {
            ApplicationUser appUser = userManager.FindById(User.Identity.GetUserId());
            appUser.UserName = model.Username;
            appUser.Email = model.Email;
            appUser.PhoneNumber = model.Mobile;
            if (Request.Files.Count >= 1)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        // Convert to Base64 string (if still needed)
                        var imgBytes = reader.ReadBytes(file.ContentLength);
                        var base64String = Convert.ToBase64String(imgBytes, 0, imgBytes.Length);
                        appUser.ProfileImage = base64String;

                        // Save to folder
                        string folderPath = Server.MapPath("~/Images/");
                        if (!System.IO.Directory.Exists(folderPath))
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }

                        // Generate a unique filename
                        string fileName = Path.GetFileName(file.FileName);
                        string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                        string filePath = Path.Combine(folderPath, uniqueFileName);

                        // Save the file
                        file.SaveAs(filePath);

                        // Store the file path in ViewBag
                        ViewBag.ProfileImagePath = $"~/Images/{uniqueFileName}";
                    }
                }
            }
            IdentityResult result = userManager.Update(appUser);
            if (result.Succeeded)
            {
                if (userManager.IsInRole(appUser.Id, "Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        
    }
}