using DAL.Data;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository.Implementation
{
    public class SaleRepository(AppDbContext context) : ISaleRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(Sale sale)
        {
            _context.Sales.Add(sale);
            _context.SaveChanges();
        }

        public Sale GetById(int id)
        {
            var sale = _context.Sales.Find(id);

            if (sale != null)
                LoadSaleItems(sale);   // Auto-load sale items

            return sale;
        }

        public IEnumerable<Sale> GetAll()
        {
            return _context.Sales
                           .OrderByDescending(s => s.SaleId)
                           .ToList();
        }

        public IEnumerable<Sale> GetByDate(DateTime date)
        {
            return _context.Sales
                           .Where(x => x.SaleDate.Date == date.Date)
                           .OrderByDescending(x => x.SaleId)
                           .ToList();
        }

        
        public void LoadSaleItems(Sale sale)
        {
            _context.Entry(sale)
                    .Collection(s => s.SaleItems)
                    .Load();
        }

        public void UpdateSale(Sale sale)
        {
            var existingSale = _context.Sales
                .Include(s => s.SaleItems)
                .FirstOrDefault(s => s.SaleId == sale.SaleId);

            if (existingSale == null)
                throw new Exception("Sale not found");

            // 🔹 Update Sale fields
            existingSale.InvoiceNumber = sale.InvoiceNumber;
            existingSale.CustomerId = sale.CustomerId;
            existingSale.TotalAmount = sale.TotalAmount;
            existingSale.Discount = sale.Discount;
            existingSale.NetAmount = sale.NetAmount;
            existingSale.PaymentMode = sale.PaymentMode;
            existingSale.SaleDate = sale.SaleDate;

            // 🔹 Handle SaleItems
            UpdateSaleItems(existingSale, sale.SaleItems);

            _context.SaveChanges();
        }

        private void UpdateSaleItems(Sale existingSale, List<SaleItem> newItems)
        {
            // 1️⃣ DELETE removed items
            var removedItems = existingSale.SaleItems
                .Where(dbItem => !newItems.Any(i => i.SaleItemId == dbItem.SaleItemId))
                .ToList();

            _context.SaleItems.RemoveRange(removedItems);

            // 2️⃣ ADD or UPDATE items
            foreach (var item in newItems)
            {
                var existingItem = existingSale.SaleItems
                    .FirstOrDefault(i => i.SaleItemId == item.SaleItemId);

                if (existingItem == null)
                {
                    // ➕ NEW ITEM
                    existingSale.SaleItems.Add(new SaleItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.Total
                    });
                }
                else
                {
                    // ✏️ UPDATE ITEM
                    existingItem.ProductId = item.ProductId;
                    existingItem.Quantity = item.Quantity;
                    existingItem.Price = item.Price;
                    existingItem.Total = item.Total;
                }
            }
        }
        public void Delete(int id)
        {
            var sale = _context.Sales
                               .Include(s => s.SaleItems) // important
                               .FirstOrDefault(s => s.SaleId == id);

            if (sale == null)
                throw new Exception("Sale not found");

            // ❌ First remove child records (SaleItems)
            _context.SaleItems.RemoveRange(sale.SaleItems);

            // ❌ Then remove Sale
            _context.Sales.Remove(sale);

            _context.SaveChanges();
        }


    }
}
