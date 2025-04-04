using Microsoft.AspNetCore.Identity;

namespace DAL
{
    public class AppUser : IdentityUser
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? Age { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public List<Product> PurchasedProducts { get; set; } = new();

        public List<Product> SoldProducts { get; set; } = new();
    }
}
