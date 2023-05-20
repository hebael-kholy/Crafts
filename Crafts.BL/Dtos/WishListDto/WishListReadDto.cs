using Crafts.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.WishListDto
{
    public class WishListReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();


    }
}
