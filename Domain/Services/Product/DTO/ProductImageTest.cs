using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Product.DTO
{
    public class ProductImageTest
    {
        public IFormFile ImageUrl { get; set; }

        public bool IsMain { get; set; }
    }
}
