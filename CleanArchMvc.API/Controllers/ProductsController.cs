using CleanArchMvc.API.ApiModels.Response;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(
            Summary = "Endpoint responsável por buscar todos produtos."
        )]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var produtos = await _productService.GetProductsAsync();

            return Ok(new ApiResponse<IEnumerable<ProductDTO>>(produtos));
        }

        [HttpGet("{id}", Name = "GetProduct")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Endpoint responsável por buscar um produto por id."
        )]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var produto = await _productService.GetByIdAsync(id);

            return Ok(new ApiResponse<ProductDTO>(produto));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Endpoint responsável cadastrar um produto (Admin)."
        )]
        public async Task<ActionResult> Post([FromBody] ProductDTO produtoDto)
        {
            await _productService.AddAsync(produtoDto);

            return Created(string.Empty, null);
        }

        [HttpPost("UploadFileListProducts")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Endpoint responsável pelo upload do arquivo com a lista de produtos (Admin)."
        )]
        [ProducesResponseType(typeof(Domain.VOs.FormFile), StatusCodes.Status200OK)]
        public async Task<ActionResult> UploadListProducts([FromForm] IFormFile file)
        {
            Domain.VOs.FormFile formFile = new(
                file.OpenReadStream(),
                file.Name,
                file.FileName,
                file.Length,
                file.ContentType);

            await _productService.UploadListProductsAsync(formFile);

            return Created(string.Empty, null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Endpoint responsável atualizar um produto (Admin)."
        )]
        public async Task<ActionResult> Put([FromBody] ProductDTO produtoDto)
        {
            await _productService.UpdateAsync(produtoDto);

            return Ok(produtoDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Endpoint responsável remover um produto (Admin)."
        )]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            await _productService.RemoveAsync(id);

            return Ok();
        }
    }
}