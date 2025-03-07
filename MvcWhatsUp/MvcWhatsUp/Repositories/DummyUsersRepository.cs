using MvcWhatsUp.Models;

namespace MvcWhatsUp.Repositories

{
    public class DummyUsersRepository : IUsersRepository
    {
        //fields and properties
        List<User> users =
                [
                    new User(1, "Peter Sauber", "06-12345678", "pete.sauber@gmail.com", "no"),
                    new User(2, "Bill hodges", "06-12345678", "bill.hodges@gmail.com", "no"),
                    new User(3, "Morris Bellamy", "06-12345678", "bill.hodges@gmail.com", "no")
                ];
        


        //methods
        public List<User> GetAll()
        {
            return users;
        }

        public User? GetById(int userId)
        {
            return users.FirstOrDefault(x => x.UserId == userId);
        }

        public void Add(User user)
        {
            int ID = users.Count;
            user.UserId = ID+1;

            users.Add(user);
        }

        public void Update(User user)
        {
            users[user.UserId-1] = user;
        }

        public void Delete(User user)
        {
            if (user.Password == users[user.UserId - 1].Password)
            {
                users.RemoveAt(user.UserId - 1);
            }

        }


    }
}
