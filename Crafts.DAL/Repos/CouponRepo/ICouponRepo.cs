using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.CouponRepo
{
    public interface ICouponRepo : IGenericRepo<Coupon>
    {
        Coupon GetCouponByName(string Name);
    }
}
