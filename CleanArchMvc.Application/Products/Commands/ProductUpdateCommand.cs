namespace CleanArchMvc.Application.Products.Commands
{
    public class ProductUpdateCommand : ProductCommand
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }
}