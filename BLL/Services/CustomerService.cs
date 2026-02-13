using DAL.Models;
using DAL.Repository.Interface;
namespace BLL.Services
{
    public class CustomerService(ICustomerRepository custRepo)
    {
        private readonly ICustomerRepository _custRepo = custRepo;

        public void AddCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new Exception("Customer name required.");

            if (string.IsNullOrWhiteSpace(customer.Phone))
                throw new Exception("Customer mobile required.");

            _custRepo.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer.CustomerId <= 0)
                throw new Exception("Invalid Customer ID.");

            _custRepo.Update(customer);
        }

        public void DeleteCustomer(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid ID.");

            _custRepo.Delete(id);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _custRepo.GetAll();
        }

        public Customer GetCustomer(int id)
        {
            return _custRepo.GetById(id);
        }
    }
}
