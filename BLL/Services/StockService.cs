using DAL.Models;
using DAL.Repository.Interface;

namespace BLL.Services
{
    public class StockService
    {
        private readonly IStockRepository _stockRepo;

        public StockService(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public void IncreaseStock(int productId, decimal qty)
        {
            if (productId <= 0)
                throw new Exception("Invalid Product.");

            if (qty <= 0)
                throw new Exception("Quantity must be greater than zero.");

            _stockRepo.IncreaseStock(productId, qty);
        }

        public void DecreaseStock(int productId, decimal qty)
        {
            if (productId <= 0)
                throw new Exception("Invalid Product.");

            if (qty <= 0)
                throw new Exception("Enter a valid quantity.");

            var productStock = _stockRepo.GetByProductId(productId);

            if (productStock == null)
                throw new Exception("Stock does not exist.");

            if (productStock.Quantity < qty)
                throw new Exception("Insufficient stock.");

            _stockRepo.DecreaseStock(productId, qty);
        }

        public IEnumerable<Stock> GetLowStock(int limitQty)
        {
            return _stockRepo.GetLowStock(limitQty);
        }

        public IEnumerable<Stock> GetAllStock()
        {
            return _stockRepo.GetAll();
        }

        public Stock GetStockByProductId(int productId)
        {
            return _stockRepo.GetByProductId(productId);
        }
    }
}
