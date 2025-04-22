using DAL;

namespace Domain
{
    public class BrandDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string ImageName { get; init; } = string.Empty;

        public static BrandDTO FromBrand(Brand brand)
        {
            return new BrandDTO
            {
                Id = brand.Id,
                Name = brand.Name,
                ImageName = brand.ImageName,
            };
        }
    }
}