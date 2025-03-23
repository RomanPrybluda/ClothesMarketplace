using DAL;

namespace Domain
{
    public class UpdateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;

        public void UpdateCategory(Category category)
        {
            category.Name = Name ?? category.Name;
        }

    }
}
