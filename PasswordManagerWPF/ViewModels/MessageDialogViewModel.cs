namespace PasswordManagerWPF
{
    /// <summary>
    /// View model for message dialog
    /// </summary>
    public class MessageDialogViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="text"></param>
        public MessageDialogViewModel(string text)
        {
            Text = text;
        }

        #endregion

        #region Public properties

        // text displayed on message dialog
        public string Text
        {
            get => _Text;
            set
            {
                if (_Text == value) return;
                _Text = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private fields

        private string _Text;

        #endregion
    }
}
