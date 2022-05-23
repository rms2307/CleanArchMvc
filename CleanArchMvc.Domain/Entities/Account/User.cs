using CleanArchMvc.Domain.Validation;
using CleanArchMvc.Domain.VOs;

namespace CleanArchMvc.Domain.Entities.Account
{
    public class User : BaseEntity
    {
        public string UserGuid { get; private set; }
        public Email Email { get; private set; }

        public User(int id, Email email)
        {
            ValidateDomain(id);
            Email = email;
        }

        private void ValidateDomain(int id)
        {
            DomainExceptionValidation.When(id < 0,
                "Invalid id. Id can't null");

            Id = id;
        }
    }
}