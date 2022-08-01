using CleanArchMvc.Application.DTOs;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<bool> Authenticate(string email, string password);
        Task<bool> RegisterUser(string email, string password, string phoneNumber);
        Task<bool> RegisterUser(string email, string password, string phoneNumber, string role);
        Task Logout();
    }
}