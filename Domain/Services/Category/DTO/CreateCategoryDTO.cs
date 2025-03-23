using DAL;

namespace Domain
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;

        public static Category ToCategory(CreateCategoryDTO request)
        {
            return new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
            };
        }
    }
}
