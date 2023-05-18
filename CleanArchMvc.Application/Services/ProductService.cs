using AutoMapper;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Exceptions;
using CleanArchMvc.Application.Interfaces.Services;
using CleanArchMvc.Application.Products.Commands;
using CleanArchMvc.Application.Products.Queries;
using CleanArchMvc.Domain.VOs;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ProductService(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
        {
            var result = await _mediator.Send(new GetProductsQuery());
            if (result == null)
                throw new NotFoundException("Products not found");

            return _mapper.Map<IEnumerable<ProductDTO>>(result);
        }

        public async Task<ProductDTO> GetByIdAsync(int? id)
        {
            var getProductByIdQuery = await _mediator.Send(new GetProductByIdQuery(id.Value));
            if (getProductByIdQuery == null)
                throw new NotFoundException("Product not found");

            return _mapper.Map<ProductDTO>(getProductByIdQuery);
        }

        public async Task AddAsync(CreateProductDTO productDTO)
        {
            ProductCreateCommand productCreateCommand = _mapper.Map<ProductCreateCommand>(productDTO);
            await _mediator.Send(productCreateCommand);
        }

        public async Task UpdateAsync(UpdateProductDTO productDTO)
        {
            await CanUpdateOrDelete(productDTO.Id);

            ProductUpdateCommand productUpdateCommand = _mapper.Map<ProductUpdateCommand>(productDTO);
            await _mediator.Send(productUpdateCommand);
        }

        public async Task RemoveAsync(int? id)
        {
            await CanUpdateOrDelete(id.Value);

            await _mediator.Send(new ProductRemoveCommand(id.Value));
        }

        public async Task UploadListProductsAsync(FormFile file)
        {
            await _mediator.Send(new ProductUploadListProductsCommand(file));
        }

        private async Task CanUpdateOrDelete(int id)
        {
            var getProductByIdQuery = await _mediator.Send(new GetProductByIdQuery(id));
            if (getProductByIdQuery == null)
                throw new NotFoundException("Product not found");
        }
    }
}