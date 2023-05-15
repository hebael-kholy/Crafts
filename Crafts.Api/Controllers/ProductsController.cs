using Crafts.BL.Dtos.CouponDtos;
using Crafts.BL.Dtos.ProductDtos;
using Crafts.BL.Managers.ProductManager;
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
        [Route("{id:int}")]
        public ActionResult<ProductReadDto> GetById(int id) 
        {
            var product = _productsManager.GetById(id); 
            return product;
        }

        [HttpPost]
        public ActionResult Add(ProductAddDto productDto) {
            _productsManager.Add(productDto);
            return NoContent();
        }
    }
}
