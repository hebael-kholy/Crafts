using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using Crafts.DAL.Repos.IdentityRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.CouponRepo
{
    public class CouponRepo : GenericRepo<Coupon>, ICouponRepo
    {
        public CouponRepo(CraftsContext context) : base(context)
        {
        }
    }
}
