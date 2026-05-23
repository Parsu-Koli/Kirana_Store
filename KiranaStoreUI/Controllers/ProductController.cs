using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KiranaStoreUI.Controllers
{
    public class ProductController(IHttpClientFactory httpClientFactory)
    : BaseLoginController(httpClientFactory)
    {
      

        // LIST
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Index()
        {
            AddJwtToken();
            if (!AddJwtToken())
                return RedirectToLogin();

            var data = await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");
            return View(data);
        }


        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            AddJwtToken();
                if (!AddJwtToken())
                    return RedirectToLogin();
            var p = await _client.GetFromJsonAsync<Product>($"Product/GetProduct/{id}");
            if (p == null) return NotFound();
            return View(p);
        }

        // CREATE (GET)
        public async Task<IActionResult> Create()
         {
            AddJwtToken();
            if (!AddJwtToken())
                return RedirectToLogin();

            await LoadCategories();
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            AddJwtToken();
            if (!AddJwtToken())
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return View(model);
            }

            var result = await _client.PostAsJsonAsync("Product/AddProduct", model);

            if (!result.IsSuccessStatusCode)
            {
                string apiResponse = await result.Content.ReadAsStringAsync();
                ModelState.AddModelError("", apiResponse);
                await LoadCategories();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            AddJwtToken();
            if (!AddJwtToken())
                return RedirectToLogin();
            var p = await _client.GetFromJsonAsync<Product>($"Product/GetProduct/{id}");
            if (p == null) return NotFound();

            await LoadCategories();
            return View(p);
        }

        // EDIT (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            AddJwtToken();
            if (!AddJwtToken())
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return View(model);
            }

            
            var result = await _client.PutAsJsonAsync("Product/UpdateProduct", model);

            if (result.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Failed to update product.");
            await LoadCategories();
            return View(model);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int id)
        {
            AddJwtToken();
            if (!AddJwtToken())
                return RedirectToLogin();

            var p = await _client.GetFromJsonAsync<Product>($"Product/GetProduct/{id}");
            if (p == null) return NotFound();

            return View(p);
        }

        // DELETE CONFIRMED
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            AddJwtToken();
            if (!AddJwtToken())
                return RedirectToLogin();
            await _client.DeleteAsync($"Product/DeleteProduct/{id}");
            return RedirectToAction("Index");
        }

        // Load categories
        private async Task LoadCategories()
        {
            
            var categories = await _client.GetFromJsonAsync<List<Category>>("Category/GetCategories");

            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();
        }
    }
}
