using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.IdentityDtos;

public class UpdateUserDto
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public IFormFile? ImageFile { get; set; }
}
