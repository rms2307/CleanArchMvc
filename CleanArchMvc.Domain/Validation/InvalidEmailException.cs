using System;

namespace CleanArchMvc.Domain.Validation
{
    public class InvalidEmailException: Exception
    {
        public InvalidEmailException(string error) : base(error) { }
    }
}
