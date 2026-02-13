using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IOrderItemRepository
    {
        IEnumerable<OrderItem> GetItemsByOrder(int orderId);
        void Add(OrderItem item);
        IEnumerable<OrderItem> GetAll();
    }
}
