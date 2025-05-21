using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class SizeService
    {
        private readonly ClothesMarketplaceDbContext _context;

        public SizeService(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SizeDTO>> GetSizesAsync()
        {
            var sizes = await _context.ProductSizes.ToListAsync();

            if (!sizes.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No sizes found.");

            List<SizeDTO> sizeDTOs = new();
            foreach (var size in sizes)
            {
                var colorDTO = SizeDTO.FromProductSize(size);
                sizeDTOs.Add(colorDTO);
            }

            return sizeDTOs;
        }

        public async Task<SizeDTO> GetSizeByIdAsync(Guid id)
        {
            var size = await _context.ProductSizes.FindAsync(id);

            if (size == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Size with ID {id} not found.");

            var sizeDTO = SizeDTO.FromProductSize(size);

            return sizeDTO;
        }

        public async Task<SizeDTO> CreateSizeAsync(CreateSizeDTO request)
        {
            var existingSize = await _context.ProductSizes.AnyAsync(s => s.Name.ToLower() == request.Name.ToLower());

            if (existingSize)
                throw new CustomException(CustomExceptionType.IsAlreadyExists, $"Size '{request.Name}' already exists.");

            var size = CreateSizeDTO.ToEntity(request);

            _context.ProductSizes.Add(size);
            await _context.SaveChangesAsync();

            var createdSize = await _context.ProductSizes.FindAsync(size.Id);
            var sizeDTO = SizeDTO.FromProductSize(createdSize);

            return sizeDTO;
        }

        public async Task<SizeDTO> UpdateSizeAsync(Guid id, UpdateSizeDTO request)
        {
            var existingSize = await _context.ProductSizes.FindAsync(id);

            if (existingSize == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Size with ID {id} not found.");

            existingSize.Name = request.Name;
            _context.ProductSizes.Update(existingSize);
            await _context.SaveChangesAsync();

            var updatedSize = await _context.ProductSizes.FindAsync(existingSize.Id);
            var sizeDTO = SizeDTO.FromProductSize(updatedSize);

            return sizeDTO;
        }

        public async Task DeleteSizeAsync(Guid id)
        {
            var size = await _context.ProductSizes.FindAsync(id);

            if (size == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Size with ID {id} not found.");

            _context.ProductSizes.Remove(size);
            await _context.SaveChangesAsync();
        }
    }
}
