using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.ProductDtos
{
    public class ProductImgAddDto
    {
        public IFormFile? Image { get; set; } = null;
    }
}
