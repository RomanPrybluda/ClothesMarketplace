using DAL;

namespace Domain
{
    public class UpdateConditionDTO
    {
        public string Name { get; set; } = string.Empty;

        public void UpdateCondition(ProductCondition condition)
        {
            condition.Name = Name;
        }
    }
}
