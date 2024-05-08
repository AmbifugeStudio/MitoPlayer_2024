using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

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

        public void SetPlaylistName(String playlistName)
        {
            this.txtPlaylistName.Text = playlistName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ListEventArgs args = new ListEventArgs();
            args.StringField1 = txtPlaylistName.Text;
            CreateOrEditPlaylist?.Invoke(this, args);
        }

        private void txtPlaylistName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ListEventArgs args = new ListEventArgs();
                args.StringField1 = txtPlaylistName.Text;
                CreateOrEditPlaylist?.Invoke(this, args);
            }else if(e.KeyCode == Keys.Escape)
            {
                ClosePlaylistEditor?.Invoke(this,new EventArgs());
            }
        }
    }
}
