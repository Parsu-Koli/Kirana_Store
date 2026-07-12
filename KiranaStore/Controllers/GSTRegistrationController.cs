using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiranaStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GSTRegistrationController : ControllerBase
    {
        private readonly GstRegistrationService _gstService;

        public GSTRegistrationController(GstRegistrationService gstService)
        {
            _gstService = gstService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_gstService.GetAll());
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var data = _gstService.GetById(id);

            if (data == null)
                return NotFound("GST Registration not found.");

            return Ok(data);
        }

        [HttpGet("GetActive")]
        public IActionResult GetActive()
        {
            var data = _gstService.GetActiveRegistration();

            if (data == null)
                return NotFound("GST is not enabled.");

            return Ok(data);
        }

        [HttpPost("Add")]
        public IActionResult Add(GstRegistration model)
        {
            try
            {
                _gstService.Add(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("Update")]
        public IActionResult Update(GstRegistration registration)
        {
            try
            {
                _gstService.Update(registration);
                return Ok("GST Registration Updated Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("EnableDisable")]
        public IActionResult EnableDisable(int id, bool status)
        {
            try
            {
                _gstService.EnableDisableGST(id, status);

                return Ok(status
                    ? "GST Enabled Successfully."
                    : "GST Disabled Successfully.");
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
                _gstService.Delete(id);
                return Ok("GST Registration Deleted Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ToggleGST")]
        public IActionResult ToggleGST(bool enabled)
        {
            try
            {
                _gstService.ToggleGST(enabled);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}