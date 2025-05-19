using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ConditionService
    {
        private readonly ClothesMarketplaceDbContext _context;

        public ConditionService(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConditionDTO>> GetConditionsAsync()
        {
            var conditions = await _context.ProductConditions.ToListAsync();
            if (!conditions.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No entries found for 'Condition'");

            List<ConditionDTO> conditionsDTOs = new();
            foreach (var condition in conditions)
            {
                var conditionDTO = ConditionDTO.FromCondition(condition);
                conditionsDTOs.Add(conditionDTO);
            }

            return conditionsDTOs;
        }

        public async Task<ConditionDTO> GetConditionByIdAsync(Guid id)
        {
            var condition = await _context.ProductConditions.FindAsync(id);
            if (condition == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No entry found with ID {id}");

            var conditionDTO = ConditionDTO.FromCondition(condition);

            return conditionDTO;
        }

        public async Task<ConditionDTO> CreateConditionAsync(CreateConditionDTO request)
        {
            var existingCondition = await _context.ProductConditions
                .AnyAsync(x => x.Name.ToLower() == request.Name.ToLower());

            if (existingCondition)
                throw new CustomException(CustomExceptionType.IsAlreadyExists, $"Entry '{request.Name}' already exists.");

            var condition = CreateConditionDTO.ToCondition(request);
            _context.ProductConditions.Add(condition);
            await _context.SaveChangesAsync();

            var createdCondition = await _context.ProductConditions.FindAsync(condition.Id);
            var conditionDTO = ConditionDTO.FromCondition(createdCondition);

            return conditionDTO;
        }

        public async Task<ConditionDTO> UpdateConditionAsync(Guid id, UpdateConditionDTO request)
        {
            var existingCondition = await _context.ProductConditions.FindAsync(id);

            if (existingCondition == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No entry found with ID {id}");

            existingCondition.Name = request.Name;
            _context.ProductConditions.Update(existingCondition);
            await _context.SaveChangesAsync();

            var updatedCondition = await _context.ProductConditions.FindAsync(existingCondition.Id);
            var conditionDTO = ConditionDTO.FromCondition(updatedCondition);

            return conditionDTO;
        }

        public async Task DeleteConditionAsync(Guid id)
        {
            var entity = await _context.ProductConditions.FindAsync(id);
            if (entity == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No entry found with ID {id}");

            _context.ProductConditions.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
