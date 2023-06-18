using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Models;

public class Review
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
}
