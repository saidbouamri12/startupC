using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StarApp.Core.Models;
using StartApp.EF.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartApp.Utilities
{
    public class DbInitializer : IdbInitializer
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public void Initialize()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Count()> 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }
            if(!_roleManager.RoleExistsAsync(WebSiteRole.WebSite_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebSiteRole.WebSite_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRole.WebSite_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRole.WebSite_Student)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRole.WebSite_Teacher)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                }, "Admin@123"
                ).GetAwaiter().GetResult();
                var appuser = _userManager.Users.Where(x=>x.Email== "admin@admin.com").FirstOrDefault();
                if(appuser != null)
                {
                    _userManager.AddToRoleAsync(appuser, WebSiteRole.WebSite_Admin).GetAwaiter().GetResult();
                }
            }
        }
    }
}
