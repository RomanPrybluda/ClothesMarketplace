using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{

    public class FavoriteProductService
    {
        private readonly ClothesMarketplaceDbContext _context;

        public FavoriteProductService(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddToFavoritesAsync(Guid userId, Guid productId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteProducts)
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());
            if (user == null)
                throw new CustomException(CustomExceptionType.NotFound, $"User with ID {productId} not found.");

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Product with ID {productId} not found.");

            if (!user.FavoriteProducts.Contains(product))
            {
                user.FavoriteProducts.Add(product);
                await _context.SaveChangesAsync();
            }

            var favoriteProduct = product.Id;

            return favoriteProduct;
        }

        public async Task RemoveFromFavoritesAsync(Guid userId, Guid productId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteProducts)
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());
            if (user == null)
                throw new CustomException(CustomExceptionType.NotFound, $"User with ID {userId} not found.");

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Product with ID {productId} not found.");

            if (user.FavoriteProducts.Contains(product))
            {
                user.FavoriteProducts.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Guid>> GetFavoriteProductsAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteProducts)
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());
            if (user == null)
                throw new CustomException(CustomExceptionType.NotFound, $"User with ID {userId} not found.");

            var favoriteProducts = user.FavoriteProducts.Select(p => p.Id).ToList();

            return favoriteProducts;
        }
    }
}
