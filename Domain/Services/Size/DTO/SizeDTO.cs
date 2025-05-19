using DAL;

namespace Domain
{
    public class SizeDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public static SizeDTO FromProductSize(ProductSize size)
        {
            return new SizeDTO
            {
                Id = size.Id,
                Name = size.Value
            };
        }
    }
}
