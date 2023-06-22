using Crafts.BL.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crafts.BL.Managers.OrderManagers;
using Crafts.BL.Dtos.OrderDtos;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Crafts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        public OrdersController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [HttpGet]
        public ActionResult<List<OrderReadDto>> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _orderManager.GetAll();
        }
        
        [HttpGet]
        [Route("{id}/{status}")]
        public ActionResult<List<OrderReadDto>> GetByStatus(string id, int status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return _orderManager.GetByStatus(id, status);
        }

        [HttpGet]
        [Route("user/{id}")]
        public ActionResult<List<OrderReadDto>> GetUserOrders(string id)
        {
            List<OrderReadDto> orders = _orderManager.GetUserOrders(id);
            if (orders == null)
            {
                return NotFound(new GeneralResponse("User does not have orders"));
            }
            return orders;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<OrderReadDto> GetById(int id)
        {
            var order = _orderManager.GetById(id);
            if (order == null)
            {
                return NotFound(new GeneralResponse("Order is not found"));
            }
            return order;
        }

        [HttpPost]
        public async Task<ActionResult> Add(OrderAddDto order)
        {
            try
            {
                await _orderManager.Add(order);
                var msg = new GeneralResponse("Order Added Successfully");
                var res = new { msg, order };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult Edit(OrderEditDto order, int id)
        {
            try
            {
                _orderManager.Edit(order, id);
                var msg = new GeneralResponse($"Order with id {id} Updated Successfully");
                var res = new { msg, id, order };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("cancelorder/{id:int}")]
        public ActionResult CancelOrder(int id)
        {
            try
            {
                _orderManager.CancelOrder(id);
                var msg = new GeneralResponse($"Order with id {id} Updated Successfully");
                var res = new { msg, id };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("acceptorder/{id:int}")]
        public ActionResult AcceptOrder(int id)
        {
            try
            {
                _orderManager.AcceptOrder(id);
                var msg = new GeneralResponse($"Order with id {id} Updated Successfully");
                var res = new { msg, id };
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
            _orderManager.Delete(id);
            var msg = new GeneralResponse($"Order with id {id} Deleted Successfully");
            var res = new { msg, id };
            return Ok(res);
        }

        
    }
}