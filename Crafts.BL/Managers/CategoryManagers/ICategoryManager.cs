using Crafts.BL.Dtos.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.CategoryManagers
{
    public interface ICategoryManager
    {
        List<CategoryReadDto> GetAll();
        void AddImage(CategoryImgAddDto category, int id);
        Task Add(CategoryAddDto category);
    }
}
