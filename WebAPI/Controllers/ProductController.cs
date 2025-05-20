using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    [ApiController]
    [Produces("application/json")]
    [Route("products")]

    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDTO<ProductDTO>>> GetAllProductsAsync([FromQuery] ProductFilterDTO filter)
        {
            var result = await _productService.GetProductsListAsync(filter);
            return Ok(result);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<List<ProductDTO>>> GetLatestProductsAsync([Required] int quantity = 10)
        {
            var latestProducts = await _productService.GetLatestProductsAsync(quantity);
            return Ok(latestProducts);
        }

        [HttpGet("popular")]
        public async Task<ActionResult<List<ProductDTO>>> GetPopularProducts([FromQuery] int count = 10)
        {
            var products = await _productService.GetPopularProductsAsync(count);
            return Ok(products);
        }



        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetProductByIdAsync([Required] Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateProductAsync([FromForm][FromBody][Required] CreateProductDTO request)
        {
            var product = await _productService.CreateProductAsync(request);
            return Ok(product);
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateProductAsync([Required] Guid id, [FromBody][Required] UpdateProductDTO request)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, request);
            return Ok(updatedProduct);
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProductAsync([Required] Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

    }
}
