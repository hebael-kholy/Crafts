using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Models;

public class Wishlist
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public string UserId { get; set; }
    public User? User { get; set; }

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();

}
