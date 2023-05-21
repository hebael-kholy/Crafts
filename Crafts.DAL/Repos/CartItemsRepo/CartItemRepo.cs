using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.CartItemsRepo
{
    public class CartItemRepo : GenericRepo<CartItem> , ICartItemRepo
    {
        private readonly CraftsContext _context;
        public CartItemRepo(CraftsContext context) : base(context)
        {
            _context = context;
        }
        #region GetByCartIdAndProductId
        public CartItem GetByCartIdAndProductId(int cartId, int productId)
        {
            return _context.CartItems.SingleOrDefault(c => c.CartId == cartId && c.ProductId == productId);
        }
        #endregion

        #region DeleteAllCartItemsByCartId
        public void DeleteAllCartItemsByCartId(int cartId)
        {
            var cartItemsToDelete = _context.CartItems.Where(ci => ci.CartId == cartId);
            _context.CartItems.RemoveRange(cartItemsToDelete);
            _context.SaveChanges();
        }
        #endregion

    }
}
