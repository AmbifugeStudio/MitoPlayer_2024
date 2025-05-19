using System;
using System.Drawing;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class AboutView : Form, IAboutView
    {

        public event EventHandler CloseView;

        public AboutView()
        {
            this.InitializeComponent();
            this.SetControlColors();
            this.CenterToScreen();
        }

        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");

        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.btnOk.BackColor = this.BackgroundColor;
            this.btnOk.ForeColor = this.FontColor;
            this.btnOk.FlatAppearance.BorderColor = this.ButtonBorderColor;
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            CloseView?.Invoke(this, e);
        }

        private void AboutView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                CloseView?.Invoke(this, e);
            }
        }

        private void AboutView_Load(object sender, EventArgs e)
        {
            String version = "";
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                version = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            this.lblVersion.Text = version;
        }
    }
}

