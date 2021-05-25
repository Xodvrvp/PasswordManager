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
    public partial class BaseForm : Form
    {
        private Color backgroundColor = Color.White;

        public BaseForm()
        {
            InitializeComponent();
            this.BackColor = backgroundColor;
        }
    }
}
