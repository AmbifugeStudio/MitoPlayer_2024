using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Presenters
{
    public class MainPresenter
    {
        private IMainView mainView;
        private readonly string sqlConnectionString;

        private IPlaylistView view;

        public MainPresenter(IMainView mainView, string sqlConnectionString)
        {
            this.mainView = mainView;
            this.sqlConnectionString = sqlConnectionString;

            this.mainView.ShowPlaylistView += ShowPlaylistView;

            this.mainView.OpenFiles += OpenFiles;
            this.mainView.OpenDirectory += OpenDirectory;
            this.mainView.PlayTrack += PlayTrack;
            this.mainView.PauseTrack += PauseTrack;
            this.mainView.StopTrack += StopTrack;
            this.mainView.PrevTrack += PrevTrack;
            this.mainView.NextTrack += NextTrack;
            this.mainView.OrderByTitle += OrderByTitle;
            this.mainView.OrderByArtist += OrderByArtist;
            this.mainView.OrderByFileName += OrderByFileName;
            this.mainView.Reverse += Reverse;
            this.mainView.Shuffle += Shuffle;
            this.mainView.RandomTrack += RandomTrack;
            this.mainView.Clear += Clear;
            this.mainView.RemoveMissingTracks += RemoveMissingTracks;
            this.mainView.RemoveDuplicatedTracks += RemoveDuplicatedTracks;

            this.mainView.CreatePlaylist += CreatePlaylist;
            this.mainView.LoadPlaylist += LoadPlaylist;
            this.mainView.RenamePlaylist += RenamePlaylist;
            this.mainView.DeletePlaylist += DeletePlaylist;

            this.ShowPlaylistView(this, new EventArgs());
        }


        private void ShowPlaylistView(object sender, EventArgs e)
        {
            IPlaylistView view = PlaylistView.GetInstance((MainView)mainView);
            this.view = view;
            IPlaylistDao playlistDao = new PlaylistDao(sqlConnectionString);
            ITrackDao trackDao = new TrackDao(sqlConnectionString);
            ISettingDao settingDao = new SettingDao(sqlConnectionString);
            new PlaylistPresenter(view, playlistDao, trackDao, settingDao);
        }

        private void OpenFiles(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).OpenFilesEvent();
            }
        }
        private void OpenDirectory(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).OpenDirectoryEvent();
            }
        }
        private void PlayTrack(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).PlayEvent();
            }
        }
        private void PauseTrack(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).PauseEvent();
            }
        }
        private void StopTrack(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).StopEvent();
            }
        }
        private void PrevTrack(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).PrevEvent();
            }
        }
        private void NextTrack(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).NextEvent();
            }
        }
        private void OrderByTitle(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).OrderByTitleEvent();
            }
        }
        private void OrderByArtist(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).OrderByArtistEvent();
            }
        }
        private void OrderByFileName(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).OrderByFileNameEvent();
            }
        }
        private void Shuffle(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).ShuffleEvent();
            }
        }
        private void Reverse(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).ReverseEvent();
            }
        }
        private void RandomTrack(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).RandomEvent();
            }
        }
        private void Clear(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).ClearEvent();
            }
        }
        private void RemoveMissingTracks(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).RemoveMissingTracksEvent();
            }
        }
        private void RemoveDuplicatedTracks(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).RemoveDuplicatedTracksEvent();
            }
        }

        private void CreatePlaylist(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).CreatePlaylistEvent();
            }
        }
        private void LoadPlaylist(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).LoadPlaylistEvent();
            }
        }
        private void RenamePlaylist(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).RenamePlaylistEvent();
            }
        }
        private void DeletePlaylist(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).DeletePlaylistEvent();
            }
        }

    }
}
