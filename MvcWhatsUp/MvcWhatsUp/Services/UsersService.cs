using MvcWhatsUp.Models;
using MvcWhatsUp.Repositories;
using System.Security.Cryptography;

namespace MvcWhatsUp.Services
{
    public class UsersService : IUsersService
    {
        //fields and properties
        private readonly IUsersRepository _usersRepository;

        //constructors
        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        //methods
        public List<User> GetAll()
        {
            return _usersRepository.GetAll();
        }

        public User? GetById(int userId)
        {
            return _usersRepository.GetById(userId);
        }

        public User? GetByLoginCredentials(string username, string password)
        {
            password = HashPassword(password);

            return _usersRepository.GetByLoginCredentials(username, password);
        }

        public void Add(User user)
        {
            //make sure email address is unique
            if (_usersRepository.EmailAddressExists(user.EmailAddress))
            {
                throw new Exception("Email address already in use.");
            }

            //hash the password and store it
            user.Password = HashPassword(user.Password);

            _usersRepository.Add(user);
        }

        public void Update(User user)
        {
            //make sure email address is unique
            if (_usersRepository.EmailAddressExists(user.EmailAddress))
            {
                throw new Exception("Email address already in use.");
            }

            //hash the password and store it
            

            _usersRepository.Update(user);
        }

        public void Delete(User user)
        {
            user.Password = HashPassword(user.Password);

            _usersRepository.Delete(user);
        }

        //this hashes the password and returns the now hashed password
        private string HashPassword(string password)
        {
            using(SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
