using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Domain.VOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProductsAsync();
        Task<ProductDTO> GetByIdAsync(int? id);
        Task AddAsync(CreateProductDTO productDTO);
        Task UploadListProductsAsync(FormFile file);
        Task UpdateAsync(UpdateProductDTO productDTO);
        Task RemoveAsync(int? id);
    }
}