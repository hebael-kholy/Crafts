using Crafts.BL.Dtos.CouponDtos;
using Crafts.BL.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.ProductManager
{
    public interface IProductsManager
    {
        Task Add(ProductAddDto productDto);
        List<ProductReadDto> GetAll();
        ProductReadDto? GetById(int id);
        void AddImage(ProductImgAddDto product, int id);

    }
}
