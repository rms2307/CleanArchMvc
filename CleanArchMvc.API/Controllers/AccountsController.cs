using CleanArchMvc.API.Annotations;
using CleanArchMvc.API.ApiModels.Response;
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
        [SwaggerOperation(Summary = "Endpoint responsável pelo login do usuário.")]
        [ProducesResponseType(typeof(ApiResponse<AuthenticationResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<AuthenticationResponseDTO>>> Login([FromBody] LoginDTO login)
        {
            AuthenticationResponseDTO userToken = await _accountService.Authenticate(login.Email, login.Password);

            return Ok(new ApiResponse<AuthenticationResponseDTO>(userToken));
        }

        [HttpPost("LoginTwoFactor")]
        [ValidForm]
        [SwaggerOperation(Summary = "Endpoint responsável por verificar código de dois fatores e fazer login se código valido.")]
        [ProducesResponseType(typeof(ApiResponse<AuthenticationResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<AuthenticationResponseDTO>>> VerifyTwoFactor([FromBody] LoginTwoFactorDTO login)
        {
            AuthenticationResponseDTO userToken = await _accountService.Authenticate(login);

            return Ok(new ApiResponse<AuthenticationResponseDTO>(userToken));
        }

        [HttpPost("CreateUser")]
        [ValidForm]
        [SwaggerOperation(Summary = "Endpoint responsável por registrar um usuário.")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterUserDTO userDTO)
        {
            await _accountService.RegisterUser(userDTO);

            return Created(string.Empty, null);
        }

        [HttpPost("AdminCreateUser")]
        [ValidForm]
        [SwaggerOperation(Summary = "Endpoint para um usuário com a role Admin registrar novos usuários")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminCreateUser([FromBody] AdminRegisterUserDTO userDTO)
        {
            await _accountService.RegisterUser(userDTO);

            return Created(string.Empty, new ApiResponse<AdminRegisterUserDTO>(userDTO));
        }
    }
}