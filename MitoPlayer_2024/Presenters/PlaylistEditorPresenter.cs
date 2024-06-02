using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Presenters
{
    public class PlaylistEditorPresenter
    {
        private IPlaylistEditorView view;
        private ITrackDao trackDao;
        private ISettingDao settingDao;
        private bool isEditMode = false;
        public Playlist newPlaylist;
        private int lastGeneratedPlaylistId;

        public PlaylistEditorPresenter(IPlaylistEditorView view, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = view;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.isEditMode = false;

            this.lastGeneratedPlaylistId = this.settingDao.GetIntegerSetting(Settings.LastGeneratedPlaylistId.ToString(),true);

            this.lastGeneratedPlaylistId = this.lastGeneratedPlaylistId + 1;
            ((PlaylistEditorView)this.view).SetPlaylistName("New Playlist " + this.lastGeneratedPlaylistId.ToString());

            this.settingDao.SetIntegerSetting(Settings.LastGeneratedPlaylistId.ToString(), this.lastGeneratedPlaylistId, true);

            this.view.CreateOrEditPlaylist += CreateOrEditPlaylist;
            this.view.ClosePlaylistEditor += ClosePlaylistEditor;
        }

        public PlaylistEditorPresenter(IPlaylistEditorView view, ITrackDao trackDao, Playlist playlist)
        {
            this.view = view;
            this.trackDao = trackDao;
            this.newPlaylist = playlist;
            this.isEditMode = true;

            ((PlaylistEditorView)this.view).SetPlaylistName(playlist.Name, true);
            
            this.view.CreateOrEditPlaylist += CreateOrEditPlaylist;
            this.view.ClosePlaylistEditor += ClosePlaylistEditor;
        }
        private void ClosePlaylistEditor(object sender, EventArgs e)
        {
            ((PlaylistEditorView)this.view).DialogResult = DialogResult.Cancel;
            ((PlaylistEditorView)this.view).Close();
        }
        private void CreateOrEditPlaylist(object sender, Helpers.ListEventArgs e)
        {
            ((PlaylistEditorView)this.view).DialogResult = DialogResult.None;

            if (this.isEditMode)
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    if (e.StringField1.Equals(this.newPlaylist.Name)){
                        ((PlaylistEditorView)this.view).DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        Playlist playlist = this.trackDao.GetPlaylistByName(e.StringField1);
                        if(playlist != null)
                        {
                            MessageBox.Show("Playlist name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            this.newPlaylist.Name = e.StringField1;
                            ((PlaylistEditorView)this.view).DialogResult = DialogResult.OK;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Playlist name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    List<Playlist> playlistList = this.trackDao.GetAllPlaylist();
                    if (playlistList != null && playlistList.Count > 0)
                    {
                        if (playlistList.Exists(x => x.Name.Equals(e.StringField1)))
                        {
                            MessageBox.Show("Playlist name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Playlist playlist = new Playlist();
                            playlist.Id = this.trackDao.GetNextId(TableName.Playlist.ToString());
                            playlist.Name = e.StringField1;
                            playlist.OrderInList = playlistList.Count;
                            playlist.QuickListGroup = 0;
                            playlist.IsActive = false;
                            this.newPlaylist = playlist;
                            ((PlaylistEditorView)this.view).DialogResult = DialogResult.OK;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Playlist name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }            
        }

    }
}
