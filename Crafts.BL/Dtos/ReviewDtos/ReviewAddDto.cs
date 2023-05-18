using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Dtos.ReviewDtos
{
    public class ReviewAddDto
    {
        public string Content { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
