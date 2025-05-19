using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("conditions")]
    public class ConditionController : ControllerBase
    {
        private readonly ConditionService _conditionService;

        public ConditionController(ConditionService conditionService)
        {
            _conditionService = conditionService;
        }

        [HttpGet]
        public async Task<ActionResult> GetConditionsAsync()
        {
            var conditions = await _conditionService.GetConditionsAsync();
            return Ok(conditions);
        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetConditionByIdAsync([FromRoute][Required] Guid id)
        {
            var condition = await _conditionService.GetConditionByIdAsync(id);
            return Ok(condition);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateConditionAsync([FromForm][Required] CreateConditionDTO request)
        {
            var condition = await _conditionService.CreateConditionAsync(request);
            return Ok(condition);
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateConditionAsync([Required] Guid id, [FromForm] UpdateConditionDTO request)
        {
            var condition = await _conditionService.UpdateConditionAsync(id, request);
            return Ok(condition);
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteConditionAsync([Required] Guid id)
        {
            await _conditionService.DeleteConditionAsync(id);
            return NoContent();
        }
    }
}
