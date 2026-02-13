using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface ICategoryRepository
    {
        Category GetById(int id);
        IEnumerable<Category> GetAll();
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);

        List<Product> GetProductsByCategory(int Id);

        bool IsCategoryNameExists(string name);
    }
}
