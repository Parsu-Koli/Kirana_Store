using DAL.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Repository.Interface
{
    public interface IStockRepository
    {
        Stock GetById(int id);
        IEnumerable<Stock> GetAll();
        IEnumerable<Stock> GetLowStock(decimal limit);

        void Add(Stock stock);
        void Update(Stock stock);
        void Delete(int id);

        // Stock Management
        void IncreaseStock(int productId, decimal qty);
        void DecreaseStock(int productId, decimal qty);
        Stock GetByProductId(int productId);
        IDbContextTransaction BeginTransaction();

    }
}
