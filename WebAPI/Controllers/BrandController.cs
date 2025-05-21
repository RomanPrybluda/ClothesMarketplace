using Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("brands")]
    public class BrandController : ControllerBase
    {
        private BrandService _brandService;

        public BrandController(BrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandsAsync()
        {
            var brands = await _brandService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetBrandByIdAsync([FromRoute][Required] Guid id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            return Ok(brand);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrandAsync([FromForm][Required] CreateBrandDTO request)
        {
            var brand = await _brandService.CreateBrandAsync(request);
            return Ok(brand);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateBrandAsync([Required] Guid id, [FromForm] UpdateBrandDTO request)
        {
            var brand = await _brandService.UpdateBrandAsync(id, request);
            return Ok(brand);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteBrandAsync([Required] Guid id)
        {
            await _brandService.DeleteBrandAsync(id);
            return NoContent();
        }


    }
}
