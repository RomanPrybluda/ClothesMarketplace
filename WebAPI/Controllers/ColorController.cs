using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    [ApiController]
    [Produces("application/json")]
    [Route("colors")]

    public class ColorController : ControllerBase
    {
        private ColorService _colorService;

        public ColorController(ColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        public async Task<ActionResult> GetcolorsAsync()
        {
            var colors = await _colorService.GetColorsAsync();
            return Ok(colors);
        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetcolorByIdAsync([FromRoute][Required] Guid id)
        {
            var color = await _colorService.GetColorByIdAsync(id);
            return Ok(color);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreatecolorAsync([FromForm][Required] CreateColorDTO request)
        {
            var color = await _colorService.CreateColorAsync(request);
            return Ok(color);
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdatecolorAsync([Required] Guid id, [FromForm] UpdateColorDTO request)
        {
            var color = await _colorService.UpdateColorAsync(id, request);
            return Ok(color);
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeletecolorAsync([Required] Guid id)
        {
            await _colorService.DeleteColorAsync(id);
            return NoContent();
        }

    }
}
