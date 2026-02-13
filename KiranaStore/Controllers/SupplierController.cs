using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using DAL.Models;

namespace KiranaStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController(SupplierService supplierService) : ControllerBase
    {
        private readonly SupplierService _supplierService = supplierService;

        [HttpPost("Add")]
        public IActionResult Add(Supplier s)
        {
            try
            {
                _supplierService.AddSupplier(s);
                return Ok("Supplier Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public IActionResult Update(Supplier s)
        {
            try
            {
                _supplierService.UpdateSupplier(s);
                return Ok("Supplier Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _supplierService.DeleteSupplier(id);
                return Ok("Supplier Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var data = _supplierService.GetSupplier(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_supplierService.GetAllSuppliers());
        }
    }
}
