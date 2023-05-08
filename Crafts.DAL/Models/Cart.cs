using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Models;

public class Cart
{
    public int Id { get; set; }
    public double TotalPrice { get; set; }
    public double TotalPriceAfterDiscount { get; set; }
    public int Quantity { get; set; }

    public ICollection<CartItem> CartItems { get; set;} = new HashSet<CartItem>();
    public ICollection<Coupon> Coupons { get; set;} = new HashSet<Coupon>();

    public int UserId { get; set; }
    public User? User { get; set; }
}
