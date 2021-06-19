namespace PasswordManagerCore
{
    /// <summary>
    /// Data model for password entry
    /// </summary>
    public class PasswordEntryModel
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }     
    }
}
