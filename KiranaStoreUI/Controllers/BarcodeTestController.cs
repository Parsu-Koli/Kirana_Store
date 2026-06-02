using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KiranaStoreUI.Controllers
{
    // Shared barcode storage
    public static class ScanStorage
    {
        public static string LastBarcode { get; set; } = "";
    }

    public class BarcodeTestController(IHttpClientFactory httpClientFactory)
        : BaseLoginController(httpClientFactory)
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct(string barcode)
        {
            AddJwtToken();

            var response =
                await _client.GetAsync($"Product/barcode/{barcode}");

            if (!response.IsSuccessStatusCode)
            {
                return Content("<div class='alert alert-danger'>Product Not Found</div>");
            }

            var product =
                await response.Content.ReadFromJsonAsync<Product>();

            return PartialView("_ProductDetails", product);
        }

        // ===============================
        // NEW METHOD
        // Mobile saves barcode here
        // ===============================
        [HttpPost]
        public IActionResult SaveBarcode([FromBody] BarcodeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Barcode))
            {
                return BadRequest();
            }

            ScanStorage.LastBarcode = request.Barcode;

            return Ok(new
            {
                success = true
            });
        }

        // ===============================
        // NEW METHOD
        // Laptop reads barcode from here
        // ===============================
        [HttpGet]
        public IActionResult GetBarcode()
        {
            var barcode = ScanStorage.LastBarcode;

            // clear after reading
            ScanStorage.LastBarcode = "";

            return Json(new
            {
                barcode = barcode
            });
        }
    }

    public class BarcodeRequest
    {
        public string Barcode { get; set; }
    }
}