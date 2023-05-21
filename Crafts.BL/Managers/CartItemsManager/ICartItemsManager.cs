using Crafts.BL.Dtos.CartItemsDtos;
using Crafts.BL.Dtos.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.CartItemsManager
{
    public interface ICartItemsManager
    {
        List<CartItemReadDto> GetAll();
        CartItemReadDto GetById(int id);
        Task Add(CartItemAddDto cartItem);
        void Edit(CartItemEditDto cartItemEditDto, int id);
        void Delete(int id);
        void DeleteAllCartItemsByCartId(int cartId);

    }
}
