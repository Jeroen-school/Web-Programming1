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

        public bool Deleted { get; set; }

        
        //constructors
        public User ()
        {        }

        public User(int id, string name, string mobileNumber, string emailAddress)
        {
            this.UserId = id;
            this.UserName = name;
            this.MobileNumber = mobileNumber;
            this.EmailAddress = emailAddress;
            this.Deleted = false;
        }

        public User (int id, string name, string mobileNumber, string emailAddress, string password)
        {
            this.UserId = id;
            this.UserName = name;
            this.MobileNumber = mobileNumber;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.Deleted = false;
        }

        public User(int id, string name, string mobileNumber, string emailAddress, bool deleted)
        {
            this.UserId = id;
            this.UserName = name;
            this.MobileNumber = mobileNumber;
            this.EmailAddress = emailAddress;
            this.Deleted = deleted;
        }

        public User(int id, string name, string mobileNumber, string emailAddress, string password, bool deleted)
        {
            this.UserId = id;
            this.UserName = name;
            this.MobileNumber = mobileNumber;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.Deleted = deleted;
        }

        //methods

    }
}
