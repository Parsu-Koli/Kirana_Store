using DAL.Models;
using DAL.Repository.Interface;
using DAL.Repository.Interfaces;

namespace BLL.Services
{
    public class SaleItemService
    {
        private readonly ISaleItemRepository _saleItemRepo;
        private readonly IStockRepository _stockRepo;

        public SaleItemService(ISaleItemRepository saleItemRepo,
                               IStockRepository stockRepo)
        {
            _saleItemRepo = saleItemRepo;
            _stockRepo = stockRepo;
        }

        public void AddSaleItem(SaleItem item)
        {
            if (item.SaleId <= 0)
                throw new Exception("Invalid sale.");

            if (item.ProductId <= 0)
                throw new Exception("Invalid product.");

            if (item.Quantity <= 0)
                throw new Exception("Invalid quantity.");

            var stock = _stockRepo.GetByProductId(item.ProductId);

            if (stock == null)
            {

                stock = new Stock
                {
                    ProductId = item.ProductId,
                    Quantity = 0
                };
            }

            _saleItemRepo.Add(item);

            _stockRepo.DecreaseStock(item.ProductId, item.Quantity);
        }

        public IEnumerable<SaleItem> GetItemsBySale(int saleId)
        {
            return _saleItemRepo.GetItemsBySale(saleId);
        }

        public IEnumerable<SaleItem> GetAllSaleItems()
        {
            return _saleItemRepo.GetAll();
        }
    }
}


//using DAL.Models;
//using DAL.Repository.Interfaces;
//using DAL.DTO;

//namespace BLL.Services
//{
//    public class SaleItemService
//    {
//        private readonly ISaleItemRepository _saleItemRepo;
//        private readonly IStockRepository _stockRepo;
//        private readonly IProductRepository _productRepo;

//        public SaleItemService(
//            ISaleItemRepository saleItemRepo,
//            IStockRepository stockRepo,
//            IProductRepository productRepo)
//        {
//            _saleItemRepo = saleItemRepo;
//            _stockRepo = stockRepo;
//            _productRepo = productRepo;
//        }

//        public void AddSaleItem(SaleItem item)
//        {
//            if (item.Quantity <= 0)
//                throw new Exception("Invalid quantity.");

//            _saleItemRepo.Add(item);

//            _stockRepo.DecreaseStock(item.ProductId, item.Quantity);
//        }

//        public List<SaleItemDto> GetItemsBySale(int saleId)
//        {
//            var items = _saleItemRepo.GetItemsBySale(saleId).ToList();

//            var dto = items.Select(i => new SaleItemDto
//            {
//                SaleItemId = i.SaleItemId,
//                SaleId = i.SaleId,
//                ProductId = i.ProductId,
//                ProductName = _productRepo.GetById(i.ProductId)?.ProductName ?? "Unknown",
//                Quantity = i.Quantity,
//                Rate = i.Price,
//                Amount = i.Total
//            })
//            .ToList();

//            return dto;
//        }
//    }
//}
