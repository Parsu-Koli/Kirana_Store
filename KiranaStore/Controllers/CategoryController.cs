using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KiranaStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(CategoryService categoryService) : ControllerBase
    {
        private readonly CategoryService _categoryService = categoryService;

        [HttpPost("AddCategory")]
        public IActionResult AddCategory(Category c)
        {
            _categoryService.AddCategory(c);
            return Ok("Category Added Successfully");
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            return Ok(_categoryService.GetAllCategories());
        }

        [HttpGet("GetCategory/{id}")]
        public IActionResult GetCategory(int id)
        {
            var data = _categoryService.GetCategory(id);
            if (data == null) return NotFound("Category Not Found");
            return Ok(data);
        }

        [HttpPut("UpdateCategory")]
        public IActionResult UpdateCategory(Category c)
        {
            _categoryService.UpdateCategory(c);
            return Ok("Category Updated Successfully");
        }

        [HttpDelete("DeleteCategory/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            _categoryService.DeleteCategory(id);
            return Ok("Category Deleted Successfully");
        }

        [HttpGet("GetAllProductsById/{id}")]
        public IActionResult GetProductByCatagoryId(int id)
        {
             var data= _categoryService.GetProductById(id);
            if (data == null) return NotFound("Category Not Found");
            return Ok(data);
        }
    }
}
