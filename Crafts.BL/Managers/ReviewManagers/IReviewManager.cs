using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Dtos.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.ReviewManagers
{
    public interface IReviewManager
    {
        List<ReviewReadDto> GetAll();
        Task Add(ReviewAddDto review, int productId, int userId);
        ReviewReadDto GetById(int id);
    }
}
