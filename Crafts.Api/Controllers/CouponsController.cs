using Crafts.BL.Dtos;
using Crafts.BL.Dtos.CouponDtos;
using Crafts.BL.Dtos.ReviewDtos;
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
            try
            {
                await _couponsManagers.Add(couponDto);
                var msg = new GeneralResponse("Coupon Added Successfully");
                var res = new { msg, couponDto };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<List<CouponReadDto>> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _couponsManagers.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<CouponReadDto> GetById(int id)
        {
            var coupon = _couponsManagers.GetById(id);
            if (coupon == null)
            {
                return NotFound(new GeneralResponse("Resource is missing"));
            }
            return coupon;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            _couponsManagers.Delete(id);
            var msg = new GeneralResponse($"Coupon with id {id} Deleted Successfully");
            var res = new { msg, id };
            return Ok(res);
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult Edit(CouponEditDto coupon, int id)
        {
            try
            {
                _couponsManagers.Edit(coupon, id);
                var msg = new GeneralResponse($"Coupon with id {id} Updated Successfully");
                var res = new { msg, id, coupon};
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
