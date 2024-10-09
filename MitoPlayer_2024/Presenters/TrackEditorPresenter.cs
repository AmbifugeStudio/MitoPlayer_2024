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
        private ITrackEditorView view;
        private ITrackDao trackDao;
        private ISettingDao settingDao;
        private MediaPlayerComponent mediaPLayerComponent { get; set; }

        public TrackEditorPresenter(ITrackEditorView view, MediaPlayerComponent mediaPlayerComponent, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = view;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.mediaPLayerComponent = mediaPLayerComponent;

            this.view.Show();
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

       
        internal void CallAddTrackToTrackListEvent(List<Track> trackList, int dragIndex, bool internalDragnadDrop = false, bool fromExternalDragAndDrop = false,  bool insertIntoDefault = false)
        {
 
        }

        internal void CallChangeProgressEvent(int integerField1, int integerField2)
        {

        }

        internal void CallChangeVolumeEvent(int integerField1)
        {
   
        }

        internal void CallChangeMuteEvent(bool booleanField1)
        {
            throw new NotImplementedException();
        }

        internal void CallChangeShuffleEvent(bool booleanField1)
        {
            throw new NotImplementedException();
        }
    }
}
