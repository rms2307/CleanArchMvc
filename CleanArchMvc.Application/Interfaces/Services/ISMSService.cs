using System.Threading.Tasks;

namespace CleanArchMvc.Application.Interfaces.Services
{
    public interface ISMSService
    {
        Task SendSmsAsync(string number, string message);
    }
}