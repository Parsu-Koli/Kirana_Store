using DAL.Data;
using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository.Implementation
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        // 🔥 ADD THIS METHOD
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Add(Stock stock)
        {
            _context.Stocks.Add(stock);
            _context.SaveChanges();
        }

        public void Update(Stock stock)
        {
            _context.Stocks.Update(stock);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = _context.Stocks.Find(id);
            if (obj != null)
            {
                _context.Stocks.Remove(obj);
                _context.SaveChanges();
            }
        }

        public Stock GetById(int id)
        {
            return _context.Stocks.Find(id);
        }

        public Stock GetByProductId(int productId)
        {
            return _context.Stocks.FirstOrDefault(x => x.ProductId == productId);
        }

        public IEnumerable<Stock> GetAll()
        {
            return _context.Stocks.
                Include(p=>p.Product)
                .ToList();
        }

        public IEnumerable<Stock> GetLowStock(decimal limit)
        {
            return _context.Stocks
                           .Where(x => x.Quantity <= x.MinimumQuantity)
                           .ToList();
        }


        public void DecreaseStock(int productId, decimal qty)
        {
            var stock = _context.Stocks.FirstOrDefault(x => x.ProductId == productId);
            if (stock == null)
                throw new Exception("Stock not found");

            if (qty <= 0)
                throw new Exception("Quantity must be greater than zero");

            if (stock.Quantity < qty)
                throw new Exception("Insufficient stock");

            // 🔻 Decrease stock
            stock.Quantity -= qty;

            // 🔻 Update product quantity
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                product.QuantityInStock -= qty;

                // Safety check
                if (product.QuantityInStock < 0)
                    product.QuantityInStock = 0;
            }

            // ❌ Remove stock record if quantity becomes zero
            if (stock.Quantity <= 0)
            {
                _context.Stocks.Remove(stock);
            }

            _context.SaveChanges();
        }



        public void IncreaseStock(int productId, decimal qty)
        {
            var stock = _context.Stocks.FirstOrDefault(x => x.ProductId == productId);

            if (stock == null)
            {
                // Create new stock entry if it doesn't exist
                stock = new Stock
                {
                    ProductId = productId,
                    Quantity = qty,
                    MinimumQuantity = 0
                };
                _context.Stocks.Add(stock);
            }
            else
            {
                stock.Quantity += qty;
            }

            // Update product quantity as well
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                product.QuantityInStock += qty;
            }

            _context.SaveChanges();
        }

    }
}
