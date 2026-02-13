using DAL.Data;
using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository.Implimentation
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public Order GetById(int id)
        {
            return _context.Orders
                .Include(x => x.Items)
                .FirstOrDefault(x => x.OrderId == id);
        }

        public IEnumerable<Order> GetAll()
        {
            return [.. _context.Orders];
        }
            
        public IEnumerable<Order> GetOrdersByDate(DateTime date)
        {
            return [.. _context.Orders.Where(x => x.OrderDate.Date == date.Date)];
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = _context.Orders.Find(id);
            if (obj != null)
            {
                _context.Orders.Remove(obj);
                _context.SaveChanges();
            }
        }
    }
}
