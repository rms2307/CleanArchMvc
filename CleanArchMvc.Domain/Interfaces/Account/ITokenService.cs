using System.Threading.Tasks;

namespace CleanArchMvc.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(string userName);
    }
}