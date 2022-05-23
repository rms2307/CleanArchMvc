using AutoMapper;
using CleanArchMvc.Application.Interfaces.Repositories;
using CleanArchMvc.Application.Interfaces.Services;
using CleanArchMvc.Application.Products.Commands;
using CleanArchMvc.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Products.Handlers.Commands
{
    public class ProductUploadListProductsCommandHandler : IRequestHandler<ProductUploadListProductsCommand, List<Product>>
    {
        public readonly IProductRepository _productRepository;
        public readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ProductUploadListProductsCommandHandler(IProductRepository productRepository,
            IFileService fileService, IMapper mapper)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<List<Product>> Handle(ProductUploadListProductsCommand request, CancellationToken cancellationToken)
        {
            var products = _fileService.ReadFile(request.FormFile);

            return await _productRepository.CreateAsync(products);
        }
    }
}
