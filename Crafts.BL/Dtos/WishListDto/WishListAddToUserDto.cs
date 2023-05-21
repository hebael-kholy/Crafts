using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.WishListDto
{
    public class WishListAddToUserDto
    {
        public int wishlistId { get; set; }
        public int UserId { get; set; }
    }
}
