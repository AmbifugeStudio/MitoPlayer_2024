using MitoPlayer_2024.Model;
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
    public partial class TrackEditorView : Form,ITrackEditorView
    {
        public TrackEditorView()
        {
            InitializeComponent();
        }

        #region SINGLETON

        private static TrackEditorView instance;

        //MDI nélkül kiveszed a containert a pm-ből
        public static TrackEditorView GetInstance(Form mainView)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new TrackEditorView();
                instance.MdiParent = mainView;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
            }
            else
            {
                if (instance.WindowState == FormWindowState.Minimized)
                    instance.WindowState = FormWindowState.Normal;
                instance.BringToFront();
            }
            return instance;
        }
        #endregion

        internal void Clear()
        {
            throw new NotImplementedException();
        }

        internal void CreatePlaylist()
        {
            throw new NotImplementedException();
        }

        internal void DeletePlaylist()
        {
            throw new NotImplementedException();
        }

        internal void LoadPlaylist()
        {
            throw new NotImplementedException();
        }

        internal void Next()
        {
            throw new NotImplementedException();
        }

        internal void OpenDirectory()
        {
            throw new NotImplementedException();
        }

        internal void OpenFiles()
        {
            throw new NotImplementedException();
        }

        internal void OrderByArtist()
        {
            throw new NotImplementedException();
        }

        internal void OrderByFileName()
        {
            throw new NotImplementedException();
        }

        internal void OrderByTitle()
        {
            throw new NotImplementedException();
        }

        internal void Pause()
        {
            throw new NotImplementedException();
        }

        internal void Play()
        {
            throw new NotImplementedException();
        }

        internal void Prev()
        {
            throw new NotImplementedException();
        }

        internal void Random()
        {
            throw new NotImplementedException();
        }

        internal void RemoveDuplicatedTracks()
        {
            throw new NotImplementedException();
        }

        internal void RenamePlaylist()
        {
            throw new NotImplementedException();
        }

        internal void Reverse()
        {
            throw new NotImplementedException();
        }

        internal void Shuffle()
        {
            throw new NotImplementedException();
        }

        internal void Stop()
        {
            throw new NotImplementedException();
        }

        internal void AddTracksToTrackList(List<Track> trackList)
        {
            throw new NotImplementedException();
        }

        internal void LoadPlaylistExt()
        {
            throw new NotImplementedException();
        }

        internal void RenamePlaylistExt()
        {
            throw new NotImplementedException();
        }

        internal void DeletePlaylistExt()
        {
            throw new NotImplementedException();
        }

        internal void RemoveDuplicatedTracksExt()
        {
            throw new NotImplementedException();
        }

        internal void OrderByTitleExt()
        {
            throw new NotImplementedException();
        }

        internal void OrderByArtistExt()
        {
            throw new NotImplementedException();
        }

        internal void OrderByFileNameExt()
        {
            throw new NotImplementedException();
        }

        internal void PlayExt()
        {
            throw new NotImplementedException();
        }

        internal void PauseExt()
        {
            throw new NotImplementedException();
        }

        internal void PrevExt()
        {
            throw new NotImplementedException();
        }

        internal void NextExt()
        {
            throw new NotImplementedException();
        }

        internal void CreatePlaylistExt()
        {
            throw new NotImplementedException();
        }

        internal void CallCreatePlaylistEvent()
        {
            throw new NotImplementedException();
        }

        internal void CallLoadPlaylistEvent()
        {
            throw new NotImplementedException();
        }

        internal void CallRenamePlaylistEvent()
        {
            throw new NotImplementedException();
        }

        internal void CallDeletePlaylistEvent()
        {
            throw new NotImplementedException();
        }

        internal void UpdateAfterPlayTrack(int currentTrackIndex)
        {
            throw new NotImplementedException();
        }

        internal void UpdateAfterPlayTrackAfterPause()
        {
            throw new NotImplementedException();
        }

        internal void UpdateAfterPauseTrack()
        {
            throw new NotImplementedException();
        }

        internal void UpdateAfterStopTrack()
        {
            throw new NotImplementedException();
        }

        internal void CallPlayTrackEvent()
        {
            throw new NotImplementedException();
        }

        internal void CallPauseTrackEvent()
        {
            throw new NotImplementedException();
        }

        internal void CallStopTrackEvent()
        {
            throw new NotImplementedException();
        }

        internal void CallPrevTrackEvent()
        {
            throw new NotImplementedException();
        }

        internal void CallNextTrackEvent()
        {
            throw new NotImplementedException();
        }

        internal void CallRandomTrackEvent()
        {
            throw new NotImplementedException();
        }
    }
}
