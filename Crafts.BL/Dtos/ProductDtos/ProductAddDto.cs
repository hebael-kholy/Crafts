using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.ProductDtos
{
    public class ProductAddDto
    {
        public string Title { get; set; } = string.Empty;
        public  double Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; } = null!;

    }
}
