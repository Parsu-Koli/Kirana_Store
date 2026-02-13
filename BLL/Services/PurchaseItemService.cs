using DAL.Models;
using DAL.Repository.Interface;
using DAL.Repository.Interfaces;

namespace BLL.Services
{
    public class PurchaseItemService
    {
        private readonly IPurchaseItemRepository _purchaseItemRepo;
        private readonly IStockRepository _stockRepo;

        public PurchaseItemService(IPurchaseItemRepository purchaseItemRepo,
                                   IStockRepository stockRepo)
        {
            _purchaseItemRepo = purchaseItemRepo;
            _stockRepo = stockRepo;
        }

        public IEnumerable<PurchaseItem> GetAllPurchaseitem()
        {
            return _purchaseItemRepo.GetAll();
        }

        public void AddPurchaseItem(PurchaseItem item)
        {
            if (item.PurchaseId <= 0)
                throw new Exception("Invalid Purchase.");

            if (item.ProductId <= 0)
                throw new Exception("Select product.");

            if (item.Quantity <= 0)
                throw new Exception("Quantity must be greater than 0.");

            _purchaseItemRepo.Add(item);

            _stockRepo.IncreaseStock(item.ProductId, item.Quantity);
        }

        public IEnumerable<PurchaseItem> GetItemsByPurchase(int purchaseId)
        {
            return _purchaseItemRepo.GetItemsByPurchase(purchaseId);
        }
    }
}
