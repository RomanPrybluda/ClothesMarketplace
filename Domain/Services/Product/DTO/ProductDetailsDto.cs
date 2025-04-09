using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Product.DTO
{
    public record ProductDetailsDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal DollarPrice { get; init; }
        public int LikesCount { get; init; }
        public List<ImageDto> Images { get; init; }
        public string Brand { get; init; }
        public string Color { get; init; }
        public string ProductSize { get; init; }
        public string Category { get; init; }
        public string ForWhom { get; init; }
        public string ProductCondition { get; init; }
        public string Details { get; init; }
        public string Seller { get; init; }
        public string Buyer { get; init; }
    }
}
