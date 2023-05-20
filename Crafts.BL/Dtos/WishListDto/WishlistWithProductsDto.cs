using Crafts.BL.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.WishListDto
{
    public class WishlistWithProductsDto
    {
        public int Id { get; set; }
        public string userId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ProductReadDto> Products { get; set; } = new();


    }
}
