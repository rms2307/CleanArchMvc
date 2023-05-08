using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Interfaces.Services
{
    public interface IStorageService
    {
        Task<string> UploadFile(IFormFile file);
    }
}
