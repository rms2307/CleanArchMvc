using System;

namespace CleanArchMvc.API.Domain.VOs
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}