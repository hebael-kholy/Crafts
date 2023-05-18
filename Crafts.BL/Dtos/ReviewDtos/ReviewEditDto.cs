using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.ReviewDtos
{
    public class ReviewEditDto
    {
        public string Content { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
