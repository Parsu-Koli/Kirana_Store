using DAL.Models;
using DAL.Repository.Interface;
using DAL.Repository.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BLL.Services
{
    public class SaleService
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IProductRepository _productRepo;

        public SaleService(ISaleRepository saleRepo, IProductRepository productRepo)
        {
            _saleRepo = saleRepo;
            _productRepo = productRepo;
        }

        public void AddSale(Sale sale)
        {
            if (sale.CustomerId <= 0)
                throw new Exception("Select customer.");

            if (sale.SaleItems == null || sale.SaleItems.Count == 0)
                throw new Exception("Add at least one product.");

            sale.SaleDate = DateTime.UtcNow;
            sale.InvoiceNumber = GenerateInvoiceNumber();

            foreach (var item in sale.SaleItems)
            {
                var product = _productRepo.GetById(item.ProductId);
                if (product == null)
                    throw new Exception("Product not found.");

                if (product.QuantityInStock < item.Quantity)
                    throw new Exception("Insufficient stock.");

                product.QuantityInStock -= item.Quantity;
                _productRepo.Update(product);

                item.Sale = null;
            }

            _saleRepo.Add(sale);
        }

        public IEnumerable<Sale> GetAllSales()
        {
            var sales = _saleRepo.GetAll().ToList();

            foreach (var sale in sales)
            {
                _saleRepo.LoadSaleItems(sale);
                LoadProductsForSaleItems(sale);
            }

            return sales;
        }

        public Sale GetSale(int saleId)
        {
            var sale = _saleRepo.GetById(saleId);
            if (sale == null) return null;

            _saleRepo.LoadSaleItems(sale);
            LoadProductsForSaleItems(sale);

            return sale;
        }

        private void LoadProductsForSaleItems(Sale sale)
        {
            if (sale.SaleItems == null || sale.SaleItems.Count == 0) return;

            var productIds = sale.SaleItems.Select(si => si.ProductId).ToList();
            var products = _productRepo.GetAll()
                                       .Where(p => productIds.Contains(p.ProductId))
                                       .ToDictionary(p => p.ProductId, p => p);

            foreach (var item in sale.SaleItems)
            {
                if (products.ContainsKey(item.ProductId))
                    item.Product = products[item.ProductId];
            }
        }

        public string GetNextInvoiceNumber()
        {
            return GenerateInvoiceNumber();
        }

        private string GenerateInvoiceNumber()
        {
            var lastSale = _saleRepo.GetAll()
                                    .OrderByDescending(s => s.SaleId)
                                    .FirstOrDefault();

            if (lastSale == null || string.IsNullOrEmpty(lastSale.InvoiceNumber))
                return "INV000001";

            string numericPart = lastSale.InvoiceNumber.Substring(3);
            int lastNumber = int.Parse(numericPart);
            int newNumber = lastNumber + 1;

            return "INV" + newNumber.ToString("000000");
        }

        public void UpdateSale(Sale sale)
        {
            if (sale.SaleId <= 0)
                throw new Exception("Invalid category ID.");

            _saleRepo.UpdateSale(sale);

        }

        public void Delete(int id)
        {
            _saleRepo.Delete(id);

        }
    }
}
