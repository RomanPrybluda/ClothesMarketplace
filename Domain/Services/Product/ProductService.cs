using DAL;
using Domain.Abstractions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ProductService
    {
        private readonly ClothesMarketplaceDbContext _context;
        private readonly IValidator<CreateProductDTO> _createProductValidator;
        private readonly IImageService _imageService;

        public ProductService(
            ClothesMarketplaceDbContext context,
            IImageService imageService,
            IValidator<CreateProductDTO> createProductValidator)
        {
            _context = context;
            _imageService = imageService;
            _createProductValidator = createProductValidator;
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
            var productById = await _context.Products.FindAsync(id);
            if (productById == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Product not found with ID {id}");

            var productDTO = ProductByIdDTO.FromProduct(productById);
            return productDTO;
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO request)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == request.Name);
            if (existingProduct != null)
                throw new CustomException(CustomExceptionType.ProductAlreadyExists, $"Product with name {request.Name} already exists.");

            var validationResult = await _createProductValidator.ValidateAsync(request);

            var product = CreateProductDTO.ToProduct(request);

            var imagesUrls = await _imageService.UploadMultipleImagesAsync(request.Images);
            AttachProductImages(product, imagesUrls, request.MainImageIndex);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var productDTO = ProductDTO.FromProduct(product);

            return productDTO;
        }

        public async Task<ProductDTO> UpdateProductAsync(Guid id, UpdateProductDTO request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Product with ID {id} not found.");

            var brand = await _context.Brands.FindAsync(request.BrandId);
            if (brand == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Brand not found with ID {request.BrandId}");

            var color = await _context.Colors.FindAsync(request.ColorId);
            if (color == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Color not found with ID {request.ColorId}");

            var productSize = await _context.ProductSizes.FindAsync(request.ProductSizeId);
            if (productSize == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Product size not found with ID {request.ProductSizeId}");

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Category not found with ID {request.CategoryId}");

            var forWhom = await _context.ForWhoms.FindAsync(request.ForWhomId);
            if (forWhom == null)
                throw new CustomException(CustomExceptionType.NotFound, $"For whom not found with ID {request.ForWhomId}");

            var productCondition = await _context.ProductConditions.FindAsync(request.ProductConditionId);
            if (productCondition == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Product condition not found with ID {request.ProductConditionId}");


            ValidateImages(request.Images.Select(img => img.ImageUrl));

            request.UpdateProduct(product);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return ProductDTO.FromProduct(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Product with ID {id} not found.");

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

        private void AttachProductImages(Product product, List<string> imagesUrls, int mainImageIndex)
        {
            for (int i = 0; i < imagesUrls.Count; i++)
            {
                if (i == mainImageIndex)
                    product.Images.Add(new Image { ImageUrl = imagesUrls[i], IsMain = true });
                else
                    product.Images.Add(new Image { ImageUrl = imagesUrls[i], IsMain = false });
            }
        }
    }
}

