using MvcWhatsUp.Models;

namespace MvcWhatsUp.Repositories
{
    public interface IChatsRepository
    {
        void AddMessage(Message message);

        List<Message> GetMessages(int userId1, int userId2);
    }
}
