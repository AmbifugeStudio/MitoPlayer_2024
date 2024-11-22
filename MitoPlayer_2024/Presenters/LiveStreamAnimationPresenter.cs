using MitoPlayer_2024.IViews;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Presenters
{
    internal class LiveStreamAnimationPresenter
    {

        private ILiveStreamAnimationView view;
        private ITagDao tagDao { get; set; }
        private ITrackDao trackDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private MediaPlayerComponent mediaPlayerComponent { get; set; }

        public LiveStreamAnimationPresenter(ILiveStreamAnimationView liveStreamAnimationView, MediaPlayerComponent mediaPlayerComponent, ITagDao tagDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = liveStreamAnimationView;
            this.mediaPlayerComponent = mediaPlayerComponent;
            this.tagDao = tagDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
        }
    }
}
