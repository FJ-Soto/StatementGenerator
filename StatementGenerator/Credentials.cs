namespace FTDStatementPrinter
{
    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string AccountID { get; set; }
        public bool DoSave { get; set; }

        public Credentials(string type, string username, string password, string accID = null, bool doSave = false)
        {
            Username = username;
            Password = password;
            Type = type;
            AccountID = accID;
            DoSave = doSave;
        }


        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password);
        }
    }
}
