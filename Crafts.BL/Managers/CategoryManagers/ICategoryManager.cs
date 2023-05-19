using Crafts.BL.Dtos.CategoryDtos;
using Crafts.DAL.Models;
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
        CategoryWithProductsDto GetByIdWithProducts(int id);
        void AddImage(CategoryImgAddDto category, int id);
        Task Add(CategoryAddDto category);
        CategoryReadDto GetById(int id);
        void Edit(CategoryEditDto categoryEditDto, int id);
        void Delete(int id);
    }
}
