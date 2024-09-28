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

        public Playlist newPlaylist;
        private String playlistName;
        private int playlistHotkey;

        public PlaylistEditorPresenter(IPlaylistEditorView view, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = view;
            this.trackDao = trackDao;
            this.settingDao = settingDao;

            this.playlistName = "New Playlist " + this.settingDao.GetNextId(TableName.Playlist.ToString());
            this.playlistHotkey = 0;
            ((PlaylistEditorView)this.view).SetPlaylistName(this.playlistName);
            ((PlaylistEditorView)this.view).SetHotkey(this.playlistHotkey);

            this.view.ChangeName += ChangeName;
            this.view.ChangeHotkey += ChangeHotkey;
            this.view.CloseWithOk += CloseWithOk;
            this.view.CloseWithCancel += CloseWithCancel;
        }

        public PlaylistEditorPresenter(IPlaylistEditorView view, ITrackDao trackDao, ISettingDao settingDao,Playlist playlist)
        {
            this.view = view;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.newPlaylist = playlist;

            this.playlistName = this.newPlaylist.Name;
            this.playlistHotkey = this.newPlaylist.Hotkey;

            ((PlaylistEditorView)this.view).SetPlaylistName(this.playlistName, true);
            ((PlaylistEditorView)this.view).SetHotkey(this.playlistHotkey);

            this.view.ChangeName += ChangeName;
            this.view.ChangeHotkey += ChangeHotkey;
            this.view.CloseWithOk += CloseWithOk;
            this.view.CloseWithCancel += CloseWithCancel;
        }
        private void ChangeName(object sender, ListEventArgs e)
        {
            if(playlistName.Equals("Default Playlist"))
            {
                MessageBox.Show("Default playlist cannot be renamed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.playlistName = e.StringField1;
            }
        }
        private void ChangeHotkey(object sender, ListEventArgs e)
        {
            this.playlistHotkey = e.IntegerField1;
        }
        private void CloseWithOk(object sender, EventArgs e)
        {
            bool result = true;

            List<Playlist> playlistList = this.trackDao.GetAllPlaylist();
            if (playlistList != null && playlistList.Count > 0 && this.newPlaylist != null)
            {
                playlistList.Remove(this.newPlaylist);
            }
            if (result)
            {
                if (String.IsNullOrEmpty(this.playlistName))
                {
                    result = false;
                    MessageBox.Show("Playlist name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (result)
            {
                if (playlistList != null && playlistList.Count > 0 &&
                    playlistList.Exists(x => x.Name == this.playlistName))
                {
                    result = false;
                    MessageBox.Show("Playlist name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (result)
            {
                if (this.playlistHotkey != 0)
                {
                    if (playlistList.Exists(x => x.Hotkey == this.playlistHotkey))
                    {
                        DialogResult dr = MessageBox.Show("Hotkey is already used by another Playlist. Do you want to replace?", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.OK)
                        {
                            Playlist playlist = playlistList.Find(x => x.Hotkey == this.playlistHotkey);
                            if (playlist != null)
                            {
                                playlist.Hotkey = 0;
                                this.trackDao.UpdatePlaylist(playlist);
                            }
                        }
                    }
                }
            }
            if (result)
            {
                if (this.newPlaylist == null)
                {
                    Playlist playlist = new Playlist();
                    playlist.Id = this.trackDao.GetNextId(TableName.Playlist.ToString());
                    playlist.Name = this.playlistName;
                    playlist.OrderInList = playlistList.Count;
                    playlist.QuickListGroup = 0;
                    playlist.IsActive = false;
                    playlist.Hotkey = this.playlistHotkey;
                    this.newPlaylist = playlist;
                    try
                    {
                        this.trackDao.CreatePlaylist(this.newPlaylist);
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                }
                else
                {
                    this.newPlaylist.Name = this.playlistName;
                    this.newPlaylist.Hotkey = this.playlistHotkey;
                    try
                    {
                        this.trackDao.UpdatePlaylist(this.newPlaylist);
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                }
            }

            if (result)
            {
                ((PlaylistEditorView)this.view).DialogResult = DialogResult.OK;
                ((PlaylistEditorView)this.view).Close();
            }
            else
            {
                ((PlaylistEditorView)this.view).DialogResult = DialogResult.None;
            }

        }
        private void CloseWithCancel(object sender, EventArgs e)
        {
            ((PlaylistEditorView)this.view).DialogResult = DialogResult.Cancel;
            ((PlaylistEditorView)this.view).Close();
        }
    }
}
