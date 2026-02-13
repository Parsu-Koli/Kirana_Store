using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiranaStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ProductService productService) : ControllerBase
    {
        private readonly ProductService _productService = productService;

        [HttpPost("AddProduct")]
        public IActionResult AddProduct(Product product)
        {
            if (product.QuantityInStock < 0)
                return BadRequest("Invalid quantity");

            _productService.AddProduct(product);
            return Ok("Product added successfully");
        }



        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            try
            {
                return Ok(_productService.GetAllProducts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            try
            {
                var data = _productService.GetProduct(id);
                if (data == null)
                    return NotFound("Product Not Found");

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(Product p)
        {
            try
            {
                _productService.UpdateProduct(p);
                return Ok("Product Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return Ok("Product Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
