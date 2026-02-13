using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using DAL.Models;

namespace KiranaStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(PaymentService service) : ControllerBase
    {
        private readonly PaymentService _service = service;

        [HttpPost("AddPayment")]
        public IActionResult AddPayment(Payment payment)
        {
            try
            {
                _service.AddPayment(payment);
                return Ok("Payment Successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPayments")]
        public IActionResult GetPayments()
        {
            return Ok(_service.GetAllPayments());
        }

    }
}
