﻿using CleanArchMvc.Domain.Interfaces;
using CleanArchMvc.Domain.Validation;
using System;

namespace CleanArchMvc.Domain.Entities
{
    public sealed class Product : BaseEntity, ISignedChanges
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public string ImageUrl { get; private set; }
        public string ImageName { get ; private set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime ModifiedWhen { get; set; }

        public string ModifiedBy { get; set; }

        public Product(string name, string description, decimal price, int stock, string imageUrl, string imageName, int categoryId)
        {
            ValidateDomain(name, description, price, stock, imageUrl, imageName, categoryId);
        }

        public Product(int id, string name, string description, decimal price, int stock, string imageUrl, string imageName, int categoryId)
        {
            DomainExceptionValidation.When(id <= 0, "Invalid Id value!");
            Id = id;
            ValidateDomain(name, description, price, stock, imageUrl, ImageName, categoryId);
        }

        public void Update(string name, string description, decimal price, int stock, string imageUrl, string imageName, int categoryId)
        {
            ValidateDomain(name, description, price, stock, imageUrl, imageName, categoryId);
        }

        private void ValidateDomain(string name, string description, decimal price, int stock, string imageUrl, string imageName, int categoryId)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(name),
                "Invalid name. Name is required!");

            DomainExceptionValidation.When(name.Length < 3,
                "Invalid name, too short, minimum 3 characters!");

            DomainExceptionValidation.When(string.IsNullOrEmpty(description),
                "Invalid description. Description is required!");

            DomainExceptionValidation.When(description.Length < 5,
                "Invalid description, too short, minimum 5 characters!");

            DomainExceptionValidation.When(price < 0, "Invalid price value!");

            DomainExceptionValidation.When(stock < 0, "Invalid stock value!");

            DomainExceptionValidation.When(categoryId < 0,
                "Invalid Id. Id is required!");

            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            ImageUrl = imageUrl;
            ImageName = imageName;
            CategoryId = categoryId;
        }

        public void SignChanges(string username)
        {
            ModifiedBy = username;
            ModifiedWhen = DateTime.Now;
        }
    }
}