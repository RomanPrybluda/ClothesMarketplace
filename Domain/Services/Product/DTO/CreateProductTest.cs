using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Product.DTO
{
    public class CreateProductTest
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal DollarPrice { get; set; }

        public ICollection<IFormFile> Images { get; set; }
        
        public int MainFileIndex { get; set; } = 0;

        public Guid BrandId { get; set; } = Guid.Empty;

        public Guid ColorId { get; set; } = Guid.Empty;

        public Guid? ProductSizeId { get; set; } = Guid.Empty;

        public Guid CategoryId { get; set; } = Guid.Empty;

        public Guid ForWhomId { get; set; } = Guid.Empty;

        public Guid ProductConditionId { get; set; } = Guid.Empty;
    }
}
