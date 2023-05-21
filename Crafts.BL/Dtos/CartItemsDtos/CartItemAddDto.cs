using Crafts.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.CartItemsDtos
{
    public class CartItemAddDto
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
    }
}
