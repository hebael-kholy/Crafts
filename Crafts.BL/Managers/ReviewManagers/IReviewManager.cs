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

        List<ReviewReadDto> GetReviewsByProductId(int id);
        Task Add(ReviewAddDto review);
        ReviewReadDto GetById(int id);
        void Edit(ReviewEditDto reviewEditDto, int id);
        void Delete(int id);
    }
}
