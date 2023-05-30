using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.WishListRepo
{
    public class WishListRepo:GenericRepo<Wishlist>,IWishListRepo
    {
        private readonly CraftsContext _context;

        public WishListRepo(CraftsContext context):base(context) 
        {
            _context = context;
        }

        //Get user wishList
        public Wishlist? GetUserWishList(string userId)
        {
            return _context.Set<Wishlist>()
                .Include(w=>w.Products)
                .FirstOrDefault(c => c.UserId == userId);
        }
        //Get WishList With Products
        public Wishlist? GetByIdWithProducts(int id)
        {
            //Explicit Loading
            return _context.Set<Wishlist>()
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);
           
        }


        public void DeleteProductByWishlistId(int wishlistId, int productId)
        {
            var wishlist = _context.Wishlists.FirstOrDefault(w => w.Id == wishlistId);
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            var productToDelete = wishlist.Products.Remove(product);
            _context.SaveChanges();
        }


    }

}

//Faild
//public void DeleteProductFromWishlist(int productId)
//{
//    var productToDelete = _context.Products.Where(p => p.Id == productId);
//    _context.Products.RemoveRange(productToDelete);
//    _context.SaveChanges();
//}
