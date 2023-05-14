using Crafts.BL.Dtos;
using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Managers.CategoryManagers;
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

        [HttpPost]
        public async Task<ActionResult> Add(CategoryAddDto categoryAddDto)
        {
            try
            {
                await _categoryManager.Add(categoryAddDto);
                var msg = new GeneralResponse("Category Added Successfully");
                var res = new { msg, categoryAddDto };
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("image/{id:int}")] //"api/categories/uploadimage" (Edit Image)
        public ActionResult AddImage([FromForm] CategoryImgAddDto categoryImgAddDto, int id)
        {
            try
            {
                _categoryManager.AddImage(categoryImgAddDto, id);
                var msg = new GeneralResponse($"Image Updated Successfully with Id: {id}");
                var res = new { msg, id, categoryImgAddDto };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
