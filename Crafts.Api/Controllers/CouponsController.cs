using Crafts.BL.Dtos.CouponDtos;
using Crafts.BL.Managers.CouponManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crafts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponsManager _couponsManagers;

        public CouponsController(ICouponsManager couponsManagers)
        {
            _couponsManagers = couponsManagers;
        }

        [HttpPost]
        public async Task<ActionResult> Add(CouponAddDto couponDto)
        {
            await _couponsManagers.Add(couponDto);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<List<CouponReadDto>> GetAll()
        {
            return _couponsManagers.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<CouponReadDto> GetById(int id)
        {
            var coupon = _couponsManagers.GetById(id);
            return coupon;
        }
    }
}
