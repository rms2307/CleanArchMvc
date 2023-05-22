using Microsoft.AspNetCore.Http;

namespace CleanArchMvc.Application.DTOs
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}