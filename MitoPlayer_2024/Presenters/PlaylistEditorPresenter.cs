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
        private IPlaylistEditorView playlistEditorView;
        private IPlaylistDao playlistDao;
        private ISettingDao settingDao;
        private bool isEditMode = false;
        public Playlist newPlaylist;
        private int lastGeneratedPlaylistId;

        public PlaylistEditorPresenter(IPlaylistEditorView playlistEditorView, IPlaylistDao playlistDao, ISettingDao settingDao)
        {
            this.playlistEditorView = playlistEditorView;
            this.playlistDao = playlistDao;
            this.settingDao = settingDao;
            this.isEditMode = false;

            this.lastGeneratedPlaylistId = this.settingDao.GetIntegerSetting(Settings.LastGeneratedPlaylistId.ToString());

            this.lastGeneratedPlaylistId = this.lastGeneratedPlaylistId + 1;
            ((PlaylistEditorView)this.playlistEditorView).SetPlaylistName("New Playlist "+ this.lastGeneratedPlaylistId.ToString());

            this.settingDao.SetIntegerSetting(Settings.LastGeneratedPlaylistId.ToString(), this.lastGeneratedPlaylistId);

            this.playlistEditorView.CreateOrEditPlaylist += CreateOrEditPlaylist;
            this.playlistEditorView.ClosePlaylistEditor += ClosePlaylistEditor;
        }

        public PlaylistEditorPresenter(IPlaylistEditorView view, IPlaylistDao playlistDao, Playlist playlist)
        {
            this.playlistEditorView = view;
            this.playlistDao = playlistDao;
            this.newPlaylist = playlist;
            this.isEditMode = true;

            ((PlaylistEditorView)this.playlistEditorView).SetPlaylistName(playlist.Name, true);
            
            this.playlistEditorView.CreateOrEditPlaylist += CreateOrEditPlaylist;
            this.playlistEditorView.ClosePlaylistEditor += ClosePlaylistEditor;
        }
        private void ClosePlaylistEditor(object sender, EventArgs e)
        {
            ((PlaylistEditorView)this.playlistEditorView).DialogResult = DialogResult.Cancel;
            ((PlaylistEditorView)this.playlistEditorView).Close();
        }
        private void CreateOrEditPlaylist(object sender, Helpers.ListEventArgs e)
        {
            ((PlaylistEditorView)this.playlistEditorView).DialogResult = DialogResult.None;

            if (this.isEditMode)
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    if (e.StringField1.Equals(this.newPlaylist.Name)){
                        ((PlaylistEditorView)this.playlistEditorView).DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        Playlist playlist = this.playlistDao.GetPlaylistByName(e.StringField1);
                        if(playlist != null)
                        {
                            MessageBox.Show("Playlist name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            try
                            {
                                this.newPlaylist.Name = e.StringField1;
                                ((PlaylistEditorView)this.playlistEditorView).DialogResult = DialogResult.OK;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Playlist hasn't been updated!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

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
                    List<Playlist> playlistList = this.playlistDao.GetAllPlaylist();
                    if (playlistList != null && playlistList.Count > 0)
                    {
                        if (playlistList.Exists(x => x.Name.Equals(e.StringField1)))
                        {
                            MessageBox.Show("Playlist name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            try
                            {
                                Playlist playlist = new Playlist();
                                playlist.Id = this.GetNewPlaylistId();
                                playlist.Name = e.StringField1;
                                playlist.OrderInList = playlistList.Count;
                                this.newPlaylist = playlist;
                                ((PlaylistEditorView)this.playlistEditorView).DialogResult = DialogResult.OK;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Playlist hasn't been created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                    }
                    else
                    {
                        try
                        {
                            Playlist playlist = new Playlist();
                            playlist.Id = this.GetNewPlaylistId();
                            playlist.Name = e.StringField1;
                            playlist.OrderInList = 0;
                            this.playlistDao.CreatePlaylist(playlist);
                            this.newPlaylist = playlist;
                            ((PlaylistEditorView)this.playlistEditorView).DialogResult = DialogResult.OK;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Playlist hasn't been created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Playlist name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            
            
        }
        private int GetNewPlaylistId()
        {
            int id = this.playlistDao.GetLastObjectId(TableName.Playlist.ToString());
            if (id == -1)
                return 0;
            else
                return id + 1;
        }
    }
}
