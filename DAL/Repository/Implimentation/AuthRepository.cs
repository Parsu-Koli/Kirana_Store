using DAL.Data;
using DAL.Models;
using DAL.Repository.Interface;
namespace DAL.Repository.Implimentation
{
    public class AuthRepository(AppDbContext context) : IAuthRepository
    {
        private readonly AppDbContext _context = context;

        public User Login(string username, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public bool IsUsernameExists(string username)
        {
            return _context.Users.Any(x => x.Username == username);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            Console.WriteLine( user.UserId);
        }
    }
}
