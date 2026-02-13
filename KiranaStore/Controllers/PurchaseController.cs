using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace KiranaStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController(PurchaseService service) : ControllerBase
    {
        private readonly PurchaseService _service = service;

        [HttpPost("AddPurchase")]
        public IActionResult AddPurchase(Purchase p)
        {
            _service.AddPurchase(p);
            return Ok("Purchase Saved");
        }

        [HttpGet("GetPurchase/{id}")]
        public IActionResult GetPurchase(int id)
        {
            var data = _service.GetPurchase(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpGet("GetPurchases")]
        public IActionResult GetPurchases()
        {
            return Ok(_service.GetAllPurchases());
        }
    }
}
