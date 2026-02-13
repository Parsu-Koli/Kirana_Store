using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using DAL.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace KiranaStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController(SaleService saleService, ProductService productService) : ControllerBase
    {
        private readonly SaleService _saleService = saleService;
        private readonly ProductService _productService = productService;

        [HttpGet("SearchProducts")]
        public IActionResult SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Ok(new List<Product>());

            var products = _productService.SearchProducts(keyword).ToList();
            return Ok(products);
        }

        
        [HttpPost("AddSale")]
        public IActionResult AddSale(Sale sale)
        {
            try
            {
                
                if (sale.SaleItems != null)
                {
                    foreach (var item in sale.SaleItems)
                    {
                        item.Sale = null; 
                    }
                }

                _saleService.AddSale(sale);
                return Ok("Sale Added Successfully");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpGet("GetSale/{id}")]
        public IActionResult GetSale(int id)
        {
            var sale = _saleService.GetSale(id);

            if (sale == null)
                return NotFound("Sale not found");

           
            if (sale.SaleItems != null)
            {
                foreach (var item in sale.SaleItems)
                {
                    if (item.ProductId > 0 && item.Product == null)
                    {
                        item.Product = _productService.GetProduct(item.ProductId);
                    }
                }
            }

            return Ok(sale);
        }

       
        [HttpGet("GetAllSales")]
        public IActionResult GetAllSales()
        {
            var sales = _saleService.GetAllSales();

            
            foreach (var sale in sales)
            {
                if (sale.SaleItems != null)
                {
                    foreach (var item in sale.SaleItems)
                    {
                        if (item.ProductId > 0 && item.Product == null)
                        {
                            item.Product = _productService.GetProduct(item.ProductId);
                        }
                    }
                }
            }

            return Ok(sales);
        }

        [HttpPut("UpdateSale")]
        public IActionResult UpdateSale([FromBody] Sale sale)
        {
            _saleService.UpdateSale(sale);
            return Ok(sale);
        }

        [HttpDelete("DeleteSale")]
        public IActionResult DeleteSale(int id)
        {
            _saleService.Delete(id);
            return Ok("Sale Deleted SuccessFully");
        }


        [HttpGet("GetNextInvoice")]
        public string GetNextInvoice()
        {
            return _saleService.GetNextInvoiceNumber();
        }
    }
}
