using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Exceptions;
using CleanArchMvc.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            string userName = GetUsername(email);
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            if (!result.Succeeded)
                throw new UnauthorizedException("Usuário e/ou Senha incorretos.");

            return result.Succeeded;
        }

        public async Task<bool> RegisterUser(string email, string password, string phoneNumber)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = GetUsername(email),
                Email = email,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = true,
                NormalizedUserName = GetUsername(email).ToUpper(),
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
                result = await _userManager.AddToRoleAsync(applicationUser, RoleConstants.USER);

            if (result.Succeeded)
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);

            if (!result.Succeeded)
                throw new BadRequestException("Error: " + result.Errors
                    .Select(x => x.Description).Aggregate((ex1, ex2) => $"{ex1}, {ex2}"));

            return result.Succeeded;
        }

        public async Task<bool> RegisterUser(string email, string password, string phoneNumber, string role)
        {
            if (!_httpContextAccessor.HttpContext.User.IsInRole(RoleConstants.ADMIN))
                throw new ForbiddenException("Apenas usuários Admin podem criar outros usuários.");

            var applicationUser = new ApplicationUser
            {
                UserName = GetUsername(email),
                Email = email,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = true,
                NormalizedUserName = GetUsername(email).ToUpper(),
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

            if (!result.Succeeded)
                throw new BadRequestException("Error: " + result.Errors
                    .Select(x => x.Description).Aggregate((ex1, ex2) => $"{ex1}, {ex2}"));

            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        private static string GetUsername(string email)
        {
            return email.Split("@")[0];
        }
    }
}