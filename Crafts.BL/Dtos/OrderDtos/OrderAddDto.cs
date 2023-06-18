using Crafts.DAL.Models.Enum;
using Crafts.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.OrderDtos;

public class OrderAddDto
{
    public string UserId { get; set; } = string.Empty;
    public int CartId { get; set; }
}
