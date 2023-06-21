using Crafts.DAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace Crafts.DAL.Models;

public class User : IdentityUser
{
    public Gender Gender { get; set; }
    public string Image { get; set; } = "https://cdn-icons-png.flaticon.com/512/149/149071.png?w=740&t=st=1687275130~exp=1687275730~hmac=d2979afa10c27e29575a144f7509cd825d65a0b9d6c838617c9c3ddb768361b6";
    public Role Role { get; set; }
    public bool? Flag { get; set; }=false;
    public DateTime? ExpirationDate { get; set; } 
    public string? HashCode { get; set; }
    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

}
