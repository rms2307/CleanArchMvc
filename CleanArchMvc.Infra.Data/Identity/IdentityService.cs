using CleanArchMvc.Application.Exceptions;
using CleanArchMvc.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace CleanArchMvc.Infrastructure.Identity
{
    public class IdentityService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public IdentityService(UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            var userName = email.Split("@")[0];
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            if (!result.Succeeded)
                throw new UnauthorizedException("Usuário e/ou Senha incorretos.");

            return result.Succeeded;
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = email.Split("@")[0],
                Email = email,
                NormalizedUserName = email.Split("@")[0].ToUpper(),
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
                result = await _userManager.AddToRoleAsync(applicationUser, "User");

            if (result.Succeeded)
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);

            if (!result.Succeeded)
                throw new BadRequestException("Erro ao registrar usuário.");

            return result.Succeeded;
        }

        public async Task<bool> RegisterUser(string email, string password, string role)
        {
            if (!_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
                throw new ForbiddenException("Apenas usuários Admin podem criar outros usuários.");

            var applicationUser = new ApplicationUser
            {
                UserName = email.Split("@")[0],
                Email = email,
                NormalizedUserName = email.Split("@")[0].ToUpper(),
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
                result = await _userManager.AddToRoleAsync(applicationUser, role);

            if (result.Succeeded)
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);

            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}