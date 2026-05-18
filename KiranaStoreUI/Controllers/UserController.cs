using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace KiranaStoreUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public UserController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        private HttpClient CreateClientWithToken()
        {
            var client = _factory.CreateClient("api");
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
     
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _factory.CreateClient("api");

            var loginDto = new
            {
                Username = model.Username,
                Password = model.Password
            };

            var response = await client.PostAsJsonAsync("Auth/Login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);

                var token = doc.RootElement.GetProperty("token").GetString();
                var role = doc.RootElement.GetProperty("role").GetString();
                var username = doc.RootElement.GetProperty("username").GetString();

                HttpContext.Session.SetString("JWToken", token);
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Role", role);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return RedirectToAction("Create", "sale");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _factory.CreateClient("api");

            var exists = await client.GetFromJsonAsync<bool>($"Auth/IsUsernameExists/{model.Username}");
            if (exists)
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(model);
            }

            var registerDto = new
            {
                FullName = model.FullName,
                Username = model.Username,
                Password = model.Password,
                Phone = model.Phone,
                Role = model.Role,
            };

            var response = await client.PostAsJsonAsync("Auth/Register", registerDto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Registration failed. Try again.");
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> IsUsernameExists(string username)
        {
            var client = _factory.CreateClient("api");
            var exists = await client.GetFromJsonAsync<bool>($"Auth/IsUsernameExists/{username}");
            return Json(exists);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }
    }
}
