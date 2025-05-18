using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ForWhomService
    {
        private readonly ClothesMarketplaceDbContext _context;

        public ForWhomService(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ForWhomDTO>> GetForWhomsAsync()
        {
            var forWhoms = await _context.ForWhoms.ToListAsync();
            if (!forWhoms.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No entries found for 'ForWhom'");

            List<ForWhomDTO> forWhomDTOs = new();
            foreach (var forWhom in forWhoms)
            {
                var forWhomDTO = ForWhomDTO.FromForWhom(forWhom);
                forWhomDTOs.Add(forWhomDTO);
            }

            return forWhomDTOs;
        }

        public async Task<ForWhomDTO> GetForWhomByIdAsync(Guid id)
        {
            var forWhom = await _context.ForWhoms.FindAsync(id);
            if (forWhom == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No entry found with ID {id}");

            var forWhomDTO = ForWhomDTO.FromForWhom(forWhom);

            return forWhomDTO;
        }

        public async Task<ForWhomDTO> CreateForWhomAsync(CreateForWhomDTO request)
        {
            var existingForWhom = await _context.ForWhoms
                .AnyAsync(x => x.Name.ToLower() == request.Name.ToLower());

            if (existingForWhom)
                throw new CustomException(CustomExceptionType.IsAlreadyExists, $"Entry '{request.Name}' already exists.");

            var forWhom = CreateForWhomDTO.ToForWhom(request);
            _context.ForWhoms.Add(forWhom);
            await _context.SaveChangesAsync();

            var createdForWhom = await _context.ForWhoms.FindAsync(forWhom.Id);
            var forWhomDTO = ForWhomDTO.FromForWhom(createdForWhom);

            return forWhomDTO;
        }

        public async Task<ForWhomDTO> UpdateForWhomAsync(Guid id, UpdateForWhomDTO request)
        {
            var existingForWhom = await _context.ForWhoms.FindAsync(id);
            if (existingForWhom == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No entry found with ID {id}");

            existingForWhom.Name = request.Name;
            _context.ForWhoms.Update(existingForWhom);
            await _context.SaveChangesAsync();

            var updatedForWhom = await _context.ForWhoms.FindAsync(existingForWhom.Id);
            var forWhomDTO = ForWhomDTO.FromForWhom(updatedForWhom);

            return forWhomDTO;
        }

        public async Task DeleteForWhomAsync(Guid id)
        {
            var entity = await _context.ForWhoms.FindAsync(id);
            if (entity == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No entry found with ID {id}");

            _context.ForWhoms.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
