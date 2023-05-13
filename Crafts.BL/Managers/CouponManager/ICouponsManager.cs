using Crafts.BL.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.CouponManager
{
    public interface ICouponsManager
    {
        Task Add(CouponAddDto couponDto);
        List<CouponReadDto> GetAll();
        CouponReadDto? GetById(int id);

    }
}
