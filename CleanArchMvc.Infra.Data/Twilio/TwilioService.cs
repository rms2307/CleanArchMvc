using CleanArchMvc.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace CleanArchMvc.Infrastructure.Twilio
{
    public class TwilioService : ISMSService
    {
        private readonly IConfiguration _configuration;

        public TwilioService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendSmsAsync(string number, string message)
        {
            string accountSid = _configuration["Twilio:AccountSid"];
            string authToken = _configuration["Twilio:AuthToken"];
            string fromPhoneNumber = _configuration["Twilio:PhoneNumber"];
            string toPhoneNumber = ConfigurePhoneNumber(number);

            TwilioClient.Init(accountSid, authToken);

            await MessageResource.CreateAsync(
                to: new PhoneNumber(toPhoneNumber),
                from: new PhoneNumber(fromPhoneNumber),
                body: message);
        }

        private static string ConfigurePhoneNumber(string number)
        {
            number = number.Replace("-", "")
                .Replace("+", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(" ","")
                .Trim();
            return $"+{number}";
        }
    }
}
