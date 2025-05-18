using DAL;

namespace Domain
{
    public class ConditionDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public static ConditionDTO FromCondition(ProductCondition сondition)
        {
            return new ConditionDTO
            {
                Id = сondition.Id,
                Name = сondition.Name
            };
        }
    }
}
