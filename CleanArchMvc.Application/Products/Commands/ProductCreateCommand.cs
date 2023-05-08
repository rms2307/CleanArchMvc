using Microsoft.AspNetCore.Http;

namespace CleanArchMvc.Application.Products.Commands
{
    public class ProductCreateCommand : ProductCommand 
    {
        public IFormFile Image { get; set; }
    }
}