using System.Security;

namespace PasswordManagerCore.Models
{
    public class PasswordEntryModel
    {
        public string Title { get; set; }
        public string Username { get; set; }
        //public SecureString Password { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
    }
}
