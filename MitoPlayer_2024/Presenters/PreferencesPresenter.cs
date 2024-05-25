using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    internal class PreferencesPresenter
    {
        private IPreferencesView preferencesView;
        private IPlaylistDao playlistDao;
        private ITrackDao trackDao;
        private IProfileDao profileDao;
        private ISettingDao settingDao;

        public bool databaseCleared = false;
        public PreferencesPresenter(IPreferencesView preferencesView, IPlaylistDao playlistDao, ITrackDao trackDao,IProfileDao profileDao, ISettingDao settingDao)
        {
            this.preferencesView = preferencesView;
            this.playlistDao = playlistDao;
            this.trackDao = trackDao;
            this.profileDao = profileDao;
            this.settingDao = settingDao;

            this.preferencesView.CloseViewWithOkEvent += CloseViewWithOkEvent;
            this.preferencesView.CloseViewWithCancelEvent += CloseViewWithCancelEvent;
            this.preferencesView.ClearDatabaseEvent += ClearDatabaseEvent;
        }

        private void ClearDatabaseEvent(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want delete the content of the database? All data will be lost!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.playlistDao.ClearPlaylistTable();
                this.playlistDao.ClearPlaylistContentTable();
                this.trackDao.ClearTrackTable();
                this.profileDao.ClearProfileTable();
                this.settingDao.ClearSettingTable();
                this.databaseCleared = true;
            }
                
        }
        private void CloseViewWithOkEvent(object sender, EventArgs e)
        {
            ((PreferencesView)this.preferencesView).DialogResult = DialogResult.OK;
            ((PreferencesView)this.preferencesView).Close();
        }
        private void CloseViewWithCancelEvent(object sender, EventArgs e)
        {
            ((PreferencesView)this.preferencesView).DialogResult = DialogResult.Cancel;
            ((PreferencesView)this.preferencesView).Close();
        }
    }
}
