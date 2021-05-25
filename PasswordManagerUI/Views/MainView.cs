using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordManagerUI
{
    public partial class MainView : BaseForm
    {
        private List<PasswordEntryViewModel> pwdEntryList = new List<PasswordEntryViewModel>();

        public MainView()
        {
            InitializeComponent();
            // Set the view to show details.
            passwordListView.View = View.Details;
            // Allow the user to edit item text.
            passwordListView.LabelEdit = true;
            // Allow the user to rearrange columns.
            passwordListView.AllowColumnReorder = true;
            // Select the item and subitems when selection is made.
            passwordListView.FullRowSelect = true;
            //passwordListView.GridLines = true;
            Populate();
            foreach (var item in pwdEntryList)
            {
                ListViewItem newListItem = new ListViewItem(item.Title);
                newListItem.SubItems.Add(item.UserName);
                newListItem.SubItems.Add(item.Password.ToString());
                newListItem.SubItems.Add(item.Url);
                newListItem.SubItems.Add(item.Note);
                passwordListView.Items.Add(newListItem);
            }
            //ListViewItem item1 = new ListViewItem("item1");
            //item1.SubItems.Add("1");
            //item1.SubItems.Add("2");
            //item1.SubItems.Add("3");
            //item1.SubItems.Add("3");
            //ListViewItem item2 = new ListViewItem("item2");
            //item2.SubItems.Add("4");
            //item2.SubItems.Add("5");
            //item2.SubItems.Add("6");
            //ListViewItem item3 = new ListViewItem("item3");
            //item3.SubItems.Add("7");
            //item3.SubItems.Add("8");
            //item3.SubItems.Add("9");

            // -2 = autosize
            passwordListView.Columns.Add("Title", -2, HorizontalAlignment.Left);
            passwordListView.Columns.Add("Username", -2, HorizontalAlignment.Left);
            passwordListView.Columns.Add("Password", -2, HorizontalAlignment.Left);
            passwordListView.Columns.Add("URL", -2, HorizontalAlignment.Left);
            passwordListView.Columns.Add("Note", -2, HorizontalAlignment.Left);

            //Add the items to the ListView.
            //passwordListView.Items.AddRange(new ListViewItem[] { item1, item2, item3 });
            // Add the ListView to the control collection.
            //this.Controls.Add(passwordListView);
        }

        private void Populate()
        {
            pwdEntryList.Add(new PasswordEntryViewModel("Title1", "Username1", new System.Security.SecureString(), "URL1", "Note1"));
            pwdEntryList.Add(new PasswordEntryViewModel("Title2", "Username2", new System.Security.SecureString(), "URL2", "Note2"));
        }

        private void passwordAdd_Click(object sender, EventArgs e)
        {
            var pwdEntryWindow = new NewPasswordEntry(ref pwdEntryList);
            pwdEntryWindow.Show();
        }
    }
}
