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
    internal class RuleEditorPresenter
    {
        private IRuleEditorView ruleEditorView;
        private IPlaylistDao playlistDao;
        private ITrackDao trackDao;
        private ISettingDao settingDao;

        public RuleEditorPresenter(IRuleEditorView ruleEditorView, IPlaylistDao playlistDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.ruleEditorView = ruleEditorView;
            this.playlistDao = playlistDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.ruleEditorView.Show();
        }
    }
}
