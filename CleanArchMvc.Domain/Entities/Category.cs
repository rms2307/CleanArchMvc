using CleanArchMvc.Domain.Interfaces.Account;
using CleanArchMvc.Domain.Validation;
using System;
using System.Collections.Generic;

namespace CleanArchMvc.Domain.Entities
{
    public sealed class Category : BaseEntity, ISignedChanges
    {
        public string Name { get; private set; }

        public ICollection<Product> Products { get; set; }

        public DateTime ModifiedWhen { get; set; }

        public string ModifiedBy { get; set; }

        public Category(int id, string name)
        {
            DomainExceptionValidation.When(id < 0, "Invalid Id value!");
            Id = id;

            ValidateDomain(name);
        }

        public Category(string name)
        {
            ValidateDomain(name);
        }

        public void Update(string name)
        {
            ValidateDomain(name);
        }

        private void ValidateDomain(string name)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(name),
                "Invalid name. Name is required!");

            DomainExceptionValidation.When(name.Length < 3,
                "Invalid name. Minimum 3 charecters!");

            Name = name;
        }

        public void SignChanges(string username)
        {
            ModifiedBy = username;
            ModifiedWhen = DateTime.Now;
        }
    }
}