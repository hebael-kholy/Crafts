using Crafts.DAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.IdentityDtos;

public record RegisterDto(string UserName, string Email, string Password, Gender Gender);
//public class RegisterDto
//{
//    public string UserName { get; set; }
//    public string Email { get; set; }   = string.Empty;
//    public string Password { get; set; }
//    public Gender Gender { get; set; }
//}
//public enum Gender
//{
//    Male, Female
//}