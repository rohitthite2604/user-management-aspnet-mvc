using Application.Identity;
using Application.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;

namespace Application.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(int? page)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(db);
            var userManager = new ApplicationUserManager(userStore);
            List<ApplicationUser> existingUsers = db.Users.ToList().Where(temp => !userManager.IsInRole(temp.Id, "Admin")).ToList(); ;

            int pageSize = 10;
            int pageNumber = (page ?? 1); // Default to the first page
            IPagedList<ApplicationUser> pagedUsers = existingUsers.ToPagedList(pageNumber, pageSize);
            return View(pagedUsers);

        }
        public async Task<ActionResult> ToggleActivation(string userId)
        {
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            ApplicationUser appUser = await userManager.FindByIdAsync(userId);
            if(appUser!=null)
            {
                appUser.IsActive = !appUser.IsActive;
                await userManager.UpdateAsync(appUser);
            }

            return RedirectToAction("Index","Admin");
        }
    }
}