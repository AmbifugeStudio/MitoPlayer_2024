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
        public event EventHandler<ListEventArgs> CreateOrEditProfile;
        public event EventHandler CloseProfileEditor;
        public ProfileEditorView()
        {
            InitializeComponent();
            this.CenterToScreen();
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
            ListEventArgs args = new ListEventArgs();
            args.StringField1 = txtProfileName.Text;
            this.CreateOrEditProfile?.Invoke(this, args);
        }
        private void txtPlaylistName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListEventArgs args = new ListEventArgs();
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
