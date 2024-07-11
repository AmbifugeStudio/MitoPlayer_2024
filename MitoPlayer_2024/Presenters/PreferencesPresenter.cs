using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    internal class PreferencesPresenter
    {
        private IPreferencesView view;
        private ITrackDao trackDao;
        private ITagDao tagDao;
        private IProfileDao profileDao;
        private ISettingDao settingDao;

        private bool automaticBpmImport; 
        private bool automaticKeyImport;
        private String virtualDjDefaultDatabasePath;

        public bool databaseCleared = false;
        public PreferencesPresenter(IPreferencesView view, ITrackDao trackDao,ITagDao tagDao, IProfileDao profileDao, ISettingDao settingDao)
        {
            this.view = view;
            this.trackDao = trackDao;
            this.tagDao = tagDao;
            this.profileDao = profileDao;
            this.settingDao = settingDao;

            this.automaticBpmImport = this.settingDao.GetBooleanSetting(Settings.AutomaticBpmImport.ToString()).Value;
            this.automaticKeyImport = this.settingDao.GetBooleanSetting(Settings.AutomaticKeyImport.ToString()).Value;
            this.virtualDjDefaultDatabasePath = this.settingDao.GetStringSetting(Settings.VirtualDjDefaultDatabasePath.ToString());
            
            this.view.SetImportSettings(this.automaticBpmImport, this.automaticKeyImport, this.virtualDjDefaultDatabasePath);

            this.view.CloseViewWithOkEvent += CloseViewWithOkEvent;
            this.view.CloseViewWithCancelEvent += CloseViewWithCancelEvent;
            this.view.ClearDatabaseEvent += ClearDatabaseEvent;
            this.view.SetAutomaticBpmImportEvent += SetAutomaticBpmImportEvent;
            this.view.SetAutomaticKeyImportEvent += SetAutomaticKeyImportEvent;
        }

        private void ClearDatabaseEvent(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want delete the content of the database? All data will be lost!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.settingDao.ClearTrackPropertyTable();

                this.tagDao.ClearTagTable();
                this.trackDao.ClearTrackTable();
                this.trackDao.ClearPlaylistTable();
                this.settingDao.ClearSettingTable();
                this.profileDao.ClearProfileTable();
                
                this.databaseCleared = true;
            }
                
        }
        private void SetAutomaticBpmImportEvent(object sender, ListEventArgs e)
        {
            this.automaticBpmImport = e.BooleanField1;            
        }
        private void SetAutomaticKeyImportEvent(object sender, ListEventArgs e)
        {
            this.automaticKeyImport = e.BooleanField1;
        }
        private void CloseViewWithOkEvent(object sender, EventArgs e)
        {
            if (!File.Exists(this.virtualDjDefaultDatabasePath))
            {
                MessageBox.Show("VirtualDJ database file does not exists!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.settingDao.SetBooleanSetting(Settings.AutomaticBpmImport.ToString(), this.automaticBpmImport);
                this.settingDao.SetBooleanSetting(Settings.AutomaticKeyImport.ToString(), this.automaticKeyImport);
                this.settingDao.SetStringSetting(Settings.VirtualDjDefaultDatabasePath.ToString(), this.virtualDjDefaultDatabasePath);

                ((PreferencesView)this.view).DialogResult = DialogResult.OK;
                ((PreferencesView)this.view).Close();
            }

           
        }
        private void CloseViewWithCancelEvent(object sender, EventArgs e)
        {
            ((PreferencesView)this.view).DialogResult = DialogResult.Cancel;
            ((PreferencesView)this.view).Close();
        }
    }
}
