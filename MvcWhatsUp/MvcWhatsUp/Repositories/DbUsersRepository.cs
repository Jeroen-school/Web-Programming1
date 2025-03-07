using MvcWhatsUp.Models;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace MvcWhatsUp.Repositories

{
    public class DbUsersRepository : IUsersRepository
    {
        //fields and properties
        private readonly string? _connectionString;

        //constructors
        public DbUsersRepository(IConfiguration configuration)
        {
            //get (database) connectionstring from appsettings
            _connectionString = configuration.GetConnectionString("WhatsUpDatabase");

        }

        //methods

        public List<User> GetAll()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT UserId, UserName, MobileNumber, EmailAddress FROM Users";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = ReadUser(reader);
                    users.Add(user);
                }

                reader.Close();
            }
            return users;
        }

        //This is take and read users from the sql database
        private User ReadUser(SqlDataReader reader)
        {
            //retrieve data from fields
            int id = (int)reader["UserId"];
            string name = (string)reader["UserName"];
            string mobileNumber = (string)reader["MobileNumber"];
            string emailAddress = (string)reader["EmailAddress"];

            //return the user that was just read
            return new User(id, name, mobileNumber, emailAddress);
        }


        public User? GetById(int userId)
        {
            return new User();
        }

        public void Add(User user)
        {

        }

        public void Update(User user)
        {

        }

        public void Delete(User user)
        {

        }
    }
}
