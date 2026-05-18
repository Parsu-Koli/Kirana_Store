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

            // =========================
            // Get API Data
            // =========================

            var allSales =
                await _client.GetFromJsonAsync<List<Sale>>("Sale/GetAllSales");

            var products =
                await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");

            var customers =
                await _client.GetFromJsonAsync<List<Customer>>("Customer/GetCustomers");

            // =========================
            // Product Dictionary
            // =========================

            var productDict = products.ToDictionary(p => p.ProductId);

            // =========================
            // Variables
            // =========================

            decimal totalProfit = 0;

            decimal totalSalesAmount = 0;

            int totalSales = allSales.Count;

            int totalCustomers = customers.Count;

            // =========================
            // Date Calculation
            // =========================

            DateTime now = DateTime.Now;

            DateTime currentMonthStart =
                new DateTime(now.Year, now.Month, 1);

            DateTime lastMonthStart =
                currentMonthStart.AddMonths(-1);

            DateTime lastMonthEnd =
                currentMonthStart.AddDays(-1);

            decimal currentMonthProfit = 0;

            decimal lastMonthProfit = 0;

            // =========================
            // Profit Calculation
            // =========================

            foreach (var sale in allSales)
            {
                decimal purchaseTotal = 0;

                decimal sellingTotal = 0;

                totalSalesAmount += sale.NetAmount;

                if (sale.SaleItems != null)
                {
                    foreach (var item in sale.SaleItems)
                    {
                        if (productDict.TryGetValue(item.ProductId, out var product))
                        {
                            purchaseTotal +=
                                product.PurchasePrice * item.Quantity;

                            sellingTotal +=
                                item.Price * item.Quantity;
                        }
                    }
                }

                decimal profit =
                    sellingTotal - purchaseTotal - sale.Discount;

                totalProfit += profit;

                // Current Month Profit

                if (sale.SaleDate >= currentMonthStart)
                {
                    currentMonthProfit += profit;
                }

                // Last Month Profit

                if (sale.SaleDate >= lastMonthStart &&
                    sale.SaleDate <= lastMonthEnd)
                {
                    lastMonthProfit += profit;
                }
            }

            // =========================
            // Growth Percentage
            // =========================

            decimal growthPercentage = 0;

            if (lastMonthProfit > 0)
            {
                growthPercentage =
                    ((currentMonthProfit - lastMonthProfit)
                    / lastMonthProfit) * 100;
            }

            // =========================
            // Business Status
            // =========================

            string businessStatus =
                growthPercentage >= 0
                ? "Business is Growing 📈"
                : "Business is Decreasing 📉";

            // =========================
            // Send Data To View
            // =========================

            ViewBag.TotalProfit = Math.Round(totalProfit, 2);

            ViewBag.TotalSalesAmount = Math.Round(totalSalesAmount, 2);

            ViewBag.TotalSales = totalSales;

            ViewBag.TotalCustomers = totalCustomers;

            ViewBag.GrowthPercentage =
                Math.Round(growthPercentage, 2);

            ViewBag.BusinessStatus = businessStatus;

            ViewBag.CurrentMonthProfit =
                Math.Round(currentMonthProfit, 2);

            ViewBag.LastMonthProfit =
                Math.Round(lastMonthProfit, 2);

            return View();
        }
    }
}