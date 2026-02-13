using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface ICustomerRepository
    {
        Customer GetById(int id);
        IEnumerable<Customer> GetAll();
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
    }
}
