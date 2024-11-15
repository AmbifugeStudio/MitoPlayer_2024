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
    public partial class ProfileEditorView : Form, IProfileEditorView
    {
        public event EventHandler<Messenger> CreateOrEditProfile;
        public event EventHandler CloseProfileEditor;
        public ProfileEditorView()
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
        public void SetProfileName(String profileName, bool edit = false)
        {
            this.txtProfileName.Text = profileName;
            if (edit)
            {
                this.Text = "Edit profile";
            }
            else
            {
                this.Text = "Create profile";
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            Messenger args = new Messenger();
            args.StringField1 = txtProfileName.Text;
            this.CreateOrEditProfile?.Invoke(this, args);
        }
        private void txtPlaylistName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Messenger args = new Messenger();
                args.StringField1 = txtProfileName.Text;
                this.CreateOrEditProfile?.Invoke(this, args);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.CloseProfileEditor?.Invoke(this, new EventArgs());
            }
        }

       
    }
}
