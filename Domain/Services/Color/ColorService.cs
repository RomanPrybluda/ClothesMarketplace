using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ColorService
    {
        private readonly ClothesMarketplaceDbContext _context;

        public ColorService(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ColorDTO>> GetColorsAsync()
        {
            var colors = await _context.Colors.ToListAsync();
            if (!colors.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No colors found");

            List<ColorDTO> colorDTOs = new();
            foreach (var color in colors)
            {
                var colorDTO = ColorDTO.FromColor(color);
                colorDTOs.Add(colorDTO);
            }

            return colorDTOs;
        }

        public async Task<ColorDTO> GetColorByIdAsync(Guid id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No color found with ID {id}");

            var colorDTO = ColorDTO.FromColor(color);

            return colorDTO;
        }

        public async Task<ColorDTO> CreateColorAsync(CreateColorDTO request)
        {
            var existingColor = await _context.Colors.FirstOrDefaultAsync(c => c.Name.ToLower() == request.Name.ToLower());

            if (existingColor != null)
                throw new CustomException(CustomExceptionType.IsAlreadyExists, $"Color '{request.Name}' already exists.");

            var color = CreateColorDTO.ToColor(request);
            _context.Colors.Add(color);
            await _context.SaveChangesAsync();

            var createdColor = await _context.Colors.FindAsync(color.Id);
            var colorDTO = ColorDTO.FromColor(createdColor);

            return colorDTO;
        }

        public async Task<ColorDTO> UpdateColorAsync(Guid id, UpdateColorDTO request)
        {
            var existingColor = await _context.Colors.FindAsync(id);

            if (existingColor == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No color found with ID {id}");

            existingColor.Name = request.Name;
            _context.Colors.Update(existingColor);
            await _context.SaveChangesAsync();

            var updatedColor = await _context.Colors.FindAsync(existingColor.Id);
            var colorDTO = ColorDTO.FromColor(updatedColor);

            return colorDTO;
        }

        public async Task DeleteColorAsync(Guid id)
        {
            var color = await _context.Colors.FindAsync(id);

            if (color == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No color found with ID {id}");

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
        }
    }
}
