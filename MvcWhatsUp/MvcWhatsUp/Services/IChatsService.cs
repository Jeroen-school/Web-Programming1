using MvcWhatsUp.Models;

namespace MvcWhatsUp.Services
{
    public interface IChatsService
    {
        //fields and properties

        //constructors

        //methods
        void AddMessage(Message message);

        List<Message> GetMessages(int userId1, int userId2);

        List<Message> GetLatestMessages(int userId, int numberOfMessages);
    }
}
