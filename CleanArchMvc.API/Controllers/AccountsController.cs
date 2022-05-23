using CleanArchMvc.API.DTOs.Account;
using CleanArchMvc.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountsController(IAccountService accountService, ITokenService tokenService, IConfiguration configuration)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        [SwaggerOperation(
            Summary = "Endpoint responsável pelo login do usuário."
        )]
        [ProducesResponseType(typeof(UserTokenDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] LoginDTO login)
        {
            var result = await _accountService.Authenticate(login.Email, login.Password);

            if (result)
            {
                var userToken = new UserTokenDTO
                {
                    Token = await _tokenService.GetTokenAsync(login.Email),
                    Expiration = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:Expires"]))
                };

                return Ok(userToken);
            }

            ModelState.AddModelError("Errors", "Invalid login attempt.");
            return BadRequest(ModelState);
        }

        [HttpPost("CreateUser")]
        [SwaggerOperation(
            Summary = "Endpoint responsável por registrar um usuário."
        )]
        public async Task<ActionResult> CreateUser([FromBody] RegisterUserDTO userDTO)
        {
            var result = await _accountService.RegisterUser(userDTO.Email, userDTO.Password);

            if (result)
                return Ok($"User {userDTO.Email} was created successfully");

            ModelState.AddModelError("Errors", "Invalid create user attempt.");
            return BadRequest(ModelState);
        }

        [HttpPost("AdminCreateUser")]
        [SwaggerOperation(
            Summary = "Endpoint para um usuário com a role Admin registrar outros usuários"
        )]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminCreateUser([FromBody] AdminRegisterUserDTO userDTO)
        {
            try
            {
                var result = await _accountService.RegisterUser(userDTO.Email, userDTO.Password, userDTO.Role);

                if (result)
                    return Ok($"User {userDTO.Email} was created successfully");

                ModelState.AddModelError("Errors", "Invalid create user attempt.");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Errors", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}