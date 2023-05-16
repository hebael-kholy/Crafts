using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.ProductDtos
{
    public class ProductUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public float Rating { get; set; }
        public int Quantity { get; set; }
        public bool IsSale { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
