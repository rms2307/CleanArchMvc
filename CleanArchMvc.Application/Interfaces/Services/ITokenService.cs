using CleanArchMvc.API.Domain.VOs;
using CleanArchMvc.Domain.VOs;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<UserToken> GetTokenAsync(string userName);
    }
}