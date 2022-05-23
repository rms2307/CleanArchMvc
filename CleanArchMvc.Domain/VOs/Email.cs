using CleanArchMvc.Domain.Validation;
using System.Text.RegularExpressions;

namespace CleanArchMvc.Domain.VOs
{
    public class Email
    {
        public Email(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new InvalidEmailException("Email is required!");

            Address = address.ToLower().Trim();
            const string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            if (!Regex.IsMatch(address, pattern))
                throw new InvalidEmailException("Email invalid");
        }

        public string Address { get; }
    }
}