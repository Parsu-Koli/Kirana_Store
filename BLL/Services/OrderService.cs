using DAL.Models;
using DAL.Repository.Interface;

namespace BLL.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepo;

        public OrderService(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public void AddOrder(Order order)
        {
            //order.OrderDate = DateTime.Now;

            _orderRepo.Add(order);
        }

        public Order GetOrder(int id)
        {
            return _orderRepo.GetById(id);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepo.GetAll();
        }

        public IEnumerable<Order> GetOrdersByDate(DateTime date)
        {
            return _orderRepo.GetOrdersByDate(date);
        }

        public void UpdateOrder(Order order)
        {
            if (order.OrderId <= 0)
                throw new Exception("Invalid Order ID.");

            _orderRepo.Update(order);
        }

        public void DeleteOrder(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid ID.");

            _orderRepo.Delete(id);
        }
    }
}
