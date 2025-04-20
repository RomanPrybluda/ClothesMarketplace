using DAL;

namespace Domain
{
    public class DeliveryMethodInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;

        public DeliveryMethodInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeDeliveryMethods()
        {
            if (!_context.DeliveryMethods.Any())
            {
                var deliveryMethods = new List<DeliveryMethod>
                {
                    new DeliveryMethod { Id = Guid.Parse("A8A1D240-946D-4AF9-84C4-61F4B47461A9"), Name = "Courier Delivery" },
                    new DeliveryMethod { Id = Guid.Parse("B2C5912B-BEF0-4F01-A960-1A2E515BE59D"), Name = "Self Pickup" },
                    new DeliveryMethod { Id = Guid.Parse("50B9A6C0-6901-4B86-A6ED-0A8A7A82D58D"), Name = "Postal Service" },
                    new DeliveryMethod { Id = Guid.Parse("2F2DB688-2E71-4E91-A3E6-E845385B1723"), Name = "Express Delivery" },
                    new DeliveryMethod { Id = Guid.Parse("65187959-E9A1-4F6A-A746-14B7C0FBCBC3"), Name = "International Shipping" }
                };

                _context.DeliveryMethods.AddRange(deliveryMethods);
                _context.SaveChanges();
            }
        }
    }
}
