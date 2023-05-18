using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
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
    }
}
