using Microsoft.EntityFrameworkCore;

namespace Crafts.DAL.Models;

[Index(nameof(Category.Title), IsUnique = true)]
public class Category
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}
