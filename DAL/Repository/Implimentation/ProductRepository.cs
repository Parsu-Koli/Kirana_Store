using DAL.Data;
using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository.Implimentation
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = _context.Products.Find(id);
            if (obj != null)
            {
                _context.Products.Remove(obj);
                _context.SaveChanges();
            }
        }

        // ----------------------- GET BY ID (Category Name Included) -----------------------
        public Product GetById(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);

            if (product != null)
                product.CategoryName = product.Category?.CategoryName;

            return product;
        }


        public IEnumerable<Product> GetAll()
        {
            var list = _context.Products
                .Include(p => p.Category)
                .ToList();

            foreach (var p in list)
            {
                p.CategoryName = p.Category?.CategoryName;
            }

            return list;
        }


        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            var list = _context.Products
                .Include(p => p.Category)
                .Where(x => x.CategoryId == categoryId)
                .ToList();

            foreach (var p in list)
            {
                p.CategoryName = p.Category?.CategoryName;
            }

            return list;
        }

        public bool IsProductNameExists(string name)
        {
            return _context.Products.Any(x => x.ProductName == name);
        }

        public void DecreaseProductStock(int productId, decimal qty)
        {
            var product = _context.Products.Find(productId);

            if (product == null)
                throw new Exception("Product not found.");

            if (product.QuantityInStock < qty)
                throw new Exception("Insufficient product stock.");

            product.QuantityInStock -= qty;
            _context.SaveChanges();
        }

        public void IncreaseProductStock(int productId, decimal qty)
        {
            var product = _context.Products.Find(productId);

            if (product == null)
                throw new Exception("Product not found.");

            product.QuantityInStock += qty;
            _context.SaveChanges();
        }



    }
}
