using DAL;
using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class UpdateBrandDTO
    {
        public string Name { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }
}