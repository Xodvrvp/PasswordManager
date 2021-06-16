namespace PasswordManagerWPF
{
    public class MessageDialogViewModel : BaseViewModel
    {
        #region Constructor
        public MessageDialogViewModel(string text)
        {
            Text = text;
        }

        #endregion

        #region Public properties

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
