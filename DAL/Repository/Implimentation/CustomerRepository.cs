using DAL.Data;
using DAL.Models;
using DAL.Repository.Interface;
namespace DAL.Repository.Implimentation
{
    public class CustomerRepository(AppDbContext context) : ICustomerRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = _context.Customers.Find(id);
            if (obj != null)
            {
                _context.Customers.Remove(obj);
                _context.SaveChanges();
            }
        }

        public Customer GetById(int id)
        {
            return _context.Customers.Find(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }
    }
}
