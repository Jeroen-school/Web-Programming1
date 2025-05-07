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
                string query = "SELECT users.UserId, users.UserName, users.MobileNumber, users.EmailAddress, users.Deleted FROM Users"; //this contains the command to be executed
                SqlCommand command = new SqlCommand(query, connection);                          //this links the command to the ground rules

                command.Connection.Open();                                                      //This opens the connections, so here you connect to the database
                SqlDataReader reader = command.ExecuteReader();                                 //This executes the command

                while (reader.Read())                                                           //Here you process the results of the command you just executed
                {
                    User user = ReadUser(reader);
                    if (user.Deleted == false)
                    {
                        users.Add(user);
                    }
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
            bool Deleted = (bool)reader["Deleted"];

            //return the user that was just read
            return new User(id, name, mobileNumber, emailAddress, Deleted);
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
            bool Deleted = (bool)reader["Deleted"];

            //return the user that was just read
            return new User(id, name, mobileNumber, emailAddress, password);
        }

        //Here you select just one row in an SQL table, then create a User using that one result
        public User? GetById(int userId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT UserId, UserName, MobileNumber, EmailAddress, [Password], users.Deleted FROM Users WHERE users.UserId = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", userId);

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

        public User? GetByLoginCredentials(string username, string password)
        {
            User? user = new User();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT UserId, UserName, MobileNumber, EmailAddress, [Password], users.Deleted FROM Users WHERE [UserName] = @UserName AND [Password] = @Password";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user = ReadUserWithPassword(reader);
                }

                reader.Close();
            }

            if (user.UserName != null)
            {
                return user;
            } else
            {
                return null;
            }
        }

        //With this you can add a new user
        public void Add(User user)
        {
            //If the user inputted prompt contains a ), throw them an error, which gets caught by the UsersController
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"INSERT INTO Users VALUES (@UserName, @MobileNumber, @EmailAddress, @Password, 0);" +
                    $"SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                command.Parameters.AddWithValue("@Password", user.Password);

                command.Connection.Open();
                user.UserId = Convert.ToInt32(command.ExecuteScalar());


            }


        }

        //With this you can edit an existing user
        public void Update(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"UPDATE users SET UserName = @UserName, MobileNumber = @MobileNumber, EmailAddress = @EmailAddress WHERE users.UserId = @Id;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", user.UserId);
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }

        }

        //With this you can delete an existing user
        public void Delete(User user)
        {
            //Check if the user's entered password matches the database password
            User passwordValidator = GetById(user.UserId);
            if (user.Password == passwordValidator.Password)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = $"UPDATE users SET Deleted = 1 WHERE users.UserId = {user.UserId};";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Close();
                }
            }
            else
            {
                throw new Exception("Wrong password.");
            }
        }

        public bool EmailAddressExists(string emailAddress)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT COUNT(*) FROM Users WHERE EmailAddress = @EmailAddress";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@EmailAddress", emailAddress);

                command.Connection.Open();
                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }
    }
}
