using DAL.Models;
using DAL.Repository.Interface;

namespace BLL.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepo;

        public PaymentService(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public void AddPayment(Payment payment)
        {
            if (payment.SaleId <= 0)
                throw new Exception("Invalid sale selected.");

            if (payment.AmountPaid <= 0)
                throw new Exception("Invalid payment amount.");

            payment.PaymentDate = DateTime.Now;

            _paymentRepo.Add(payment);
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            return _paymentRepo.GetAll();
        }
    }
}
