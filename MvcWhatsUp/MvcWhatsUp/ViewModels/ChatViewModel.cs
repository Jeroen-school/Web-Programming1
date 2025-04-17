using MvcWhatsUp.Models;

namespace MvcWhatsUp.ViewModels
{
    public class ChatViewModel
    {
        public List<Message> Messages { get; }
        public User SendingUser { get; }
        public User ReceivingUser { get; }

        public ChatViewModel(List<Message> message, User sendingUser, User receivingUser)
        {
            Messages = message;
            SendingUser = sendingUser;
            ReceivingUser = receivingUser;
        }
    }
}
