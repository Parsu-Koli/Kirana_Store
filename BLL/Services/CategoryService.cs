using DAL.Models;
using DAL.Repository.Interface;

namespace BLL.Services
{
    public class CategoryService(ICategoryRepository categoryRepo)
    {
        private readonly ICategoryRepository _categoryRepo = categoryRepo;

        public void AddCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new Exception("Category name is mandatory.");

            if (_categoryRepo.IsCategoryNameExists(category.CategoryName))
                throw new Exception("Category already exists.");

            _categoryRepo.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            if (category.CategoryId <= 0)
                throw new Exception("Invalid category ID.");

            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new Exception("Category name required.");

            _categoryRepo.Update(category);
        }

        public void DeleteCategory(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid Category ID.");

            _categoryRepo.Delete(id);
        }

        public Category GetCategory(int id)
        {
            return _categoryRepo.GetById(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepo.GetAll();
        }

        public List<Product> GetProductById(int id)
        {
            return _categoryRepo.GetProductsByCategory(id);
        }
    }
}
