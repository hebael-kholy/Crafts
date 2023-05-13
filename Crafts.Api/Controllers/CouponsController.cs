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
        [Route("create")]
        public ActionResult Add(CouponAddDto couponDto)
        {
            _couponsManagers.Add(couponDto);
            return NoContent();
        }
    }
}
