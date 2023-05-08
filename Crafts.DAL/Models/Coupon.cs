using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Models;

public class Coupon
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ExpireDate { get; set; }
    public double Discount { get; set; }

    public ICollection<Cart> Carts { get; set; } = new HashSet<Cart>();
}
