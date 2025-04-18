using Microsoft.AspNetCore.Identity;

namespace DAL
{
    public class AppUser : IdentityUser
    {

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? Age { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public ICollection<Product> SoldProducts { get; set; } = new List<Product>();

        public ICollection<Product> PurchasedProducts { get; set; } = new List<Product>();

        public ICollection<Product> FavoriteProducts { get; set; } = new List<Product>();
    }
}
