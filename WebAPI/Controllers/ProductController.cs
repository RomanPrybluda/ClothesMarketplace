using Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
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
        public async Task<ActionResult> GetAllProductsAsync()
        {
            var products = await _productService.GetProductsListAsync();
            return Ok(products);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetProductByIdAsync([Required] Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        [SwaggerRequestExample(typeof(CreateProductDTO), typeof(CreateProductExample))]
        public async Task<ActionResult> CreateProductAsync([FromBody][Required] CreateProductDTO request)
        {
            var product = await _productService.CreateProductAsync(request);
            return Ok(product);
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateProductAsync([Required] Guid id, [FromBody][Required] UpdateProductDTO request)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, request);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProductAsync([Required] Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        public class CreateProductExample : IExamplesProvider<CreateProductDTO>
        {
            public CreateProductDTO GetExamples()
            {
                return new CreateProductDTO
                {
                    Name = "Product name",
                    Description = "Description of product",
                    DollarPrice = 100,
                    Images = new List<ProductImageDTO>
                {
                    new ProductImageDTO { ImageUrl = "image1.jpg", IsMain = true },
                    new ProductImageDTO { ImageUrl = "image2.jpg", IsMain = false },
                    new ProductImageDTO { ImageUrl = "image3.jpg", IsMain = false },
                    new ProductImageDTO { ImageUrl = "image4.jpg", IsMain = false },
                    new ProductImageDTO { ImageUrl = "image5.jpg", IsMain = false },
                    new ProductImageDTO { ImageUrl = "image6.jpg", IsMain = false },
                    new ProductImageDTO { ImageUrl = "image7.jpg", IsMain = false },
                    new ProductImageDTO { ImageUrl = "image8.jpg", IsMain = false },
                    new ProductImageDTO { ImageUrl = "image9.jpg", IsMain = false },
                    new ProductImageDTO { ImageUrl = "image10.jpg", IsMain = false }
                },
                    BrandId = Guid.NewGuid(),
                    CategoryId = Guid.NewGuid(),
                    ForWhomId = Guid.NewGuid(),
                    ProductConditionId = Guid.NewGuid()
                };
            }
        }

    }
}
