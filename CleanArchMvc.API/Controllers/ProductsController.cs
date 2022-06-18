using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces.Services;
using CleanArchMvc.Domain.VOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
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
            await _productService.AddAsync(produtoDto);

            return Created(string.Empty, null);
        }

        [HttpPost("UploadListProducts")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Endpoint responsável pelo upload do arquivo com a lista de produtos."
        )]
        [ProducesResponseType(typeof(Domain.VOs.FormFile), StatusCodes.Status200OK)]
        public async Task<ActionResult> UploadListProducts([FromForm] IFormFile file)
        {
            try
            {
                Domain.VOs.FormFile formFile = new(
                    file.OpenReadStream(),
                    file.Name,
                    file.FileName,
                    file.Length,
                    file.ContentType
                    );

                await _productService.UploadListProductsAsync(formFile);

                return Created(string.Empty, null);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Errors", ex.Message);
                return BadRequest(ModelState);
            }
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