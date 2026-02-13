using DAL.Models;
using DAL.Repository.Interface;


namespace BLL.Services
{
    public class OrderItemService(IOrderItemRepository orderItemRepo,
                            IStockRepository stockRepo)
    {
        private readonly IOrderItemRepository _orderItemRepo = orderItemRepo;
        private readonly IStockRepository _stockRepo = stockRepo;

        public void AddOrderItem(OrderItem item)
        {
            if (item.OrderId <= 0)
                throw new Exception("Invalid order.");

            if (item.ProductId <= 0)
                throw new Exception("Invalid product.");

            if (item.Quantity <= 0)
                throw new Exception("Invalid quantity.");

            using var transaction = _stockRepo.BeginTransaction();

            try
            {
                var stock = _stockRepo.GetByProductId(item.ProductId);

                if (stock == null)
                    throw new Exception("Stock not found.");

                if (stock.Quantity < item.Quantity)
                    throw new Exception("Insufficient stock.");

                _orderItemRepo.Add(item);
                _stockRepo.DecreaseStock(item.ProductId, item.Quantity);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public IEnumerable<OrderItem> GetItemsByOrder(int orderId)
        {
            return _orderItemRepo.GetItemsByOrder(orderId);
        }

        public IEnumerable<OrderItem> GetAllOrderItems()
        {
            return _orderItemRepo.GetAll();
        }
    }
}
