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
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStorageService _storageService;

        public ProductUpdateCommandHandler(IProductRepository productRepository, IStorageService storageService)
        {
            _productRepository = productRepository;
            _storageService = storageService;
        }

        public async Task<Product> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            Product product = await _productRepository.GetProductAsync(request.Id);
            if (product is null)
                throw new NotFoundException($"Product {request.Id} could not be found");

            UploadFileDto uploadResponse = await UpdateImageInStorage(request, product);

            product.Update(
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                uploadResponse.FileUrl,
                uploadResponse.FileName,
                request.CategoryId);

            return await _productRepository.UpdateAsync(product);
        }

        private async Task<UploadFileDto> UpdateImageInStorage(ProductUpdateCommand request, Product product)
        {
            await _storageService.DeleteFile(product.ImageName);
            UploadFileDto uploadResponse = await _storageService.UploadFile(request.Image);
            return uploadResponse;
        }
    }
}