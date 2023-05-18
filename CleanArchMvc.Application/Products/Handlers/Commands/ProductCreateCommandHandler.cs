using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Exceptions;
using CleanArchMvc.Application.Interfaces.Repositories;
using CleanArchMvc.Application.Interfaces.Services;
using CleanArchMvc.Application.Products.Commands;
using CleanArchMvc.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Products.Handlers.Commands
{
    public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStorageService _storageService;

        public ProductCreateCommandHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IStorageService storageService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _storageService = storageService;
        }

        public async Task<Product> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            UploadFileDto uploadResponse = await _storageService.UploadFile(request.Image);

            Product product = new(request.Name,
                request.Description,
                request.Price,
                request.Stock,
                uploadResponse.FileUrl,
                uploadResponse.FileName,
                request.CategoryId);

            if (product is null)
                throw new BadRequestException($"Error creating product");

            Category category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
            if (category is null)
                throw new NotFoundException($"The category don't exist.");

            return await _productRepository.CreateAsync(product);
        }
    }
}
