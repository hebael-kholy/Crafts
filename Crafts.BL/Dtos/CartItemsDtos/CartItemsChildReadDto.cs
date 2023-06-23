using Crafts.BL.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.CartItemsDtos
{
    public class CartItemsChildReadDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }


    }
}
