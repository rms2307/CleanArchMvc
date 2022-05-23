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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var produtos = await _productService.GetProductsAsync();
            if (produtos == null)
                return NotFound("Products not found");

            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        [Authorize]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var produto = await _productService.GetByIdAsync(id);
            if (produto == null)
                return NotFound("Product not found");

            return Ok(produto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Post([FromBody] ProductDTO produtoDto)
        {
            if (produtoDto == null)
                return BadRequest("Data Invalid");

            await _productService.AddAsync(produtoDto);

            return new CreatedAtRouteResult("GetProduct",
                new { id = produtoDto.Id }, produtoDto);
        }

        [HttpPost("UploadFile")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Endpoint responsável pelo upload do arquivo de produtos (.xlxs)."
        )]
        [ProducesResponseType(typeof(FormFileDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult> UploadFile([FromForm] IFormFile file)
        {
            var fileDTO = new FormFileDTO
            {
                Name = file.Name,
                FileName = file.FileName,
                ContentDisposition = file.ContentDisposition,
                ContentType = file.ContentType,
                Length = file.Length
            };

            await _productService.UploadFileAsync(fileDTO);

            return Ok(fileDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductDTO produtoDto)
        {
            if (id != produtoDto.Id)
                return BadRequest("Data invalid");

            if (produtoDto == null)
                return BadRequest("Data invalid");

            await _productService.UpdateAsync(produtoDto);

            return Ok(produtoDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var produtoDto = await _productService.GetByIdAsync(id);

            if (produtoDto == null)
                return NotFound("Product not found");

            await _productService.RemoveAsync(id);

            return Ok(produtoDto);
        }
    }
}