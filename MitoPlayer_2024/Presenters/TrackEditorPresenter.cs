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
    internal class TrackEditorPresenter
    {
        internal DataTable trackListTable;
        internal int currentPlaylistId;
        private ITrackEditorView trackEditorView;
        private IPlaylistDao playlistDao;
        private ITrackDao trackDao;
        private ISettingDao settingDao;
        private MediaPlayerComponent mediaPLayerComponent { get; set; }

        public TrackEditorPresenter(ITrackEditorView trackEditorView, MediaPlayerComponent mediaPlayerComponent, IPlaylistDao playlistDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.trackEditorView = trackEditorView;
            this.playlistDao = playlistDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.mediaPLayerComponent = mediaPLayerComponent;

            this.trackEditorView.Show();
        }


        internal void Clear()
        {
        }

        internal void OrderByArtist()
        {
        }

        internal void OrderByFileName()
        {
        }

        internal void OrderByTitle()
        {
        }

        internal void RemoveDuplicatedTracks()
        {
        }

        internal void RemoveMissingTracks()
        {
        }

        internal void Reverse()
        {
        }

        internal void Shuffle()
        {
        }

       
        internal void CallAddTrackToTrackListEvent(List<Track> trackList)
        {
 
        }

        internal void CallChangeProgressEvent(int integerField1, int integerField2)
        {

        }

        internal void CallChangeVolumeEvent(int integerField1)
        {
   
        }
    }
}
