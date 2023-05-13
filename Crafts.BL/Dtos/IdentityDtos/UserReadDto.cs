using Crafts.DAL.Models.Enum;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.IdentityDtos;

public class UserReadDto : IdentityUser
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Gender Gender { get; set; }
    public string Image { get; set; } 

}
