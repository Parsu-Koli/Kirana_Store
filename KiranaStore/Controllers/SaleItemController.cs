using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace KiranaStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleItemController(SaleItemService service) : ControllerBase
    {
        private readonly SaleItemService _service = service;

        [HttpPost("Add")]
        public IActionResult Add(SaleItem item)
        {
            try
            {
                _service.AddSaleItem(item);
                return Ok("Sale Item Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetItems/{saleId}")]
        public IActionResult GetItems(int saleId)
        {
            return Ok(_service.GetItemsBySale(saleId));
        }

        [HttpGet("GetAllSaleItems")]
        public IActionResult GetAllSaleItems()
        {
            return Ok(_service.GetAllSaleItems());
        }
    }
}
