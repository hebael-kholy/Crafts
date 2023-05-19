using Crafts.BL.Dtos;
using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Managers.CategoryManagers;
using Crafts.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crafts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        public CategoriesController(ICategoryManager categoryManager) 
        {
            _categoryManager = categoryManager;
        }

        [HttpGet]
        public ActionResult<List<CategoryReadDto>> GetAll() 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _categoryManager.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<CategoryReadDto> GetById(int id) 
        {
            var category = _categoryManager.GetById(id);
            if (category == null)
            {
                return NotFound(new GeneralResponse("Category is not found"));
            }
            return category;
        }

        [HttpGet]
        [Route("{id:int}/Products")]
        public ActionResult<CategoryWithProductsDto> GetByIdWithProducts(int id)
        {
            try
            {
                var categoryWithProducts = _categoryManager.GetByIdWithProducts(id);
                return Ok(categoryWithProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(CategoryAddDto category)
        {
            try
            {
                await _categoryManager.Add(category);
                var msg = new GeneralResponse("Category Added Successfully");
                var res = new { msg, category };
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("image/{id:int}")] //"api/categories/uploadimage" (Edit Image)
        public ActionResult AddImage([FromForm] CategoryImgAddDto categoryImage, int id)
        {
            try
            {
                _categoryManager.AddImage(categoryImage, id);
                var msg = new GeneralResponse($"Image with id {id} Updated Successfully");
                var res = new { msg, id, categoryImage };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult Edit(CategoryEditDto category, int id)
        {
            try
            {
                _categoryManager.Edit(category, id);
                var msg = new GeneralResponse($"Category with id {id} Updated Successfully");
                var res = new { msg, id, category };
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
            _categoryManager.Delete(id);
            var msg = new GeneralResponse($"Category with id {id} Deleted Successfully");
            var res = new { msg, id };
            return Ok(res);
        }
    }
}
