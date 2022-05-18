using System;

namespace CleanArchMvc.API.DTOs.Account
{
    public class UserTokenDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}