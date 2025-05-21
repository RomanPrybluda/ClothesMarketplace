using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]

    public class ImageCleanupController : ControllerBase
    {
        private readonly ImageCleanupService _imageCleanupService;

        public ImageCleanupController(ImageCleanupService imageCleanupService)
        {
            _imageCleanupService = imageCleanupService;
        }

        [HttpPost("cleanup")]
        public async Task<IActionResult> TriggerCleanup()
        {
            try
            {
                await _imageCleanupService.CleanupImages();
                return Ok("Image cleaning completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while executing cleanup: {ex.Message}");
            }
        }
    }
}
