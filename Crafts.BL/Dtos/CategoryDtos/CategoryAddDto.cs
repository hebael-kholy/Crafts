using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.CategoryDtos
{
    public class CategoryAddDto
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile Image { get; set; } = null;
    }
}
