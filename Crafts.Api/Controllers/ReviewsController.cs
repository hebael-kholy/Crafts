using Crafts.BL.Dtos;
using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Dtos.ReviewDtos;
using Crafts.BL.Managers.ReviewManagers;
using Crafts.DAL.Models;
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

        //[HttpGet]
        //[Route("{id:int}")]
        //public ActionResult<ReviewReadDto> GetById(int id)
        //{
        //    var review = _reviewManager.GetById(id);
        //    if (review == null)
        //    {
        //        return NotFound(new GeneralResponse("Review is not found"));
        //    }
        //    return review;
        //}

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<List<ReviewReadDto>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _reviewManager.GetReviewsByProductId(id);
        }
        [HttpPost]
        public async Task<ActionResult> Add(ReviewAddDto review)
        {
            try
            {
                await _reviewManager.Add(review);
                var msg = new GeneralResponse("Review Added Successfully");
                var res = new { msg, review };
                return Ok(res);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult Edit(ReviewEditDto review, int id)
        {
            try
            {
                _reviewManager.Edit(review, id);
                var msg = new GeneralResponse($"Review with id {id} Updated Successfully");
                var res = new { msg, id, review };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _reviewManager.Delete(id);
            var msg = new GeneralResponse($"Review with id {id} Deleted Successfully");
            var res = new { msg, id };
            return Ok(res);
        }
    }
}
