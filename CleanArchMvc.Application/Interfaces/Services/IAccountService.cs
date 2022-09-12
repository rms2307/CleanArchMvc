using CleanArchMvc.Application.DTOs.Account;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<bool> Authenticate(string email, string password);
        Task<bool> RegisterUser(RegisterUserDTO userDTO);
        Task<bool> RegisterUser(AdminRegisterUserDTO userDTO);
        Task Logout();
    }
}