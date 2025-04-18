﻿using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    [ApiController]
    [Produces("application/json")]
    [Route("products")]
    [Authorize]

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

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetProductByIdAsync([Required] Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductAsync([FromForm][FromBody][Required] CreateProductDTO request)
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

    }
}
