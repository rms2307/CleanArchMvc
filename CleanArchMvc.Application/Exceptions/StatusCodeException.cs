using System;

namespace CleanArchMvc.Application.Exceptions
{
    public class StatusCodeException : Exception
    {
        private StatusCodeException(string messsage) : base(messsage)
        {
        }

        public StatusCodeException(int statusCode, string message) : this($"_$!_{statusCode}_$!_ {message}")
        {
            if (statusCode < 99 || statusCode > 999)
                throw new ArgumentException("Status code deve estar entre 100 e 999.");
        }
    }
}
