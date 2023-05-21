using Crafts.BL.Dtos.CartDtos;
using Crafts.BL.Dtos.ReviewDtos;
using Crafts.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.CartManager
{
    public interface ICartManager
    {
        List<CartReadDto> GetAll();
        CartReadDto GetById(int id);
        CartWithCartItemsReadDto GetByIdWithCartItems(int id);
        Task Add(CartAddDto cart);
        void ApplyCouponForDiscount(int cartId, int couponId);
        void Delete(int id);

    }
}
