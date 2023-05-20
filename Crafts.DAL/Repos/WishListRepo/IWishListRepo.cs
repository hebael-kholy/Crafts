using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.WishListRepo
{
    public interface IWishListRepo:IGenericRepo<Wishlist>
    {
        Wishlist? GetUserWishList(string userId);
    }
}
