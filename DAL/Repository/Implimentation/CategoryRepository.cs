using DAL.Data;
using DAL.Models;
using DAL.Repository.Interface;


namespace DAL.Repository.Implimentation
{
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        private readonly AppDbContext _context = context;

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = _context.Categories.Find(id);
            if (obj != null)
            {
                _context.Categories.Remove(obj);
                _context.SaveChanges();
            }
        }

        public Category GetById(int id)
        {
            return _context.Categories.Find(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return [.. _context.Categories];
        }

        public bool IsCategoryNameExists(string name)
        {
            return _context.Categories.Any(x => x.CategoryName == name);
        }


        public List<Product> GetProductsByCategory(int id)
        {
            return [.. _context.Products.Where(p => p.CategoryId == id)];
        }
    }
}
