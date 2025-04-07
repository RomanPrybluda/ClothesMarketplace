using AutoMapper;
using Azure.Core;
using DAL;
using Domain.Abstractions;
using Domain.Services.Product.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ProductService
    {
        private readonly ClothesMarketplaceDbContext _context;
        private readonly IValidator<CreateProductTest> _createProductValidator;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ProductService(ClothesMarketplaceDbContext context,
            IValidator<CreateProductTest> createProductValidator,
            IMapper mapper, IImageService imageService)
        {
            _context = context;
            _createProductValidator = createProductValidator;
            _mapper = mapper;
            _imageService = imageService;
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

        public async Task<Product> CreateProductAsync(CreateProductTest request)
        {
            var validationResult = await _createProductValidator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var product = _mapper.Map<Product>(request);
                var imagesUrls = await _imageService.UploadMultipleImagesAsync(request.Images);
                AttachProductImages(product, imagesUrls, request.MainImageIndex);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return product;
            }

            // TODO: Temporary stub – returns deafult value for product if it is invalid
            return default;
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

        private void AttachProductImages(Product product, List<string> imagesUrls, int mainImageIndex)
        {
            for (int i = 0; i < imagesUrls.Count; i++)
            {
                if (i == mainImageIndex)
                {
                    product.Images.Add(new ProductImage { ImageUrl = imagesUrls[i], IsMain = true });
                }
                product.Images.Add(new ProductImage { ImageUrl = imagesUrls[i], IsMain = false });
            }
        }
    }
}

