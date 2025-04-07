using Microsoft.Data.SqlClient;
using MvcWhatsUp.Models;

namespace MvcWhatsUp.Repositories
{
    public class ChatsRepository : IChatsRepository
    {

        //fields and properties
        private readonly string? _connectionString;

        //constructors
        public ChatsRepository(IConfiguration configuration)
        {
            //get (database) connectionstring from appsettings
            _connectionString = configuration.GetConnectionString("WhatsUpDatabase");

        }


        //methods
        public List<Message> GetMessages(int userId1, int userId2)
        {
            List<Message> messages = new List<Message>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT [MessageId], [SenderUserId], [ReceiverUserId], [Message], [SendAt]" +
                            "FROM [messages]" +
                            "WHERE ([SenderUserId] = @UserId1 AND [ReceiverUserId] = @UserId2) OR" +
                            "([SenderUserId] = @UserId2 AND [ReceiverUserId] = @UserId1)" +
                            "ORDER BY [SendAt] ASC";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId1", userId1);
                command.Parameters.AddWithValue("@UserId2", userId2);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Message message = ReadMessage(reader);
                    messages.Add(message);
                }
            }
            return messages;
        }

        //This is to help process the results of the sql commands
        private Message ReadMessage(SqlDataReader reader)
        {
            //retrieve data from fields
            int id = (int)reader["MessageId"];
            int senderUserId = (int)reader["SenderUserId"];
            int receiverUserId = (int)reader["ReceiverUserId"];
            string messageText = (string)reader["Message"];
            DateTime sendAt = (DateTime)reader["SendAt"];

            //return the user that was just read
            return new Message(id, senderUserId, receiverUserId, messageText, sendAt);
        }

        public void AddMessage(Message message)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"INSERT INTO messages VALUES (@SenderUserId, @ReceiverUserId, @Message, @SendAt);" +
                    $"SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@SenderUserId", message.SenderUserId);
                command.Parameters.AddWithValue("@ReceiverUserId", message.ReceiverUserId);
                command.Parameters.AddWithValue("@Message", message.MessageText);
                command.Parameters.AddWithValue("@SendAt", message.SendAt);

                command.Connection.Open();
                message.MessageId = Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}
