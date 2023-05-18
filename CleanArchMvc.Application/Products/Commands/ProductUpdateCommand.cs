using Microsoft.AspNetCore.Http;

namespace CleanArchMvc.Application.Products.Commands
{
    public class ProductUpdateCommand : ProductCommand
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }
}