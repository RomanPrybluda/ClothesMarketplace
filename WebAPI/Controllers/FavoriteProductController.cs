using Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{

    [ApiController]
    [Produces("application/json")]
    [Route("favorite-products")]

    public class FavoriteProductController : ControllerBase
    {
        private readonly FavoriteProductService _favoriteProductService;

        public FavoriteProductController(FavoriteProductService favoriteProductService)
        {
            _favoriteProductService = favoriteProductService;
        }

        [HttpPost]
        public async Task<ActionResult> AddToFavorites([FromForm][Required] Guid userId, [FromForm][Required] Guid productId)
        {
            var favoriteProduct = await _favoriteProductService.AddToFavoritesAsync(userId, productId);
            return Ok(favoriteProduct);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFromFavorites([FromForm][Required] Guid userId, [FromForm][Required] Guid productId)
        {
            await _favoriteProductService.RemoveFromFavoritesAsync(userId, productId);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> GetFavorites([FromQuery][Required] Guid userId)
        {
            var favoriteProducts = await _favoriteProductService.GetFavoriteProductsAsync(userId);
            return Ok(favoriteProducts);
        }
    }
}
