using Crafts.BL.Dtos.CartDtos;
using Crafts.BL.Dtos;
using Crafts.BL.Managers.CartManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crafts.DAL.Models;
using Crafts.BL.Dtos.ReviewDtos;

namespace Crafts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartManager _cartManager;
        public CartsController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }


        [HttpGet]
        public ActionResult<List<CartReadDto>> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _cartManager.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<CartReadDto> GetById(int id)
        {
            var cart = _cartManager.GetById(id);
            if (cart == null)
            {
                return NotFound(new GeneralResponse("Cart is not found"));
            }
            return cart;
        }

        [HttpGet]
        [Route("/WithCartItems/{id}")]
        public ActionResult<CartWithCartItemsReadDto> GetByIdWithCartItems(int id)
        {
            var cartDto = _cartManager.GetByIdWithCartItems(id);
            if (cartDto == null)
            {
                return NotFound();
            }
            return cartDto;
        }

        [HttpGet]
        [Route("/WithCartItems")]
        public ActionResult<CartWithCartItemsReadDto> GetByUserIdWithCartItems(string id)
        {
            var cartDto = _cartManager.GetByUserIdWithCartItems(id);
            if (cartDto == null)
            {
                return NotFound();
            }
            return cartDto;
        }

        [HttpPost]
        public async Task<ActionResult> Add(CartAddDto cart)
        {
            try
            {
                await _cartManager.Add(cart);
                var msg = new GeneralResponse("New Cart is Added Successfully OR existing Cart updated quantity");
                var res = new { msg, cart};
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/Coupon")]
        public ActionResult ApplyCoupon(int CartId, int couponId)
        {
            try
            {
                _cartManager.ApplyCouponForDiscount(CartId, couponId);
                var msg = new GeneralResponse($"Cart with id {CartId} Updated Successfully");
                var res = new { msg};
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            _cartManager.Delete(id);
            var msg = new GeneralResponse($"Cart with id {id} Deleted Successfully");
            var res = new { msg, id };
            return Ok(res);
        }


    }
}
