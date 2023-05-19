using Crafts.BL.Dtos.ProductDtos;
using Crafts.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.CategoryDtos
{
    public class CategoryWithProductsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public List<ProductReadDto> Products { get; set; } = new();
    }
}
