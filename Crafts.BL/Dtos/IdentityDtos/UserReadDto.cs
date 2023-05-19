using Crafts.DAL.Models.Enum;
using Microsoft.AspNetCore.Identity;

namespace Crafts.BL.Dtos.IdentityDtos;

public class UserReadDto : IdentityUser
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Gender Gender { get; set; }
    public string Image { get; set; }

}
