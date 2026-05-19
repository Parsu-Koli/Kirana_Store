using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace KiranaStoreUI.Controllers
{
    public class DashBoardController(IHttpClientFactory factory) : Controller
    {
        private readonly HttpClient _client = factory.CreateClient("api");

        private void AddJwtToken()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            AddJwtToken();

            // Fetch data from APIs
            var allSales = await _client.GetFromJsonAsync<List<Sale>>("Sale/GetAllSales");
            var products = await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");
            var customers = await _client.GetFromJsonAsync<List<Customer>>("Customer/GetCustomers");

            // Calculate metrics
            var totalSalesAmount = allSales.Sum(s => s.NetAmount);
            var totalSales = allSales.Count;
            var totalCustomers = customers.Count;
            var totalProfit = allSales.Sum(s => s.SaleItems?.Sum(i => (i.Price - products.FirstOrDefault(p => p.ProductId == i.ProductId)?.PurchasePrice ?? 0) * i.Quantity) ?? 0);

            // Sales overview (last 7 days)
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.Date.AddDays(-i))
                .OrderBy(d => d)
                .ToList();

            var salesOverview = last7Days.Select(date => new
            {
                Date = date.ToString("MMM dd"),
                TotalSales = allSales.Where(s => s.SaleDate.Date == date).Sum(s => s.NetAmount)
            }).ToList();

            // Sales by payment mode
            var salesByPaymentMode = allSales
                .GroupBy(s => s.PaymentMode)
                .Select(g => new { PaymentMode = g.Key, Count = g.Count() })
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

            return View();
        }
    }
}

