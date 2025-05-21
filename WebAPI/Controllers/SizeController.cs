using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    [ApiController]
    [Produces("application/json")]
    [Route("sizes")]
    public class SizeController : ControllerBase
    {
        private readonly SizeService _sizeService;

        public SizeController(SizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet]
        public async Task<ActionResult> GetSizesAsync()
        {
            var sizes = await _sizeService.GetSizesAsync();
            return Ok(sizes);
        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetSizeByIdAsync([FromRoute][Required] Guid id)
        {
            var size = await _sizeService.GetSizeByIdAsync(id);
            return Ok(size);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateSizeAsync([FromBody][Required] CreateSizeDTO request)
        {
            var size = await _sizeService.CreateSizeAsync(request);
            return Ok(size);
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateSizeAsync([FromRoute][Required] Guid id, [FromBody] UpdateSizeDTO request)
        {
            var size = await _sizeService.UpdateSizeAsync(id, request);
            return Ok(size);
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteSizeAsync([FromRoute][Required] Guid id)
        {
            await _sizeService.DeleteSizeAsync(id);
            return NoContent();
        }
    }
}