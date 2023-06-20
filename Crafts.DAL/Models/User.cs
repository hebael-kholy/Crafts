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
    public string Image { get; set; } = "https://www.kindpng.com/picc/m/24-248253_user-profile-default-image-png-clipart-png-download.png";
    public Role Role { get; set; }
    public bool? Flag { get; set; }=false;
    public DateTime? ExpirationDate { get; set; } 
    public string? HashCode { get; set; }
    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

}
