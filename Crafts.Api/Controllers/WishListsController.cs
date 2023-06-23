using Crafts.BL.Dtos;
using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Dtos.ProductDtos;
using Crafts.BL.Dtos.WishListDto;
using Crafts.BL.Managers.WishListManager;
using Crafts.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crafts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListsController : ControllerBase
    {
        private readonly IWishListManager _wishListManager;

        public WishListsController(IWishListManager wishListManager)
        {
            _wishListManager = wishListManager;
        }

        [HttpGet]
        public ActionResult<List<WishListReadDto>> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _wishListManager.GetAll();
        }

        [HttpGet]
        [Route("User")]
        public ActionResult<WishListReadDto> GetUserWishlist(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _wishListManager.GetUserWishList(userId);
        }
        

        [HttpPost]
        public async Task<ActionResult> Add(WishListAddDto wishListAddDto)
        {
            await _wishListManager.Add(wishListAddDto);
            return Ok(wishListAddDto);
        }

        //Get WishList With Products
        [HttpGet]
        [Route("{id}")]
        public ActionResult<WishlistWithProductsDto> GetWishListWithProducts(int id)
        {
            var w = _wishListManager.GetByIdWithProducts(id);
            return Ok(w);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            var wishList = _wishListManager.GetById(id);
            if (wishList?.Id != id) return NotFound(new { Message = "No WishLists Found!!" });

            _wishListManager.Delete(id);
            return Ok("Deleted Successfully");
        }

        //Add Product To WishList
        [HttpPost]
        [Route("/WishListProduct/{wishlistId}")]
        public ActionResult AddProductToWishlist( int wishlistId, ProductToAddToWishList product)
        {
            try
            {
                _wishListManager.AddProductToWishlistAsync(wishlistId, product);
                var msg = new GeneralResponse($"WishList with id {wishlistId} Updated Successfully");
                var res = new { msg, wishlistId };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delete Product From WishList
        [HttpDelete]
        [Route("{wishlistId}/{productId}")]
        public ActionResult DeleteProduct(int productId, int wishlistId)
        {
            try
            {
                _wishListManager.DeleteProductFromWishListAsync(productId, wishlistId);
                var msg = new GeneralResponse($"Product with id {productId} is deleted successfully from wishlist with id {wishlistId}");
                var res = new { msg, wishlistId };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
