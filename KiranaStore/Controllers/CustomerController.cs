using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace KiranaStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(CustomerService customerService) : ControllerBase
    {
        private readonly CustomerService _customerService = customerService;

        [HttpPost("AddCustomer")]
        public IActionResult AddCustomer(Customer c)
        {
            try
            {
                _customerService.AddCustomer(c);
                return Ok(c);  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCustomer/{id}")]
        public IActionResult GetCustomer(int id)
        {
            var data = _customerService.GetCustomer(id);
            if (data == null) return NotFound("Customer Not Found");
            return Ok(data);
        }

        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers()
        {
            return Ok(_customerService.GetAllCustomers());
        }

        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomer(Customer c)
        {
            try
            {
                _customerService.UpdateCustomer(c);
                return Ok("Customer Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                _customerService.DeleteCustomer(id);
                return Ok("Customer Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
