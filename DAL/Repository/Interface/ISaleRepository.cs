using DAL.Models;

namespace DAL.Repository.Interfaces
{
    public interface ISaleRepository
    {
        Sale GetById(int id);
        IEnumerable<Sale> GetAll();
        IEnumerable<Sale> GetByDate(DateTime date);
        void Add(Sale sale);

        IQueryable<Sale> GetSalesQueryable();
        void LoadSaleItems(Sale sale);
        void UpdateSale(Sale sale);

        void Delete(int id);
    }
}
