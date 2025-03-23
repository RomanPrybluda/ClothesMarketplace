using DAL;

namespace Domain
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public static CategoryDTO FromCategory(Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
            };
        }
    }
}
