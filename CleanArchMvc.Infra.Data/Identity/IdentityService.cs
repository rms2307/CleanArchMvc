using CleanArchMvc.Application.DTOs.Account;
using CleanArchMvc.Application.Exceptions;
using CleanArchMvc.Application.Interfaces.Services;
using CleanArchMvc.Domain.VOs;
using ExcelDataReader.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace CleanArchMvc.Infrastructure.Identity
{
    public class IdentityService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly ISMSService _SMSService;

        private const string TOKEN_PROVIDER = "Phone";

        public IdentityService(UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IHttpContextAccessor httpContextAccessor,
                ITokenService tokenService,
                ISMSService sMSService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _SMSService = sMSService;
        }

        public async Task<AuthenticationResponseDTO> Authenticate(string email, string password)
        {
            ApplicationUser user = await GetUser(email);

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new UnauthorizedException("Usuário e/ou Senha incorretos.");

            if (user.TwoFactorEnabled)
            {
                string token = await GenerateTwoFactorToken(user);
                await SendTwoFactorToken(user, token);

                return new AuthenticationResponseDTO
                {
                    TwoFactorEnabled = true,
                    Message = "Código de verificação enviado para número de celular cadastrado."
                };
            }

            result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);
            if (!result.Succeeded)
                throw new UnauthorizedException("Usuário e/ou Senha incorretos.");

            UserToken userToken = await _tokenService.GetTokenAsync(email);

            return new AuthenticationResponseDTO
            {
                IsAuthenticated = true,
                Token = userToken.Token,
                Expiration = userToken.Expiration,
                Message = "Sucesso ao gerar token de acesso."
            };
        }

        public async Task<AuthenticationResponseDTO> Authenticate(LoginTwoFactorDTO login)
        {
            ApplicationUser user = await GetUser(login.Email);

            bool validToken = await _userManager.VerifyTwoFactorTokenAsync(user, TOKEN_PROVIDER, login.Token);
            if (!validToken)
                throw new UnauthorizedException("Código de verificação invalido.");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded)
                throw new UnauthorizedException("Usuário e/ou Senha incorretos.");

            UserToken userToken = await _tokenService.GetTokenAsync(user.Email);

            return new AuthenticationResponseDTO
            {
                TwoFactorEnabled = true,
                IsAuthenticated = true,
                Token = userToken.Token,
                Expiration = userToken.Expiration,
                Message = "Sucesso ao gerar token de acesso."
            };
        }

        private async Task<ApplicationUser> GetUser(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new UnauthorizedException("Usuário e/ou Senha incorretos.");

            return user;
        }

        public async Task<bool> RegisterUser(RegisterUserDTO dto)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = GetUsername(dto.Email),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PhoneNumberConfirmed = true,
                NormalizedUserName = GetUsername(dto.Email).ToUpper(),
                NormalizedEmail = dto.Email.ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = dto.EnabledTwoFactor,
            };

            var result = await _userManager.CreateAsync(applicationUser, dto.Password);

            if (result.Succeeded)
                result = await _userManager.AddToRoleAsync(applicationUser, RoleConstants.USER);

            if (result.Succeeded)
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);

            if (!result.Succeeded)
                throw new BadRequestException("Error: " + result.Errors
                    .Select(x => x.Description).Aggregate((ex1, ex2) => $"{ex1}, {ex2}"));

            return result.Succeeded;
        }

        public async Task<bool> RegisterUser(AdminRegisterUserDTO dto)
        {
            if (!_httpContextAccessor.HttpContext.User.IsInRole(RoleConstants.ADMIN))
                throw new ForbiddenException("Apenas usuários Admin podem criar outros usuários.");

            var applicationUser = new ApplicationUser
            {
                UserName = GetUsername(dto.Email),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PhoneNumberConfirmed = true,
                NormalizedUserName = GetUsername(dto.Email).ToUpper(),
                NormalizedEmail = dto.Email.ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = dto.EnabledTwoFactor,
            };

            var result = await _userManager.CreateAsync(applicationUser, dto.Password);

            if (result.Succeeded)
                result = await _userManager.AddToRoleAsync(applicationUser, dto.Role);

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

        private async Task SendTwoFactorToken(ApplicationUser user, string token)
        {
            string message = $"Seu código de verificação é: {token}";

            await _SMSService.SendSmsAsync(user.PhoneNumber, message);
        }

        private async Task<string> GenerateTwoFactorToken(ApplicationUser user)
        {
            return await _userManager.GenerateTwoFactorTokenAsync(user, TOKEN_PROVIDER);
        }

        private static string GetUsername(string email)
        {
            return email.Split("@")[0];
        }
    }
}