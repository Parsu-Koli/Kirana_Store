using DAL.Models;
using DAL.Repository.Interface;
namespace BLL.Services
{
    public class AuthService(IAuthRepository authRepo)
    {
        private readonly IAuthRepository _authRepo = authRepo;

        public User Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Username is required.");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required.");

            var user = _authRepo.Login(username, password);

            if (user == null)
                throw new Exception("Invalid Username or Password.");

            return user;
        }

        public bool IsUsernameExists(string username)
        {
            return _authRepo.IsUsernameExists(username);
        }

        public void CheckDuplicateUser(string username)
        {
            if (_authRepo.IsUsernameExists(username))
                throw new Exception("Username already exists.");
        }

        public User Register(User user)
        {
            if (_authRepo.IsUsernameExists(user.Username))
                throw new Exception("Username already exists.");

            user.CreatedDate = DateTime.Now;
            user.IsActive = true;

            _authRepo.AddUser(user);

            return user;
        }
    }
}
