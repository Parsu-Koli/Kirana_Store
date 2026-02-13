using DAL.Data;
using DAL.Models;
using DAL.Repository.Interface;
using DAL.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository.Implimentation
{
    public class OrderItemRepository(AppDbContext context) : IOrderItemRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(OrderItem item)
        {
            _context.OrderItems.Add(item);
            _context.SaveChanges();
        }

        public IEnumerable<OrderItem> GetAll()
        {
            return [.. _context.OrderItems];
        }

        public IEnumerable<OrderItem> GetItemsByOrder(int orderId)
        {
            return [.. _context.OrderItems.Where(x => x.OrderId == orderId)];
        }
    }
}
