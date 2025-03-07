namespace MvcWhatsUp.Models
{
    public class Message
    {
        //fields and properties
        public string Name { get; set; }
        public string MessageText { get; set; }

        //constructors
        public Message()
        {
            this.Name = "";
            this.MessageText = "";
        }

        public Message (string name, string message)
        {
            this.Name = name;
            this.MessageText = message;
        }

        //methods
    }
}
