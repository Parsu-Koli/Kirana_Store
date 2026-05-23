using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace KiranaStoreUI.Controllers
{
    public class BaseLoginController : Controller
    {
        protected readonly HttpClient _client;

        public BaseLoginController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("api");
        }

        // ADD THIS METHOD HERE
        protected bool AddJwtToken()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] =
                    "Please login first.";

                return false;
            }

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return true;
        }

        protected async Task<bool> IsUnauthorized(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();

                TempData["ErrorMessage"] =
                    "Session expired. Please login again.";

                return true;
            }

            return false;
        }

        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "User");
        }
    }
}
