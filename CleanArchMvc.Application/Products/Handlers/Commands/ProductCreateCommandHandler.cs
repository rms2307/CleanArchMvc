using CleanArchMvc.Application.Exceptions;
using CleanArchMvc.Application.Interfaces.Repositories;
using CleanArchMvc.Application.Products.Commands;
using CleanArchMvc.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Products.Handlers.Commands
{
    public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductCreateCommandHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Product> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(
                request.Name, request.Description, request.Price, request.Stock, request.Image, request.CategoryId);

            if (product is null)
                throw new BadRequestException($"Error creating entity");

            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
            if (category is null)
                throw new NotFoundException($"The category don't exist.");

            return await _productRepository.CreateAsync(product);
        }
    }
}
