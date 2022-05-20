using System.Threading.Tasks;

namespace CleanArchMvc.Domain.Interfaces.Account
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(string email, string password);
        Task<bool> RegisterUser(string email, string password);
        Task<bool> RegisterUser(string email, string password, string role);
        Task Logout();
    }
}