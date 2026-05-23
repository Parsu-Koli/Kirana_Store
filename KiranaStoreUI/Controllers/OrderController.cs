using KiranaStoreUI.Controllers;
using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KiaranaStroreUI.Controllers
{
    public class OrderController(IHttpClientFactory httpClientFactory)
    : BaseLoginController(httpClientFactory)
    {

        // GET: Order
        public async Task<IActionResult> Index()
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            var orders = await _client.GetFromJsonAsync<List<Order>>("Order/GetOrders");
            return View(orders);
        }

        // GET: Order/Create
        public IActionResult Create() => View();

        // POST: Order/Create
        [HttpPost]
        public async Task<IActionResult> Create(Order model)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            if (!ModelState.IsValid)
                return View(model);

            var result = await _client.PostAsJsonAsync("Order/AddOrder", model);

            if (result.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Failed to create order");
            return View(model);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            var order = await _client.GetFromJsonAsync<Order>($"Order/GetOrder/{id}");
            return View(order);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            var order = await _client.GetFromJsonAsync<Order>($"Order/GetOrder/{id}");
            return View(order);
        }

        // POST: Order/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Order model)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            if (!ModelState.IsValid)
                return View(model);

            var response = await _client.PutAsJsonAsync($"Order/UpdateOrder/", model);


            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

         
            ModelState.AddModelError("", "Edit endpoint not available in API");
            return View(model);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            var order = await _client.GetFromJsonAsync<Order>($"Order/GetOrder/{id}");
            return View(order);
        }

        // POST: Order/Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            // Your API does NOT have Delete endpoint → Add it OR disable delete.
            return RedirectToAction("Index");
        }

        // Search orders by date
        public async Task<IActionResult> OrdersByDate(DateTime date)
        {
            var result = await _client.GetFromJsonAsync<List<Order>>($"Order/OrdersByDate/{date:yyyy-MM-dd}");
            return View("Index", result);
        }
    }
}
