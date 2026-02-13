using DAL.Models;
using DAL.Repository.Interface;

namespace BLL.Services
{
    public class ProductService
    {
        private readonly IProductRepository _prodRepo;
        private readonly ICategoryRepository _catRepo;
        private readonly IStockRepository _stockRepo;

        public ProductService(IProductRepository prodRepo, ICategoryRepository catRepo, IStockRepository stockRepo)
        {
            _prodRepo = prodRepo;
            _catRepo = catRepo;
            _stockRepo = stockRepo;
        }

        public void AddProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.ProductName))
                throw new Exception("Product name required.");

            if (product.CategoryId <= 0)
                throw new Exception("Choose valid category.");

            if (_prodRepo.IsProductNameExists(product.ProductName))
                throw new Exception("Product already exists.");

            var category = _catRepo.GetById(product.CategoryId);
            if (category == null)
                throw new Exception("Invalid category. Category not found.");

            product.Category = category;

            _prodRepo.Add(product);

            var stock = new Stock
            {
                ProductId = product.ProductId,
                Quantity = product.QuantityInStock
            };

            _stockRepo.Add(stock);
        }

        public void UpdateProduct(Product product)
        {
            if (product.ProductId <= 0)
                throw new Exception("Invalid product ID.");

            var category = _catRepo.GetById(product.CategoryId);
            product.Category = category;

            _prodRepo.Update(product);
        }

        public void DeleteProduct(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid ID.");

            _prodRepo.Delete(id);
        }

        public Product GetProduct(int id)
        {
            return _prodRepo.GetById(id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _prodRepo.GetAll();
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return _prodRepo.GetByCategory(categoryId);
        }

        public IEnumerable<Product> SearchProducts(string keyword)
        {
            return _prodRepo.GetAll()
                .Where(p => p.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }
}
