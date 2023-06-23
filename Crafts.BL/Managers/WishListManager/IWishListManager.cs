using Crafts.BL.Dtos.ProductDtos;
using Crafts.BL.Dtos.WishListDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.WishListManager
{
    public interface IWishListManager
    {
        WishListReadDto GetUserWishList(string userId);
        List<WishListReadDto> GetAll();
        Task Add(WishListAddDto wishList);
        void AddProductToWishlistAsync(int wishListId, ProductToAddToWishList prod);
        void DeleteProductFromWishListAsync(int productId , int wishlistId);
        WishlistWithProductsDto GetByIdWithProducts(int id);
        WishListReadDto GetById(int id);
        void Delete(int id);
    }
}
