using DAL.Models;
using DAL.Repository.Interfaces;

namespace BLL.Services
{
    public class PurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepo;

        public PurchaseService(IPurchaseRepository purchaseRepo)
        {
            _purchaseRepo = purchaseRepo;
        }

        public void AddPurchase(Purchase purchase)
        {
            if (purchase.SupplierId <= 0)
                throw new Exception("Select supplier.");

            purchase.PurchaseDate = DateTime.UtcNow;

            _purchaseRepo.Add(purchase);
        }

        public Purchase GetPurchase(int id)
        {
            return _purchaseRepo.GetById(id);
        }

        public IEnumerable<Purchase> GetAllPurchases()
        {
            return _purchaseRepo.GetAll();
        }
    }
}
