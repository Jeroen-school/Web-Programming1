using MvcWhatsUp.Repositories;
using MvcWhatsUp.Models;

namespace MvcWhatsUp.Services
{
    public class ChatsService : IChatsService
    {
        //fields and properties
        private readonly IChatsRepository _chatsRepository;

        //constructors
        public ChatsService(IChatsRepository chatsRepository)
        {
            _chatsRepository = chatsRepository;
        }

        //methods
        public void AddMessage(Message message)
        {
            _chatsRepository.AddMessage(message);
        }

        public List<Message> GetMessages(int userId1, int userId2)
        {
            //get messages between two users
            return _chatsRepository.GetMessages(userId1, userId2);
        }

        public List<Message> GetLatestMessages(int userId, int numberOfMessages)
        {
            //get latest messages for a user
            return _chatsRepository.GetLatestMessages(userId, numberOfMessages);
        }
    }
}
