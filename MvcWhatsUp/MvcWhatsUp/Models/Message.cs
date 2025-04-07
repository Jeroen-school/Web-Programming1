namespace MvcWhatsUp.Models
{
    public class Message
    {
        //fields and properties
        public int MessageId { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string MessageText { get; set; }
        public DateTime SendAt { get; set; }

        //constructors
        public Message()
        {
            this.MessageText = "";
        }

        public Message (int id, int senderUserId, int receiverUserId, string messageText, DateTime sendAt)
        {
            this.MessageId = id;
            this.SenderUserId = senderUserId;
            this.ReceiverUserId = receiverUserId;
            this.MessageText = messageText;
            this.SendAt = sendAt;
        }

        //methods
    }
}
