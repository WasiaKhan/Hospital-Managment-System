using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Utilities
{
    public class DbInitializer : IDbInitializer
    {

        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
                
            }
            catch(Exception) {
                throw;
            }
            if (!_roleManager.RoleExistsAsync(WebsiteRoles.Website_Admin).GetAwaiter().GetResult()) { 
                
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Website_Admin)).GetAwaiter().GetResult();  
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Website_Patient)).GetAwaiter().GetResult();  
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Website_Doctor)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "John Smith",
                    Email = "john.smith@gmail.com"
                },"john@123").GetAwaiter().GetResult();
                var AppUser = _context.ApplicationUsers.FirstOrDefault(x => x.Email == "john.smith@gmail.com");
                if (AppUser != null)
                {
                    _userManager.AddToRoleAsync(AppUser, WebsiteRoles.Website_Admin).GetAwaiter().GetResult();
                }

            }
        }
    }
}
