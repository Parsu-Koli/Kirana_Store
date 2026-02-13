using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KiranaStoreUI.Controllers
{
    public class CustomerController(IHttpClientFactory factory) : Controller
    {
        private readonly HttpClient _client = factory.CreateClient("api");

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            var customers = await _client.GetFromJsonAsync<List<Customer>>("Customer/GetCustomers");
            return View(customers);
        }

        // GET: Customer/Create
        public IActionResult Create() => View();

        // POST: Customer/Create
        [HttpPost]
        public async Task<IActionResult> Create(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _client.PostAsJsonAsync("Customer/AddCustomer", model);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Failed to add customer.");
            return View(model);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _client.GetFromJsonAsync<Customer>($"Customer/GetCustomer/{id}");
            return View(customer);
        }

        // POST: Customer/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _client.PutAsJsonAsync($"Customer/UpdateCustomer/", model);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Failed to update customer.");
            return View(model);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _client.GetFromJsonAsync<Customer>($"Customer/GetCustomer/{id}");
            return View(customer);
        }

        // POST: Customer/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"Customer/DeleteCustomer/{id}");
            return RedirectToAction("Index");
        }
    }
}
