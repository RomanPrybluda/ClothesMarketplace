using DAL;

namespace Domain
{
    public class ColorDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public static ColorDTO FromColor(Color color)
        {
            return new ColorDTO
            {
                Id = color.Id,
                Name = color.Name
            };
        }
    }
}
