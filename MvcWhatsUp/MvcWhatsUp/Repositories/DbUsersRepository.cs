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

            using (SqlConnection connection = new SqlConnection(_connectionString))             //this sets up the ground rules for the connection
            {
                string query = "SELECT UserId, UserName, MobileNumber, EmailAddress FROM Users"; //this contains the command to be executed
                SqlCommand command = new SqlCommand(query, connection);                          //this links the command to the ground rules

                command.Connection.Open();                                                      //This opens the connections, so here you connect to the database
                SqlDataReader reader = command.ExecuteReader();                                 //This executes the command

                while (reader.Read())                                                           //Here you process the results of the command you just executed
                {
                    User user = ReadUser(reader);
                    users.Add(user);
                }

                reader.Close();                                                                 //Here you close the reader, finishing the prompt
            }
            return users;
        }

        //This is to help process the results of the sql commands
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

        //This is to also read the password (THIS IS NOT A GOOD IDEA)
        private User ReadUserWithPassword(SqlDataReader reader)
        {
            //retrieve data from fields
            int id = (int)reader["UserId"];
            string name = (string)reader["UserName"];
            string mobileNumber = (string)reader["MobileNumber"];
            string emailAddress = (string)reader["EmailAddress"];
            string password = (string)reader["Password"];

            //return the user that was just read
            return new User(id, name, mobileNumber, emailAddress, password);
        }

        //Here you select just one row in an SQL table, then create a User using that one result
        public User? GetById(int userId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT UserId, UserName, MobileNumber, EmailAddress, Password FROM Users WHERE UserId = {userId}";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user = ReadUserWithPassword(reader);
                }

                reader.Close();
            }

            return user;
        }

        //With this you can add a new user
        public void Add(User user)
        {
            //If the user inputted prompt contains a ), throw them an error, which gets caught by the UsersController
            if (user.UserName.Contains(")") || user.MobileNumber.Contains(")") || user.EmailAddress.Contains(")") || user.Password.Contains(")"))
            {
                throw new Exception("Nice try nerd");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = $"INSERT INTO Users VALUES ('{user.UserName}', '{user.MobileNumber}', '{user.EmailAddress}', '{user.Password}')";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Close();
                }
            }

        }

        //With this you can edit an existing user
        public void Update(User user)
        {
            //If the user inputted prompt contains a ), throw them an error, which gets caught by the UsersController
            if (user.UserName.Contains(")") || user.MobileNumber.Contains(")") || user.EmailAddress.Contains(")") || user.Password.Contains(")"))
            {
                throw new Exception("Nice try nerd");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = $"UPDATE users SET UserName = '{user.UserName}', MobileNumber = '{user.MobileNumber}', EmailAddress = '{user.EmailAddress}' WHERE UserId = {user.UserId};";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Close();
                }
            }
        }

        //With this you can delete an existing user
        public void Delete(User user)
        {
            //If the user inputted prompt contains a ), throw them an error, which gets caught by the UsersController
            if (user.UserName.Contains(")") || user.MobileNumber.Contains(")") || user.EmailAddress.Contains(")") || user.Password.Contains(")"))
            {
                throw new Exception("Nice try nerd");
            }
            else
            {
                //Check if the user's entered password matches the database password
                User passwordValidator = GetById(user.UserId);
                if (user.Password == passwordValidator.Password)
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        string query = $"DELETE FROM users WHERE UserId = {user.UserId};";

                        SqlCommand command = new SqlCommand(query, connection);

                        command.Connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        reader.Close();
                    }
                } else
                {
                    throw new Exception("Wrong password.");
                }
            }
        }
    }
}
