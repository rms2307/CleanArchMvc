using System.Threading.Tasks;

namespace CleanArchMvc.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(string userName);
    }
}