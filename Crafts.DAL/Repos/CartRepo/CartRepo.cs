using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using Microsoft.EntityFrameworkCore;

namespace Crafts.DAL.Repos.CartRepo
{
    public class CartRepo : GenericRepo<Cart>, ICartRepo

    {
        private readonly CraftsContext _context;
        public CartRepo(CraftsContext context) : base(context)
        {
            _context = context;
        }
        #region GetByIdWithCartItems
        public Cart? GetByIdWithCartItems(int id)
        {
            return _context.Set<Cart>()
            .Include(c => c.CartItems)
            .ThenInclude(ci=>ci.Product)
            .FirstOrDefault(c=>c.Id == id);             
        }
        #endregion


    }




}
