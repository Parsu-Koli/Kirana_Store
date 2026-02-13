using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
namespace KiranaStoreUI.Controllers
{
   
    public class CategoryController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient("api");

        private void AddJwtToken()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            AddJwtToken();
            var list = await _client.GetFromJsonAsync<List<Category>>("Category/GetCategories");
            return View(list);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            AddJwtToken();
            if (!ModelState.IsValid)
                return View(model);

            var result = await _client.PostAsJsonAsync("Category/AddCategory", model);

            if (result.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error creating category");
            return View(model);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            AddJwtToken();
            var category = await _client.GetFromJsonAsync<Category>($"Category/GetCategory/{id}");
            return View(category);
        }

        // POST: Category/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            AddJwtToken();
            if (!ModelState.IsValid)
                return View(model);

            var result = await _client.PutAsJsonAsync($"Category/UpdateCategory/", model);

            if (result.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error updating category");
            return View(model);
        }



        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            AddJwtToken();
            var category = await _client.GetFromJsonAsync<Category>($"Category/GetCategory/{id}");
            return View(category);
        }

        // POST: Category/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            AddJwtToken();
            var result = await _client.DeleteAsync($"Category/DeleteCategory/{id}");

            if (result.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error deleting category");
            return RedirectToAction(nameof(Index));
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int id)
        {
            AddJwtToken();
            var category = await _client.GetFromJsonAsync<Category>($"Category/GetCategory/{id}");
            return View(category);
        }

        public async Task<IActionResult> All_productByID(int id)
        {
            AddJwtToken();

            var products = await _client
                .GetFromJsonAsync<List<Product>>($"Category/GetAllProductsById/{id}");

            return View(products);
        }

    }
}
