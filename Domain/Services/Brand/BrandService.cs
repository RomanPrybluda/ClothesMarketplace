using DAL;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Brand
{
    public class BrandService
    {
        private readonly ClothesMarketplaceDbContext _context;
        private readonly IImageService _imageService;

        public BrandService(ClothesMarketplaceDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<IEnumerable<BrandDTO>> GetBrandsAsync()
        {
            var brands = await _context.Brands.ToListAsync();
            if (!brands.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No brands found");

            List<BrandDTO> brandsResponse = new();
            foreach (var brand in brands)
            {
                var brandDTO = BrandDTO.FromBrand(brand);
                brandsResponse.Add(brandDTO);
            }

            return brandsResponse;
        }

        public async Task<BrandDTO> GetBrandByIdAsync(Guid id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand is null)
                throw new CustomException(CustomExceptionType.NotFound, $"No brand found with ID {id}");

            var brandDTO = BrandDTO.FromBrand(brand);

            return brandDTO;
        }

        public async Task<BrandDTO> CreateBrandAsync(CreateBrandDTO request)
        {
            var existingBrand = await _context.Brands.FirstOrDefaultAsync(
                b => b.Name.ToLower() == request.Name.ToLower());

            if (existingBrand != null)
                throw new CustomException(CustomExceptionType.IsAlreadyExists, $"Brand '{request.Name}' already exists.");

            var brand = CreateBrandDTO.ToBrand(request);
            brand.ImageName = await _imageService.UploadImageAsync(request.Image);
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            var createdBrand = await _context.Brands.FindAsync(brand.Id);
            return BrandDTO.FromBrand(createdBrand);
        }

        public async Task<BrandDTO> UpdateBrandAsync(Guid Id, UpdateBrandDTO request)
        {
            var existingBrand = await _context.Brands.FindAsync(Id);

            if (existingBrand is null)
                throw new CustomException(CustomExceptionType.NotFound, $"No brand found with ID {Id}");
            if (request.Image != null)
                existingBrand.ImageName = await _imageService.UploadImageAsync(request.Image);

            existingBrand.Name = request.Name;

            _context.Update(existingBrand);
            await _context.SaveChangesAsync();

            var updatedBrand = await _context.Brands.FindAsync(existingBrand.Id);
            return BrandDTO.FromBrand(updatedBrand);
        }

        public async Task DeleteBrandAsync(Guid Id)
        {
            var brand = await _context.Brands.FindAsync(Id);

            if (brand is null)
                throw new CustomException(CustomExceptionType.NotFound, $"No brand found with ID {Id}");

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
        }
    }
}
