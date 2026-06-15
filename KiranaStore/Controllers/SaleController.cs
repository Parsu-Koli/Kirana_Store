using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using DAL.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using DAL.Repository.Implementation;

namespace KiranaStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly SaleService _saleService;
        private readonly ProductService _productService;

        public SaleController(
            SaleService saleService,
            ProductService productService)
        {
            _saleService = saleService;
            _productService = productService;
        }

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

                return Ok(sale);   // Return object instead of string
            }
            catch (Exception ex)
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
        [HttpGet("GetSalesPaged")]
        public IActionResult GetSalesPaged(
    int pageNumber = 1,
    int pageSize = 20)
        {
            var query = _saleService.GetSalesQueryable();

            var totalRecords = query.Count();

            var sales = query
                .OrderByDescending(x => x.SaleId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Data = sales
            });
        }
    }
}
