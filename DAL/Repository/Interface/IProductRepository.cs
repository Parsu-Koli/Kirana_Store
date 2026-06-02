using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IProductRepository
    {
        Product GetById(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetByCategory(int categoryId);

        void Add(Product product);
        void Update(Product product);
        void Delete(int id);

        bool IsProductNameExists(string name);

        void DecreaseProductStock(int productId, decimal qty);
        void IncreaseProductStock(int productId, decimal qty);

        Task<Product?> GetProductByBarcodeAsync(string barcode);
    }
}
