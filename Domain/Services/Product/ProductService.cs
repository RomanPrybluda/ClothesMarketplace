using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ProductService
    {

        private readonly ClothesMarketplaceDbContext _context;

        public ProductService(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsListAsync()
        {
            var products = await _context.Products.ToListAsync();

            if (products == null || !products.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No Products");

            return products.Select(ProductDTO.FromProduct).ToList();
        }

        public async Task<ProductByIdDTO> GetProductByIdAsync(Guid id)
        {
            var productById = await _context.Products.FindAsync(id)
                ?? throw new CustomException(CustomExceptionType.NotFound, $"No Product with ID {id}");

            return ProductByIdDTO.FromProduct(productById);
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO request)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == request.Name);
            if (existingProduct != null)
                throw new CustomException(CustomExceptionType.ProductAlreadyExists, $"Product with Name {request.Name} already exists.");

            var category = await _context.Categories.FindAsync(request.CategoryId)
                ?? throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {request.CategoryId}");

            ValidateImages(request.Images.Select(img => img.ImageUrl));


            var product = CreateProductDTO.ToProduct(request);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return ProductDTO.FromProduct(product);
        }

        public async Task<ProductDTO> UpdateProductAsync(Guid id, UpdateProductDTO request)
        {
            var product = await _context.Products.FindAsync(id)
                ?? throw new CustomException(CustomExceptionType.NotFound, $"Product with ID {id} not found.");

            var category = await _context.Categories.FindAsync(request.CategoryId)
                ?? throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {request.CategoryId}");

            ValidateImages(request.Images.Select(img => img.ImageUrl));

            request.UpdateProduct(product);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return ProductDTO.FromProduct(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id)
                ?? throw new CustomException(CustomExceptionType.NotFound, $"No Product with {id} id");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public void ValidateImages(IEnumerable<string> images)
        {
            if (images == null || !images.Any())
            {
                throw new CustomException(CustomExceptionType.NotFound, "At least one image is required.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            foreach (var image in images)
            {
                var extension = Path.GetExtension(image).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    throw new CustomException(CustomExceptionType.InvalidData, $"Invalid image format: {extension}. Allowed formats: {string.Join(", ", allowedExtensions)}");
                }
            }
        }
    }
}
