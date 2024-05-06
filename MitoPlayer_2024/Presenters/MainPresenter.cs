using MitoPlayer_2024._Repositories;
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
            this.mainView.RemoveMissingTracks += RemoveMissingTracks;
            this.mainView.RemoveDuplicatedTracks += RemoveDuplicatedTracks;
            this.mainView.OpenFiles += OpenFiles;
            this.mainView.OpenDirectory += OpenDirectory;

            this.mainView.PlayTrack += PlayTrack;
            this.mainView.PauseTrack += PauseTrack;
            this.mainView.StopTrack += StopTrack;
            this.mainView.PrevTrack += PrevTrack;
            this.mainView.NextTrack += NextTrack;
            this.mainView.RandomTrack += RandomTrack;

            this.mainView.OrderByTitle += OrderByTitle;
            this.mainView.OrderByArtist += OrderByArtist;
            this.mainView.OrderByFileName += OrderByFileName;
            this.mainView.Reverse += Reverse;
            this.mainView.Shuffle += Shuffle;
            this.mainView.Clear += Clear;


            this.ShowPlaylistView(this, new EventArgs());
        }
        private void ShowPlaylistView(object sender, EventArgs e)
        {
            IPlaylistView view = PlaylistView.GetInstance((MainView)mainView);
            this.view = view;
            IPlaylistDao playlistDao = new PlaylistDao(sqlConnectionString);
            ITrackDao trackDao = new TrackDao(sqlConnectionString);
            new PlaylistPresenter(view, playlistDao, trackDao);
        }

        private void RemoveMissingTracks(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).RemoveMissingTracks();
            }
        }
        private void RemoveDuplicatedTracks(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).RemoveDuplicatedTracks();
            }
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
        private void RandomTrack(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).RandomEvent();
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
        private void Clear(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                ((PlaylistView)this.view).ClearEvent();
            }
        }
    }
}
