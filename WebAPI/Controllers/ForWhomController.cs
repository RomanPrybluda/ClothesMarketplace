using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    [ApiController]
    [Produces("application/json")]
    [Route("forWhom")]

    public class ForWhomController : ControllerBase
    {
        private readonly ForWhomService _forWhomService;

        public ForWhomController(ForWhomService forWhomService)
        {
            _forWhomService = forWhomService;
        }

        [HttpGet]
        public async Task<ActionResult> GetForWhomsAsync()
        {
            var forWhoms = await _forWhomService.GetForWhomsAsync();
            return Ok(forWhoms);
        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetForWhomByIdAsync([FromRoute][Required] Guid id)
        {
            var forWhom = await _forWhomService.GetForWhomByIdAsync(id);
            return Ok(forWhom);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateForWhomAsync([FromBody][Required] CreateForWhomDTO request)
        {
            var forWhom = await _forWhomService.CreateForWhomAsync(request);
            return Ok(forWhom);
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateForWhomAsync([FromRoute][Required] Guid id, [FromBody] UpdateForWhomDTO request)
        {
            var forWhom = await _forWhomService.UpdateForWhomAsync(id, request);
            return Ok(forWhom);
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteForWhomAsync([FromRoute][Required] Guid id)
        {
            await _forWhomService.DeleteForWhomAsync(id);
            return NoContent();
        }
    }
}
