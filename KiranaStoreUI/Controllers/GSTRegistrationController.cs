using KiranaStoreUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KiranaStoreUI.Controllers
{
    public class GSTRegistrationController(IHttpClientFactory httpClientFactory)
        : BaseLoginController(httpClientFactory)
    {
        // ===================== INDEX =====================

        public async Task<IActionResult> Index()
        {
            AddJwtToken();

            if (!AddJwtToken())
                return RedirectToLogin();

            var list = await _client.GetFromJsonAsync<List<GstRegistration>>("GSTRegistration/GetAll");
            return View(list);
        }

        // ===================== CREATE =====================

        [HttpGet]
        public IActionResult Create()
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            return View(new GstRegistration
            {
                CreatedDate = DateTime.UtcNow
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(GstRegistration model)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            if (!ModelState.IsValid)
                return View(model);

            model.CreatedDate = DateTime.UtcNow;

    
            var result = await _client.PostAsJsonAsync("GSTRegistration/Add", model);

            var response = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(response);
            }

            return View(model);
        }

        // ===================== EDIT =====================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            var gst = await _client.GetFromJsonAsync<GstRegistration>(
                $"GSTRegistration/Get/{id}");

            if (gst == null)
                return NotFound();

            return View(gst);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GstRegistration model)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            if (!ModelState.IsValid)
                return View(model);

            var result = await _client.PutAsJsonAsync(
                "GSTRegistration/Update",
                model);

            if (result.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", await result.Content.ReadAsStringAsync());

            return View(model);
        }

        // ===================== DETAILS =====================

        public async Task<IActionResult> Details(int id)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            var gst = await _client.GetFromJsonAsync<GstRegistration>(
                $"GSTRegistration/Get/{id}");

            if (gst == null)
                return NotFound();

            return View(gst);
        }

        // ===================== DELETE =====================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            var gst = await _client.GetFromJsonAsync<GstRegistration>(
                $"GSTRegistration/Get/{id}");

            if (gst == null)
                return NotFound();

            return View(gst);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!AddJwtToken())
                return RedirectToLogin();

            var result = await _client.DeleteAsync(
                $"GSTRegistration/Delete/{id}");

            if (result.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Unable to delete GST Registration.");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleGST(bool enabled)
        {
            if (!AddJwtToken())
                return RedirectToAction("Login", "User");

            var response = await _client.PostAsync(
    $"GSTRegistration/ToggleGST?enabled={enabled}",
    null);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest(await response.Content.ReadAsStringAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrent()
        {
            if (!AddJwtToken())
                return Unauthorized();

            var gst = await _client.GetFromJsonAsync<GstRegistration>("GSTRegistration/GetActive");

            if (gst == null)
                return Json(new
                {
                    gstRegistrationId = 0,
                    isGstEnabled = false
                });

            return Json(new
            {
                gstRegistrationId = gst.GstRegistrationId,
                isGstEnabled = gst.IsGstEnabled
            });
        }

        [HttpPost]
        public async Task<IActionResult> EnableDisable(int id, bool status)
        {
            if (!AddJwtToken())
                return RedirectToAction("Login", "User");

            var response = await _client.PutAsync(
                $"GSTRegistration/EnableDisable?id={id}&status={status}",
                null);

            if (response.IsSuccessStatusCode)
                return Ok();

            return BadRequest(await response.Content.ReadAsStringAsync());
        }
    }
}