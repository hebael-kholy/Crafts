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

        public CouponReadDto GetCouponByName(string Name)
        {
            Coupon? coupon = _couponRepo.GetCouponByName(Name);
            if (coupon == null)
            {
                throw new ArgumentException($"Coupon with Name {Name} is not found");
            }
            return new CouponReadDto
            {
                Id = coupon.Id,
                Name = coupon.Name,
                ExpireDate = coupon.ExpireDate,
                Discount = coupon.Discount
            };
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

        public List<CouponReadDto> GetAll()
        {
            List<Coupon> couponsFromDb = _couponRepo.GetAll();
            return couponsFromDb
                .Select(c => new CouponReadDto 
                { 
                    Id = c.Id,
                    Name = c.Name,
                    ExpireDate = c.ExpireDate,
                    Discount = c.Discount
                }).ToList();
        }

        public CouponReadDto? GetById(int id)
        {
            Coupon? coupon = _couponRepo.GetById(id);
            if (coupon == null)
            {
                throw new ArgumentException($"Coupon with id {id} is not found");
            }
            return new CouponReadDto
            {
                Id = coupon.Id,
                Name = coupon.Name,
                ExpireDate = coupon.ExpireDate,
                Discount = coupon.Discount
            };
        }

        public void Delete(int id)
        {
            Coupon? coupon = _couponRepo.GetById(id);
            if (coupon == null)
            {
                throw new ArgumentException($"Coupon with id {id} is not found");
            }
            _couponRepo.Delete(coupon);
            _couponRepo.SaveChanges();

        }

        public void Edit(CouponEditDto couponEditDto, int id)
        {
            Coupon? couponToEdit = _couponRepo.GetById(id);
            if (couponToEdit == null)
            {
                throw new ArgumentException($"Coupon with id {id} is not found");
            }

            couponToEdit.Name = couponEditDto.Name;
            couponToEdit.Discount = couponEditDto.Discount;
            couponToEdit.ExpireDate = couponEditDto.ExpireDate;
            
            _couponRepo.Update(couponToEdit);
            _couponRepo.SaveChanges();
        }
    }
}
