using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiranaStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(OrderService orderService) : ControllerBase
    {
        private readonly OrderService _orderService = orderService;

        [HttpPost("AddOrder")]
        public IActionResult AddOrder(Order order)
        {
            _orderService.AddOrder(order);
            return Ok("Order Created Successfully");
        }

        [HttpGet("GetOrder/{id}")]
        public IActionResult GetOrder(int id)
        {
            var data = _orderService.GetOrder(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpGet("GetOrders")]
        public IActionResult GetOrders()
        {
            return Ok(_orderService.GetAllOrders());
        }

        [HttpGet("OrdersByDate/{date}")]
        public IActionResult OrdersByDate(DateTime date)
        {
            return Ok(_orderService.GetOrdersByDate(date));
        }

        [HttpPut("UpdateOrder")]
        public IActionResult UpdateOrder(Order o)
        {
            try
            {
                _orderService.UpdateOrder(o);
                return Ok("Order Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteOrder/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                _orderService.DeleteOrder(id);
                return Ok("Order Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
