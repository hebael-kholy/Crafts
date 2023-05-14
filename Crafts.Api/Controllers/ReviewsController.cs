using Crafts.BL.Dtos;
using Crafts.BL.Dtos.ReviewDtos;
using Crafts.BL.Managers.ReviewManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crafts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewManager _reviewManager;
        public ReviewsController(IReviewManager reviewManager)
        {
            _reviewManager = reviewManager;
        }

        [HttpGet]
        public ActionResult<List<ReviewReadDto>> GetAll() 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _reviewManager.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<ReviewReadDto> GetById(int id)
        {
            var review = _reviewManager.GetById(id);
            if (review == null)
            {
                return NotFound(new GeneralResponse("Resource is missing"));
            }
            return review;
        }

        [HttpPost]
        public async Task<ActionResult> Add(ReviewAddDto reviewAddDto, int productId, int userId)
        {
            try
            {
                //To replace the productid, userid that are in Dto with the productid, userid that are correctly exist
                reviewAddDto.ProductId = productId; 
                reviewAddDto.UserId = userId;

                await _reviewManager.Add(reviewAddDto, productId, userId);
                var msg = new GeneralResponse("Review Added Successfully");
                var res = new { msg, reviewAddDto };
                return Ok(res);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
