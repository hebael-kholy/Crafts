using Crafts.BL.Dtos.CouponDtos;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.CouponRepo;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.CouponManager
{
    public class CouponsManager : ICouponsManager
    {
        private readonly ICouponRepo _couponRepo;

        public CouponsManager(ICouponRepo couponRepo)
        {
            _couponRepo = couponRepo;
        }
        public  async Task Add(CouponAddDto couponDto)
        {
            var coupon = new Coupon
            {
                Name = couponDto.Name,
                ExpireDate = couponDto.ExpireDate,
                Discount = couponDto.Discount
            };
            await _couponRepo.Add(coupon);
            _couponRepo.SaveChanges();
        }
    }
}
