using MitoPlayer_2024.Helpers;
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
        public DataTable trackListTable { get; set; }
        private IHarmonizerView view { get; set; }
        private ITrackDao trackDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private MediaPlayerComponent mediaPLayerComponent { get; set; }

        public HarmonizerPresenter(IHarmonizerView view, MediaPlayerComponent mediaPlayer, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = view;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.mediaPLayerComponent = mediaPlayer;
            this.mediaPLayerComponent.Initialize(this.trackListTable);
            this.InitializeVolume();

            this.view.Show();
        }
        private void InitializeVolume()
        {
            int volume = this.settingDao.GetIntegerSetting(Settings.Volume.ToString());
            if (volume == -1)
                volume = 50;
           // this.view.SetVolume(volume);
            this.mediaPLayerComponent.MediaPlayer.settings.volume = volume;
        }
        public void CallAddTrackToTrackListEvent(List<Model.Track> trackList, int dragIndex, bool internalDragnadDrop = false, bool fromExternalDragAndDrop = false,  bool insertIntoDefault = false)
        {
        }
        public void CallChangeProgressEvent(int integerField1, int integerField2)
        {
        }
        public void CallChangeVolumeEvent(int integerField1)
        {
        }
        public void OrderByArtist()
        {
        }
        public void OrderByFileName()
        {
        }
        public void OrderByTitle()
        {
        }
        public void Reverse()
        {
        }
        public void Shuffle()
        {
        }
        public void RemoveDuplicatedTracks()
        {
        }
        public void RemoveMissingTracks()
        {
        }
        public void Clear()
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

        internal void CallChangePreviewEvent(bool booleanField1)
        {
            throw new NotImplementedException();
        }
    }
}
