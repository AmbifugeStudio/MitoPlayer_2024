using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.IViews;
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
    public partial class PlaylistCreatorView : Form, IPlaylistCreatorView
    {
        private BindingSource playlistListBindingSource;
        private BindingSource trackListBindingSource;
        private BindingSource selectorTrackListBindingSource;

        public PlaylistCreatorView()
        {
            InitializeComponent();
        }

        public event EventHandler<Messenger> CreatePlaylistEvent;
        public event EventHandler<Messenger> EditPlaylistEvent;
        public event EventHandler<Messenger> DeletePlaylistEvent;
        public event EventHandler<Messenger> LoadPlaylistEvent;
        public event EventHandler<Messenger> LoadSelectedPlaylistEvent;
        public event EventHandler<Messenger> SelectPlaylistEvent;
        public event EventHandler<Messenger> SelectTrackEvent;

        public void InitializePlaylistList(DataTableModel model)
        {
            if(model.BindingSource != null)
            {
                if (playlistListBindingSource == null)
                {
                    playlistListBindingSource = new BindingSource();
                }
                playlistListBindingSource.DataSource = model.BindingSource;
                dgvPlaylistList.DataSource = playlistListBindingSource.DataSource;
            }
        }

        public void InitializeSelectorTracklist(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public void InitializeTracklist(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlaylistList(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectorTracklist(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public void UpdateTracklist(DataTableModel model)
        {
            throw new NotImplementedException();
        }


        public void EnablePlaylistListSelection()
        {
            dgvPlaylistList.SelectionChanged += dgvPlaylistList_SelectionChanged;
        }
        public void EnableTracklistSelection()
        {
            dgvSelectorTracklist.SelectionChanged += dgvSelectorTracklist_SelectionChanged;
        }
        public void EnableSelectorTracklistSelection()
        {
            dgvTracklist.SelectionChanged += dgvTracklist_SelectionChanged;
        }
        public void DisablePlaylistListSelection()
        {
            dgvPlaylistList.SelectionChanged -= dgvPlaylistList_SelectionChanged;
        }
        public void DisableTracklistSelection()
        {
            dgvTracklist.SelectionChanged -= dgvTracklist_SelectionChanged;
        }
        public void DisableSelectorTracklistSelection()
        {
            dgvSelectorTracklist.SelectionChanged -= dgvSelectorTracklist_SelectionChanged;
        }
        private void PlaylistCreatorView_Shown(object sender, EventArgs e)
        {
            EnablePlaylistListSelection();
            EnableSelectorTracklistSelection();
            EnableSelectorTracklistSelection();
        }


        private void dgvPlaylistList_SelectionChanged(object sender, EventArgs e)
        {

        }
        private void dgvTracklist_SelectionChanged(object sender, EventArgs e)
        {

        }
        private void dgvSelectorTracklist_SelectionChanged(object sender, EventArgs e)
        {

        }

        
    }
}
