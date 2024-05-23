using MitoPlayer_2024.Helpers;
using System;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class PlaylistEditorView : Form, IPlaylistEditorView
    {
        public event EventHandler<ListEventArgs> CreateOrEditPlaylist;
        public event EventHandler ClosePlaylistEditor;

        public PlaylistEditorView()
        {
            InitializeComponent();
            this.txtPlaylistName.Focus();
            this.CenterToScreen();
        }

        public void SetPlaylistName(String playlistName, bool edit = false)
        {
            this.txtPlaylistName.Text = playlistName;
            if (edit)
            {
                this.Text = "Edit playlist";
            }
            else
            {
                this.Text = "Create playlist";
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ListEventArgs args = new ListEventArgs();
            args.StringField1 = txtPlaylistName.Text;
            this.CreateOrEditPlaylist?.Invoke(this, args);
        }

        private void txtPlaylistName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ListEventArgs args = new ListEventArgs();
                args.StringField1 = txtPlaylistName.Text;
                this.CreateOrEditPlaylist?.Invoke(this, args);
            }
            else if(e.KeyCode == Keys.Escape)
            {
                this.ClosePlaylistEditor?.Invoke(this,new EventArgs());
            }
        }
    }
}
