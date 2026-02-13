using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IOrderRepository
    {
        Order GetById(int id);
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetOrdersByDate(DateTime date);
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
    }
}
