namespace MvcWhatsUp.Models
{
    public class LoginModel
    {
        //fields and properties
        public string Username { get; set; }
        public string Password { get; set; }

        //constructors
        public LoginModel() { }

        public LoginModel(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        //methods

    }
}
