using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class SetupView : Form, ISetupView
    {
        public event EventHandler<ListEventArgs> CloseWithOk;
        public SetupView()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseWithOk?.Invoke(this, new ListEventArgs() { 
                StringField1 = this.txtBoxHost.Text,
                StringField2 = this.txtBoxPort.Text,
                StringField3 = this.txtBoxUserName.Text,
                StringField4 = this.txtBoxPassword.Text,
            });
        }
    }
}
