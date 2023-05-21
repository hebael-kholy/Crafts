using Crafts.BL.Dtos.CartItemsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.CartDtos
{
    public class CartWithCartItemsReadDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceAfterDiscount { get; set; }
        public List<CartItemsChildReadDto>? CartItems { get; set; } = new ();
    }
}
