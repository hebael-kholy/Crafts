using Crafts.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.CategoryDtos
{
    public class CategoryImgAddDto
    {
        public IFormFile? Image { get; set; } = null;
    }
}
