using CleanArchMvc.Domain.Validation;

namespace CleanArchMvc.Domain.Entities.Account
{
    public class User : BaseEntity
    {
        public string UserGuid { get; private set; }
        public string Email { get; private set; }

        public User(int id, string email)
        {
            ValidateDomain(id, email);
        }

        private void ValidateDomain(int id, string email)
        {
            DomainExceptionValidation.When(id < 0,
                "Invalid id. Id can't null");

            DomainExceptionValidation.When(string.IsNullOrEmpty(email),
                "Invalid Email. Email can't null");
            DomainExceptionValidation.When(!email.Contains("@"),
                "Invalid Email");

            Id = id;
            Email = email;
        }
    }
}