using KiranaStoreUI.Models;
using KiranaStoreUI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KiranaStoreUI.Controllers
{
    public class SaleController(IHttpClientFactory factory) : Controller
    {
        private readonly HttpClient _client = factory.CreateClient("api");

        // ---------------- JWT Helper ----------------
        private void AddJwtToken()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ---------------- INDEX ----------------
        public async Task<IActionResult> Index()
        {
            AddJwtToken();

            var sales = await _client.GetFromJsonAsync<List<Sale>>("Sale/GetAllSales");
            var customers = await _client.GetFromJsonAsync<List<Customer>>("Customer/GetCustomers");

            var customerDict = customers.ToDictionary(c => c.CustomerId, c => c.Name);

            foreach (var s in sales)
            {
                s.Customer = new Customer
                {
                    Name = s.CustomerId.HasValue && customerDict.ContainsKey(s.CustomerId.Value)
                           ? customerDict[s.CustomerId.Value]
                           : "N/A"
                };
            }

            return View(sales);
        }

        // ---------------- CREATE ----------------
        public async Task<IActionResult> Create()
        {
            AddJwtToken();
            string nextInvoice = await _client.GetStringAsync("Sale/GetNextInvoice");

            var vm = new SaleCustomerVM
            {
                Sale = new Sale
                {
                    InvoiceNumber = nextInvoice,
                    SaleDate = DateTime.Now,
                    SaleItems = new List<SaleItem>()
                },
                Customer = new Customer()
            };

            ViewBag.NextInvoice = nextInvoice;
            return View(vm);
        }

        // [HttpPost]
        // public async Task<IActionResult> Create(SaleCustomerVM vm)
        // {
        //     AddJwtToken();

        //     if (vm.Sale == null || vm.Customer == null)
        //     {
        //         ModelState.AddModelError("", "Sale or Customer data is missing.");
        //         return View(vm);
        //     }

        //     // 1️⃣ Create Customer
        //     var custResponse = await _client.PostAsJsonAsync("Customer/AddCustomer", vm.Customer);
        //     if (!custResponse.IsSuccessStatusCode)
        //     {
        //         ModelState.AddModelError("", "Customer creation failed.");
        //         ViewBag.NextInvoice = vm.Sale.InvoiceNumber;
        //         return View(vm);
        //     }

        //     var createdCustomer = await custResponse.Content.ReadFromJsonAsync<Customer>();
        //     if (createdCustomer == null || createdCustomer.CustomerId <= 0)
        //     {
        //         ModelState.AddModelError("", "Invalid customer data returned from server.");
        //         ViewBag.NextInvoice = vm.Sale.InvoiceNumber;
        //         return View(vm);
        //     }

        //     vm.Sale.CustomerId = createdCustomer.CustomerId;

        //     // 🔥 2️⃣ LOAD PRODUCTS ONCE
        //     var products = await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");

        //     decimal totalAmount = 0;

        //     if (vm.Sale.SaleItems != null)
        //     {
        //         foreach (var item in vm.Sale.SaleItems)
        //         {
        //             var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
        //             if (product == null) continue;

        //             // ✅ AUTO PRICE FROM PRODUCT
        //             item.Price = product.SellingPrice;

        //             // ✅ AUTO TOTAL = Quantity × Price
        //             item.Total = item.Quantity * item.Price;

        //             totalAmount += item.Total;

        //             // prevent circular ref
        //             item.Product = null;
        //             item.Sale = null;
        //         }
        //     }

        //     // ✅ SET BILL TOTALS
        //     vm.Sale.TotalAmount = totalAmount;
        //     vm.Sale.NetAmount = totalAmount - vm.Sale.Discount;
        //     vm.Sale.SaleDate = DateTime.UtcNow;

        //     // 3️⃣ Create Sale
        //     var saleResponse = await _client.PostAsJsonAsync("Sale/AddSale", vm.Sale);
        //     if (saleResponse.IsSuccessStatusCode)
        //     {
        //         TempData["SuccessMsg"] =
        //             $"Sale created successfully! Invoice: {vm.Sale.InvoiceNumber}";
        //         return RedirectToAction("Index");
        //     }

        //     ModelState.AddModelError("", "Sale creation failed.");
        //     ViewBag.NextInvoice = vm.Sale.InvoiceNumber;
        //     return View(vm);
        // }

        [HttpPost]
public async Task<IActionResult> Create(SaleCustomerVM vm)
{
    AddJwtToken();

    // 🔥 Reload invoice if missing
    if (string.IsNullOrEmpty(vm.Sale.InvoiceNumber))
    {
        vm.Sale.InvoiceNumber =
            await _client.GetStringAsync("Sale/GetNextInvoice");
    }

    // 🔥 ALWAYS load products
    var products =
        await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");

    // 🔥 VALIDATION
    if (vm.Sale == null || vm.Customer == null)
    {
        ModelState.AddModelError("", "Sale or Customer data missing.");
        ViewBag.NextInvoice = vm.Sale?.InvoiceNumber;

        await ReloadProducts(vm);
        return View(vm);
    }

    // 🔥 FIX PRODUCT ITEMS
    decimal totalAmount = 0;

    if (vm.Sale.SaleItems != null)
    {
        foreach (var item in vm.Sale.SaleItems)
        {
            var product =
                products.FirstOrDefault(p => p.ProductId == item.ProductId);

            if (product == null)
                continue;

            // ✅ AUTO PRICE
            item.Price = product.SellingPrice;

            // ✅ TOTAL
            item.Total = item.Quantity * item.Price;

            totalAmount += item.Total;

            // ✅ REMOVE CIRCULAR REFERENCES
            item.Product = null;
            item.Sale = null;
        }
    }

    // 🔥 SALE TOTALS
    vm.Sale.TotalAmount = totalAmount;
    vm.Sale.NetAmount = totalAmount - vm.Sale.Discount;
    // vm.Sale.SaleDate = DateTime.Now;
    vm.Sale.SaleDate = DateTime.UtcNow;

    // =====================================================
    // 1️⃣ CREATE CUSTOMER
    // =====================================================

    var custResponse =
        await _client.PostAsJsonAsync("Customer/AddCustomer", vm.Customer);

    if (!custResponse.IsSuccessStatusCode)
    {
        ModelState.AddModelError("", "Customer creation failed.");

        ViewBag.NextInvoice = vm.Sale.InvoiceNumber;

        await ReloadProducts(vm);
        return View(vm);
    }

    var createdCustomer =
        await custResponse.Content.ReadFromJsonAsync<Customer>();

    if (createdCustomer == null)
    {
        ModelState.AddModelError("", "Customer response invalid.");

        ViewBag.NextInvoice = vm.Sale.InvoiceNumber;

        await ReloadProducts(vm);
        return View(vm);
    }

    // 🔥 ASSIGN CUSTOMER ID
    vm.Sale.CustomerId = createdCustomer.CustomerId;

    // =====================================================
    // 2️⃣ CREATE SALE
    // =====================================================

    var saleResponse =
        await _client.PostAsJsonAsync("Sale/AddSale", vm.Sale);

    if (saleResponse.IsSuccessStatusCode)
    {
        TempData["SuccessMsg"] =
            $"Sale created successfully! Invoice: {vm.Sale.InvoiceNumber}";

        return RedirectToAction("Index");
    }

    // 🔥 READ ACTUAL ERROR FROM API
    var errorMessage = await saleResponse.Content.ReadAsStringAsync();

    ModelState.AddModelError("",
        $"Sale creation failed : {errorMessage}");

    ViewBag.NextInvoice = vm.Sale.InvoiceNumber;

    // 🔥 RELOAD PRODUCTS AGAIN
    await ReloadProducts(vm);

    return View(vm);
}


        // ---------------- SEARCH PRODUCT ----------------
        [HttpGet]
        public async Task<JsonResult> SearchProduct(string keyword)
        {
            AddJwtToken();

            if (string.IsNullOrWhiteSpace(keyword))
                return Json(new { success = false, products = new List<object>() });

            var result = await _client.GetAsync($"Sale/SearchProducts?keyword={keyword}");
            if (!result.IsSuccessStatusCode)
                return Json(new { success = false });

            var products = await result.Content.ReadFromJsonAsync<List<Product>>();
            return Json(new { success = true, products });
        }

        // ---------------- DETAILS ----------------
        public async Task<IActionResult> Details(int id)
        {
            AddJwtToken();

            var sale = await _client.GetFromJsonAsync<Sale>($"Sale/GetSale/{id}");
            if (sale == null) return NotFound();

            if (sale.SaleItems != null && sale.SaleItems.Count > 0)
            {
                var productIds = sale.SaleItems.Select(si => si.ProductId).ToList();
                var products = await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");
                var productDict = products.Where(p => productIds.Contains(p.ProductId))
                                          .ToDictionary(p => p.ProductId, p => p);

                foreach (var item in sale.SaleItems)
                    if (productDict.ContainsKey(item.ProductId))
                        item.Product = productDict[item.ProductId];
            }

            // Calculate date-wise total
            var allSales = await _client.GetFromJsonAsync<List<Sale>>("Sale/GetAllSales");
            var selectedDate = sale.SaleDate.Date;
            decimal dateWiseTotal = allSales
                .Where(s => s.SaleDate.Date == selectedDate)
                .Sum(s => s.NetAmount);

            ViewBag.DateWiseTotalNetAmount = dateWiseTotal;
            ViewBag.SaleDate = selectedDate.ToString("dd-MMM-yyyy");

            return View(sale);
        }

        // ---------------- EDIT ----------------
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AddJwtToken();

            // 1️⃣ Get Sale
            var sale = await _client.GetFromJsonAsync<Sale>($"Sale/GetSale/{id}");
            if (sale == null) return NotFound();

            // 2️⃣ Get related Customer
            Customer customer = new Customer();
            if (sale.CustomerId.HasValue)
            {
                var custResp = await _client.GetFromJsonAsync<Customer>($"Customer/GetCustomer/{sale.CustomerId.Value}");
                if (custResp != null)
                    customer = custResp;
            }

            // 3️⃣ Load products for SaleItems
            var products = await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");
            var productDict = products.ToDictionary(p => p.ProductId);

            if (sale.SaleItems != null)
            {
                foreach (var item in sale.SaleItems)
                {
                    if (productDict.TryGetValue(item.ProductId, out var prod))
                        item.Product = prod;
                }
            }
            else
            {
                sale.SaleItems = new List<SaleItem>();
            }

            var vm = new SaleCustomerVM
            {
                Sale = sale,
                Customer = customer
            };

            return View(vm);
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(SaleCustomerVM vm)
        //{
        //    AddJwtToken();

        //    // ✅ FIX 1: If validation fails, reload products for view
        //    if (!ModelState.IsValid)
        //    {
        //        // reload products so ProductName doesn't break in view
        //        var products = await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");
        //        var productDict = products.ToDictionary(p => p.ProductId);

        //        if (vm.Sale?.SaleItems != null)
        //        {
        //            foreach (var item in vm.Sale.SaleItems)
        //            {
        //                if (productDict.TryGetValue(item.ProductId, out var prod))
        //                    item.Product = prod;
        //            }
        //        }

        //        return View(vm);
        //    }

        //    // 🔁 SAME LOGIC: prevent circular reference
        //    if (vm.Sale.SaleItems != null)
        //    {
        //        foreach (var item in vm.Sale.SaleItems)
        //            item.Sale = null;
        //    }

        //    // 1️⃣ SAME LOGIC: Update Customer
        //    var customerResponse =
        //        await _client.PutAsJsonAsync("Customer/UpdateCustomer", vm.Customer);

        //    if (!customerResponse.IsSuccessStatusCode)
        //    {
        //        ModelState.AddModelError("", "Failed to update customer.");
        //        return View(vm);
        //    }

        //    // 2️⃣ SAME LOGIC: Assign CustomerId
        //    vm.Sale.CustomerId = vm.Customer.CustomerId;

        //    // 3️⃣ SAME LOGIC: Recalculate NetAmount
        //    vm.Sale.NetAmount = vm.Sale.TotalAmount - vm.Sale.Discount;

        //    // 4️⃣ SAME LOGIC: Update Sale
        //    var saleResponse =
        //        await _client.PutAsJsonAsync("Sale/UpdateSale", vm.Sale);

        //    if (saleResponse.IsSuccessStatusCode)
        //        return RedirectToAction("Index");

        //    ModelState.AddModelError("", "Failed to update sale.");
        //    return View(vm);
        //}


        [HttpPost]
        public async Task<IActionResult> Edit(SaleCustomerVM vm)
        {
            AddJwtToken();

            // 🔥 FIX 1: Assign CustomerId BEFORE validation
            vm.Sale.CustomerId = vm.Customer.CustomerId;

            // 🔥 FIX 2: Validation AFTER fixing required fields
            if (!ModelState.IsValid)
            {
                await ReloadProducts(vm);
                return View(vm);
            }

            // 🔁 Prevent circular refs
            if (vm.Sale.SaleItems != null)
            {
                foreach (var item in vm.Sale.SaleItems)
                    item.Sale = null;
            }

            // 1️⃣ Update Customer
            var customerResponse =
                await _client.PutAsJsonAsync("Customer/UpdateCustomer", vm.Customer);

            if (!customerResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to update customer.");
                await ReloadProducts(vm);
                return View(vm);
            }

            // 2️⃣ Recalculate NetAmount
            vm.Sale.NetAmount = vm.Sale.TotalAmount - vm.Sale.Discount;

            // 3️⃣ Update Sale
            var saleResponse =
                await _client.PutAsJsonAsync("Sale/UpdateSale", vm.Sale);

            if (saleResponse.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Failed to update sale.");
            await ReloadProducts(vm);
            return View(vm);
        }



        public async Task<IActionResult> ProfitByDateRange(DateTime? startDate, DateTime? endDate)
        {
            AddJwtToken();

            if (!startDate.HasValue || !endDate.HasValue)
            {
                ViewBag.TotalProfit = 0;
                return View(new List<SaleProfitVM>());
            }

            DateTime start = startDate.Value.Date;
            DateTime end = endDate.Value.Date;

            // 1️⃣ Get all sales
            var allSales = await _client.GetFromJsonAsync<List<Sale>>("Sale/GetAllSales");

            // Filter by date range
            var filteredSales = allSales
                .Where(s => s.SaleDate.Date >= start && s.SaleDate.Date <= end)
                .ToList();

            // 2️⃣ Get all products
            var products = await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");
            var productDict = products.ToDictionary(p => p.ProductId);

            // 3️⃣ Build list of SaleProfitVM
            var profitList = new List<SaleProfitVM>();
            decimal totalProfit = 0;

            foreach (var sale in filteredSales)
            {
                decimal purchaseTotal = 0;
                decimal sellingTotal = 0;

                if (sale.SaleItems != null)
                {
                    foreach (var item in sale.SaleItems)
                    {
                        if (productDict.TryGetValue(item.ProductId, out var prod))
                        {
                            purchaseTotal += prod.PurchasePrice * item.Quantity;
                            sellingTotal += item.Price * item.Quantity;
                        }
                    }
                }

                var vm = new SaleProfitVM
                {
                    InvoiceNumber = sale.InvoiceNumber,
                    SaleDate = sale.SaleDate,
                    PurchasePrice = purchaseTotal,
                    SellingPrice = sellingTotal,
                    TotalDiscount = sale.Discount,   // ✅ assign discount
                    Total = sale.NetAmount
                };

                totalProfit += vm.Profit; // ✅ profit is calculated automatically
                profitList.Add(vm);
            }

            ViewBag.TotalProfit = totalProfit;
            ViewBag.StartDate = start.ToString("yyyy-MM-dd");
            ViewBag.EndDate = end.ToString("yyyy-MM-dd");

            return View(profitList);
        }


        private async Task ReloadProducts(SaleCustomerVM vm)
        {
            var products = await _client.GetFromJsonAsync<List<Product>>("Product/GetProducts");
            var productDict = products.ToDictionary(p => p.ProductId);

            if (vm.Sale?.SaleItems != null)
            {
                foreach (var item in vm.Sale.SaleItems)
                {
                    if (productDict.TryGetValue(item.ProductId, out var prod))
                        item.Product = prod;
                }
            }
        }


    }
}
