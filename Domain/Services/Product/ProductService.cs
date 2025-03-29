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

        public async Task<PagedResponseDTO<ProductDTO>> GetProductsListAsync(ProductFilterDTO filter)
        {
            var query = _context.Products.AsQueryable();

            if (filter.BrandId.HasValue)
                query = query.Where(p => p.BrandId == filter.BrandId.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            if (filter.ColorId.HasValue)
                query = query.Where(p => p.ColorId == filter.ColorId.Value);

            if (filter.ProductSizeId.HasValue)
                query = query.Where(p => p.ProductSizeId == filter.ProductSizeId.Value);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.DollarPrice >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.DollarPrice <= filter.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
                query = query.Where(p => p.Name.ToLower().Contains(filter.SearchQuery.ToLower()));

            if (filter.SortBy.HasValue)
            {
                bool isAscending = filter.SortDirection == SortDirection.Asc;

                query = filter.SortBy switch
                {
                    ProductSortBy.Price => isAscending ? query.OrderBy(p => p.DollarPrice) : query.OrderByDescending(p => p.DollarPrice),
                    ProductSortBy.Likes => isAscending ? query.OrderBy(p => p.LikesCount) : query.OrderByDescending(p => p.LikesCount),
                    _ => query
                };
            }

            int totalItems = await query.CountAsync();

            List<Product> products;

            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
            {
                int skip = (filter.PageNumber.Value - 1) * filter.PageSize.Value;
                int take = filter.PageSize.Value;

                products = await query.Skip(skip).Take(take).ToListAsync();

                return new PagedResponseDTO<ProductDTO>(
                    products.Select(ProductDTO.FromProduct),
                    totalItems,
                    skip,
                    take
                );
            }

            products = await query.ToListAsync();

            return new PagedResponseDTO<ProductDTO>(
                products.Select(ProductDTO.FromProduct),
                totalItems,
                0,
                totalItems
            );
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
