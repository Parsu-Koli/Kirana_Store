using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IPaymentRepository
    {
        Payment GetById(int id);
        IEnumerable<Payment> GetPaymentsBySale(int saleId);
        IEnumerable<Payment> GetAll();
        void Add(Payment payment);
    }
}
