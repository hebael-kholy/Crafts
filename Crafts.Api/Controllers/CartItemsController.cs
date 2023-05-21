using Crafts.BL.Dtos.CartDtos;
using Crafts.BL.Dtos;
using Crafts.BL.Managers.CartItemsManager;
using Crafts.BL.Managers.CartManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crafts.BL.Dtos.CartItemsDtos;
using Crafts.BL.Dtos.ReviewDtos;

namespace Crafts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemsManager _cartItemsManager;
        public CartItemsController(ICartItemsManager cartItemsManager)
        {
            _cartItemsManager= cartItemsManager;    
        }
        [HttpGet]
        public ActionResult<List<CartItemReadDto>> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _cartItemsManager.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<CartItemReadDto> GetById(int id)
        {
            var cartItem = _cartItemsManager.GetById(id);
            if (cartItem == null)
            {
                return NotFound(new GeneralResponse("Cart is not found"));
            }
            return cartItem;
        }
        [HttpPost]
        public async Task<ActionResult> Add(CartItemAddDto cartItem)
        {

                await _cartItemsManager.Add(cartItem);
                var msg = new GeneralResponse("Cart Item Added Successfully");
                var res = new { msg, cartItem };
                return Ok(res);

        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult Edit(CartItemEditDto cartItem, int id)
        {
            try
            {
                _cartItemsManager.Edit(cartItem, id);
                var msg = new GeneralResponse($"Cart Item with id {id} Updated Successfully");
                var res = new { msg, id, cartItem };
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
            _cartItemsManager.Delete(id);
            var msg = new GeneralResponse($"CartItem with id {id} Deleted Successfully");
            var res = new { msg, id };
            return Ok(res);
        }

        [HttpDelete]
        [Route("/AllItems/{cartId}")]

        public ActionResult DeleteCartItemsByCartId(int cartId)
        {
            _cartItemsManager.DeleteAllCartItemsByCartId(cartId);
            var msg = new GeneralResponse($"CartItems belong to Cart with id {cartId} Deleted Successfully");
            var res = new { msg, cartId };
            return Ok(res);
        }


    }
}
