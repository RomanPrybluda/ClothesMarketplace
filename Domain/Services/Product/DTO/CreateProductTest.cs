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
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal DollarPrice { get; set; }

        public List<IFormFile> Images { get; set; }
        
        public int MainImageIndex { get; set; } = 0;

        public Guid BrandId { get; set; }

        public Guid ColorId { get; set; }

        public Guid? ProductSizeId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ForWhomId { get; set; }

        public Guid ProductConditionId { get; set; }
    }
}
