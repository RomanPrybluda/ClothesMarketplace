using DAL;

namespace Domain
{
    public class CreateColorDTO
    {
        public string Name { get; set; } = string.Empty;

        public static Color ToColor(CreateColorDTO request)
        {
            return new Color
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };
        }
    }
}
