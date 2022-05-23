using CleanArchMvc.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System;

namespace CleanArchMvc.Infrastructure.Identity
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(RoleManager<IdentityRole> roleManager,
              UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void SeedUsers()
        {
            CreateUser("usuario@localhost", "usuario@localhost", "USUARIO@LOCALHOST", "USUARIO@LOCALHOST", true, false, "Numsey#2021", "User");
            CreateUser("admin@localhost", "admin@localhost", "ADMIN@LOCALHOST", "ADMIN@LOCALHOST", true, false, "Numsey#2021", "Admin");
        }

        public void SeedRoles()
        {
            CreateRole("User", "USER");
            CreateRole("Admin", "ADMIN");
        }

        private void CreateRole(string roleName, string roleNormalizedName)
        {
            if (!_roleManager.RoleExistsAsync(roleName).Result)
            {
                IdentityRole role = new()
                {
                    Name = roleName,
                    NormalizedName = roleNormalizedName
                };
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
        }

        private void CreateUser(string userName, string email, string normalizedUserName, string normalizedEmail,
                                    bool emailConfirmed, bool lockoutEnabled, string password, string role)
        {
            if (_userManager.FindByEmailAsync(email).Result == null)
            {
                ApplicationUser user = new()
                {
                    UserName = userName,
                    Email = email,
                    NormalizedUserName = normalizedUserName,
                    NormalizedEmail = normalizedEmail,
                    EmailConfirmed = emailConfirmed,
                    LockoutEnabled = lockoutEnabled,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult result = _userManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                    _userManager.AddToRoleAsync(user, role).Wait();                
            }
        }
    }
}