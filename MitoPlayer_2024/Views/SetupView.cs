using MitoPlayer_2024.Helpers;
using System;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class SetupView : Form, ISetupView
    {
        public event EventHandler<Messenger> CloseWithOk;
        public SetupView()
        {
            this.InitializeComponent();
            this.SetControlColors();
        }

        private void SetControlColors()
        {
            this.BackColor = CustomColor.BackColor;
            this.ForeColor = CustomColor.ForeColor;

            this.btnOk.BackColor = CustomColor.BackColor;
            this.btnOk.ForeColor = CustomColor.ForeColor;
            this.btnOk.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseWithOk?.Invoke(this, new Messenger() { 
                StringField1 = this.txtBoxHost.Text,
                StringField2 = this.txtBoxPort.Text,
                StringField3 = this.txtBoxUserName.Text,
                StringField4 = this.txtBoxPassword.Text,
            });
        }
    }
}
