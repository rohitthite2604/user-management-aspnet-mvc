using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImage { get; set; }
        public bool IsActive { get; set; } = true;

        public bool RemoveImage {  get; set; }
    }
}