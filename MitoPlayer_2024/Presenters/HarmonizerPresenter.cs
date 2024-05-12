using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Presenters
{
    internal class HarmonizerPresenter
    {
        internal DataTable trackListTable;
        internal int currentPlaylistId;
        private IHarmonizerView view;
        private IPlaylistDao playlistDao;
        private ITrackDao trackDao;
        private ISettingDao settingDao;
        private MediaPlayerComponent mediaPLayerComponent { get; set; }

        public HarmonizerPresenter(IHarmonizerView view, MediaPlayerComponent mediaPlayer, IPlaylistDao playlistDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = view;
            this.playlistDao = playlistDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.mediaPLayerComponent = mediaPLayerComponent;
        }

        internal void AddTracksToTrackList(List<Track> trackList)
        {
            throw new NotImplementedException();
        }

        internal void Clear()
        {
            throw new NotImplementedException();
        }

        internal void CreatePlaylist()
        {
            throw new NotImplementedException();
        }

        internal void Next()
        {
            throw new NotImplementedException();
        }

        internal void NextTrack()
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

        internal void PauseTrack()
        {
            throw new NotImplementedException();
        }

        internal void Prev()
        {
            throw new NotImplementedException();
        }

        internal void PrevTrack()
        {
            throw new NotImplementedException();
        }

        internal void Random()
        {
            throw new NotImplementedException();
        }

        internal void RandomTrack()
        {
            throw new NotImplementedException();
        }

        internal void RemoveDuplicatedTracks()
        {
            throw new NotImplementedException();
        }

        internal void RemoveMissingTracks()
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

        internal void StopTrack()
        {
            throw new NotImplementedException();
        }
    }
}
