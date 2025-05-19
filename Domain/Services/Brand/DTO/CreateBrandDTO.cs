using DAL;
using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class CreateBrandDTO
    {
        public string Name { get; set; } = string.Empty;

        public IFormFile Image { get; set; }

        public static Brand ToBrand(CreateBrandDTO request)
        {
            return new Brand
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
            };
        }
    }
}