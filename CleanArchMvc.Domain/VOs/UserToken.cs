using System;

namespace CleanArchMvc.Domain.VOs
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}