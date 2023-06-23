using Crafts.BL.Dtos;
using Crafts.BL.Dtos.CouponDtos;
using Crafts.BL.Dtos.ProductDtos;
using Crafts.BL.Managers.ProductManager;
using Crafts.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crafts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsManager _productsManager;
        

        public ProductsController(IProductsManager productManager)
        {
            _productsManager = productManager;
            
        }

        [HttpGet]
        public ActionResult<List<ProductReadDto>> GetAll()
        {
            return _productsManager.GetAll();
        }

        [HttpGet]
        [Route("/api/Products/Sale")]
        public ActionResult<List<ProductReadDto>> GetProductwithSale()
        {
            return _productsManager.GetProductwithSale();
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<ProductReadDto> GetById(int id) 
        {
            try
            {
                var product = _productsManager.GetById(id);
                return product;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm] ProductAddDto productDto) {
            try
            {
                await _productsManager.Add(productDto);
                var msg = new GeneralResponse("Product Added Successfully");
                var res = new { msg, productDto };
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        [Route("image/{id:int}")] //"api/categories/uploadimage" (Edit Image)
        public ActionResult AddImage([FromForm] ProductImgAddDto productImgAddDto, int id)
        {
            try
            {  
                _productsManager.AddImage(productImgAddDto, id);
                var msg = new GeneralResponse($"Image Updated Successfully with Id: {id}");
                var res = new { msg, id, productImgAddDto };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult Edit(int id, ProductUpdateDto productUpdateDto)
        {
            try
            {
                var product = _productsManager.GetById(id);

                _productsManager.Update(productUpdateDto, id);
                var msg = new GeneralResponse($"Product with id {id} Updated Successfully");
                var res = new { msg, product};
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
            var product = _productsManager.GetById(id);

            _productsManager.Delete(id);
            var msg = new GeneralResponse($"Product with id {id} Deleted Successfully");
            var res = new { msg, product };

            return Ok(res);
        }
    }
}
