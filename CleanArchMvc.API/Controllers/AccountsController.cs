using CleanArchMvc.API.Annotations;
using CleanArchMvc.API.ApiModels.Response;
using CleanArchMvc.API.Domain.VOs;
using CleanArchMvc.Application.DTOs.Account;
using CleanArchMvc.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
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
        [ValidForm]
        [SwaggerOperation(
            Summary = "Endpoint responsável pelo login do usuário."
        )]
        [ProducesResponseType(typeof(ApiResponse<UserToken>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<UserToken>>> Login([FromBody] LoginDTO login)
        {
            await _accountService.Authenticate(login.Email, login.Password);

            return Ok(new ApiResponse<UserToken>(await _tokenService.GetTokenAsync(login.Email)));
        }

        [HttpPost("CreateUser")]
        [ValidForm]
        [SwaggerOperation(
            Summary = "Endpoint responsável por registrar um usuário."
        )]
        public async Task<ActionResult> CreateUser([FromBody] RegisterUserDTO userDTO)
        {
            await _accountService.RegisterUser(userDTO.Email, userDTO.Password, userDTO.PhoneNumber);

            return Created(string.Empty, null);
        }

        [HttpPost("AdminCreateUser")]
        [ValidForm]
        [SwaggerOperation(
            Summary = "Endpoint para um usuário com a role Admin registrar outros usuários"
        )]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminCreateUser([FromBody] AdminRegisterUserDTO userDTO)
        {
            await _accountService.RegisterUser(userDTO.Email, userDTO.Password, userDTO.PhoneNumber, userDTO.Role);

            return Created(string.Empty, new ApiResponse<AdminRegisterUserDTO>(userDTO));
        }
    }
}