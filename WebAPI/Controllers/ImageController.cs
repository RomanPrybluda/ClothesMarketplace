using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("images")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
                var result = await _imageService.UploadImageAsync(file);
                return Ok(new { FilePath = result });
        }
    }
}
