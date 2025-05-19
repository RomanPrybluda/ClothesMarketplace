using DAL;

namespace Domain
{
    public class CreateSizeDTO
    {
        public string Name { get; set; } = string.Empty;

        public static ProductSize ToEntity(CreateSizeDTO dto)
        {
            return new ProductSize
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };
        }
    }
}
