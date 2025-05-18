using DAL;

namespace Domain
{
    public class CreateConditionDTO
    {
        public string Name { get; set; } = string.Empty;

        public static ProductCondition ToCondition(CreateConditionDTO request)
        {
            return new ProductCondition
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };
        }
    }
}
