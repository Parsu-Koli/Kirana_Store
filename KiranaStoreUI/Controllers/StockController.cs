using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KiaranaStroreUI.Controllers
{
    public class StockController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public StockController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        private HttpClient CreateClientWithToken()
        {
            var client = _factory.CreateClient("api");
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public async Task<IActionResult> Index()
        {
            var client = CreateClientWithToken();
            var data = await client.GetFromJsonAsync<List<Stock>>("Stock/GetAllStock");
            return View(data);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Stock model)
        {
            var client = CreateClientWithToken();
            var result = await client.PostAsJsonAsync("Stock/IncreaseStock?productId=" + model.ProductId + "&qty=" + model.Quantity, model);

            if (result.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(model);
        }

        public async Task<IActionResult> Chart()
        {
            var client = CreateClientWithToken();

            var products =
await client.GetFromJsonAsync<List<ProductStockDto>>
("Product/GetProducts")
?? new List<ProductStockDto>();
            var allSales =
await client.GetFromJsonAsync<List<Sale>>
("Sale/GetAllSales")
?? new List<Sale>();
            var customers =
await client.GetFromJsonAsync<List<Customer>>
("Customer/GetCustomers")
?? new List<Customer>();

            // Calculate metrics
            var totalSalesAmount = allSales.Sum(s => s.NetAmount);
            var totalSales = allSales.Count;
            var totalCustomers = customers.Count;
            decimal totalProfit = 0;

            foreach (var sale in allSales)
            {
                if (sale.SaleItems == null)
                    continue;

                foreach (var item in sale.SaleItems)
                {
                    var product = products
                        .FirstOrDefault(p => p.ProductId == item.ProductId);

                    decimal purchasePrice =
                        product?.PurchasePrice ?? 0;

                    totalProfit +=
                        (item.Price - purchasePrice)
                        * item.Quantity;
                }
            }
            // Sales overview (last 7 days)
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.Date.AddDays(-i))
                .OrderBy(d => d)
                .ToList();

            var salesOverview = last7Days.Select(date => new SalesOverviewDto
            {
                Date = date.ToString("MMM dd"),
                TotalSales = allSales
        .Where(s => s.SaleDate.Date == date)
        .Sum(s => s.NetAmount)
            }).ToList();

            // Sales by payment mode
            var salesByPaymentMode = allSales
.GroupBy(s => s.PaymentMode)
.Select(g => new PaymentModeDto
{
    PaymentMode = g.Key,
    Count = g.Count()
})
.ToList();

            // Recent sales
            var recentSales = allSales
    .OrderByDescending(s => s.SaleDate)
    .Take(5)
    .ToList();

            // Attach customer data manually
            foreach (var sale in recentSales)
            {
                sale.Customer = customers
                    .FirstOrDefault(c => c.CustomerId == sale.CustomerId);
            }

            // Pass data to the view
            ViewBag.TotalSalesAmount = totalSalesAmount;
            ViewBag.TotalSales = totalSales;
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalProfit = totalProfit;
            ViewBag.SalesOverview = salesOverview;
            ViewBag.SalesByPaymentMode = salesByPaymentMode;
            ViewBag.RecentSales = recentSales;

            return View(products);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var data = await client.GetFromJsonAsync<Stock>($"Stock/GetStockByProduct/{id}");
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Stock model)
        {
            var client = CreateClientWithToken();
            var result = await client.PostAsJsonAsync("Stock/IncreaseStock?productId=" + model.ProductId + "&qty=" + model.Quantity, model);

            if (result.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClientWithToken();
            var data = await client.GetFromJsonAsync<Stock>($"Stock/GetStockByProduct/{id}");
            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClientWithToken();
            await client.DeleteAsync($"Stock/Delete/{id}");
            return RedirectToAction("Index");
        }
    }
}
