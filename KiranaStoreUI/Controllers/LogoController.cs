using Microsoft.AspNetCore.Mvc;

namespace KiranaStoreUI.Controllers
{
    public class LogoController(IWebHostEnvironment env) : Controller
    {
        private readonly IWebHostEnvironment _env = env;

        [HttpPost]
        public async Task<IActionResult> UploadLogo(IFormFile logoFile)
        {
            if (logoFile != null && logoFile.Length > 0)
            {
                string imageFolder = Path.Combine(_env.WebRootPath, "images");

                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                string logoPath = Path.Combine(imageFolder, "logo.jpg");

                // Delete old logo
                if (System.IO.File.Exists(logoPath))
                    System.IO.File.Delete(logoPath);

                using (var stream = new FileStream(logoPath, FileMode.Create))
                {
                    await logoFile.CopyToAsync(stream);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}