using DAL.Data;
using DAL.Models;
using DAL.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public Payment GetById(int id)
        {
            return _context.Payments.Find(id);
        }

        public IEnumerable<Payment> GetAll()
        {
            return [.. _context.Payments];
        }

        public IEnumerable<Payment> GetPaymentsBySale(int saleId)
        {
            return [.. _context.Payments.Where(x => x.SaleId == saleId)];
        }
    }
}
