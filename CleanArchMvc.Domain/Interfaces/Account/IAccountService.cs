using System.Threading.Tasks;

namespace CleanArchMvc.Domain.Interfaces.Account
{
    public interface IAccountService
    {
        Task<bool> Authenticate(string email, string password);
        Task<bool> RegisterUser(string email, string password);
        Task<bool> RegisterUser(string email, string password, string role);
        Task Logout();
    }
}