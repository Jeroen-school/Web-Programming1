namespace MvcWhatsUp.Models
{
    public class User
    {
        //fields and properties
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        
        //constructors
        public User ()
        {
            this.UserId = 0;
            this.UserName = "";
            this.MobileNumber = "";
            this.EmailAddress = "";
            this.Password = "";
        }

        public User(int id, string name, string mobileNumber, string emailAddress)
        {
            this.UserId = id;
            this.UserName = name;
            this.MobileNumber = mobileNumber;
            this.EmailAddress = emailAddress;
        }

        public User (int id, string name, string mobileNumber, string emailAddress, string password)
        {
            this.UserId = id;
            this.UserName = name;
            this.MobileNumber = mobileNumber;
            this.EmailAddress = emailAddress;
            this.Password = password;
        }

        //methods

    }
}
