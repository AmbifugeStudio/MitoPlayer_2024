using MitoPlayer_2024.Helpers;
using System;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class PlaylistEditorView : Form, IPlaylistEditorView
    {

        public event EventHandler<ListEventArgs> ChangeName;
        public event EventHandler<ListEventArgs> ChangeHotkey;
        public event EventHandler CloseWithOk;
        public event EventHandler CloseWithCancel;

        public PlaylistEditorView()
        {
            this.InitializeComponent();
            this.txtPlaylistName.Focus();
            this.CenterToScreen();
        }

        public void SetPlaylistName(String playlistName, bool edit = false)
        {
            this.txtPlaylistName.Text = playlistName;
            if(playlistName.Equals("Default Playlist")){
                this.txtPlaylistName.Enabled = false;
            }
            if (edit)
            {
                this.Text = "Edit playlist";
            }
            else
            {
                this.Text = "Create playlist";
            }
        }

        public void SetHotkey(int number)
        {
            rdb1.Checked = false;
            rdb2.Checked = false;
            rdb3.Checked = false;
            rdb4.Checked = false;
            if (number == 1)
            {
                rdb1.Checked = true;
            }
            else if (number == 2)
            {
                rdb2.Checked = true;
            }
            else if (number == 3)
            {
                rdb3.Checked = true;
            }
            else if (number == 4)
            {
                rdb4.Checked = true;
            }
            else
            {
                rdb0.Checked = true;
            }
        }

        private void txtPlaylistName_TextChanged(object sender, EventArgs e)
        {
            this.ChangeName?.Invoke(this, new ListEventArgs() { StringField1 = this.txtPlaylistName.Text });
        }
        private void rdb0_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeHotkey?.Invoke(this, new ListEventArgs() { IntegerField1 = 0 });
        }

        private void rdb1_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeHotkey?.Invoke(this, new ListEventArgs() { IntegerField1 = 1 });
        }

        private void rdb2_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeHotkey?.Invoke(this, new ListEventArgs() { IntegerField1 = 2 });
        }

        private void rdb3_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeHotkey?.Invoke(this, new ListEventArgs() { IntegerField1 = 3 });
        }

        private void rdb4_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeHotkey?.Invoke(this, new ListEventArgs() { IntegerField1 = 4 });
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseWithOk?.Invoke(this, new EventArgs());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseWithCancel?.Invoke(this, new EventArgs());
        }

        private void txtPlaylistName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.CloseWithOk?.Invoke(this, new EventArgs());
            }
            else if(e.KeyCode == Keys.Escape)
            {
                this.CloseWithCancel?.Invoke(this, new EventArgs());
            }
        }

        

        
    }
}
