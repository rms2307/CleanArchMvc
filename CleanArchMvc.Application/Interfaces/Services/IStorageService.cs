using CleanArchMvc.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Interfaces.Services
{
    public interface IStorageService
    {
        Task<UploadFileDto> UploadFile(IFormFile file);
        Task DeleteFile(string fileName);
        Task<bool> CheckFileExists(string fileName);
    }
}
