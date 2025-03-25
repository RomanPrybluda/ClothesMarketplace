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
                    new DeliveryMethod { Id = Guid.NewGuid(), Name = "Courier Delivery" },
                    new DeliveryMethod { Id = Guid.NewGuid(), Name = "Self Pickup" },
                    new DeliveryMethod { Id = Guid.NewGuid(), Name = "Postal Service" },
                    new DeliveryMethod { Id = Guid.NewGuid(), Name = "Express Delivery" },
                    new DeliveryMethod { Id = Guid.NewGuid(), Name = "International Shipping" }
                };

                _context.DeliveryMethods.AddRange(deliveryMethods);
                _context.SaveChanges();
            }
        }
    }
}
