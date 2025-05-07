using MvcWhatsUp.Models;

namespace MvcWhatsUp.Services
{
    public interface IUsersService
    {
        //fields and properties

        //constructors

        //methods
        List<User> GetAll();

        User? GetById(int userId);

        User? GetByLoginCredentials(string username, string password);

        void Add(User user);

        void Update(User user);

        void Delete(User user);
    }
}
