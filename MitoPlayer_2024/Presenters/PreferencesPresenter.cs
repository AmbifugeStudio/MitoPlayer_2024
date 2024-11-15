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
        private String virtualDjDatabasePath;
        private bool playTrackAfterOpenFiles;
        private bool hasVirtualDj;
        private int previewPercentage;
        private bool isShortTrackColouringEnabled;
        private decimal shortTrackColouringThreshold;

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
            this.virtualDjDatabasePath = this.settingDao.GetStringSetting(Settings.VirtualDjDatabasePath.ToString());
            this.playTrackAfterOpenFiles = this.settingDao.GetBooleanSetting(Settings.PlayTrackAfterOpenFiles.ToString()).Value;
            this.previewPercentage = this.settingDao.GetIntegerSetting(Settings.PreviewPercentage.ToString());
            this.isShortTrackColouringEnabled = this.settingDao.GetBooleanSetting(Settings.IsShortTrackColouringEnabled.ToString()).Value;
            this.shortTrackColouringThreshold = this.settingDao.GetDecimalSetting(Settings.ShortTrackColouringThreshold.ToString());

            this.hasVirtualDj = this.HasVirtualDj();

            this.view.SetImportSettings(
                this.automaticBpmImport, 
                this.automaticKeyImport, 
                this.virtualDjDatabasePath,
                this.playTrackAfterOpenFiles,
                this.hasVirtualDj,
                this.previewPercentage,
                this.isShortTrackColouringEnabled,
                this.shortTrackColouringThreshold);

            this.view.CloseViewWithOkEvent += CloseViewWithOkEvent;
            this.view.CloseViewWithCancelEvent += CloseViewWithCancelEvent;
            this.view.ClearDatabaseEvent += ClearDatabaseEvent;
            this.view.SetAutomaticBpmImportEvent += SetAutomaticBpmImportEvent;
            this.view.SetAutomaticKeyImportEvent += SetAutomaticKeyImportEvent;
            this.view.SetVirtualDjDatabasePathEvent += SetVirtualDjDatabasePathEvent;
            this.view.SetPlayTrackAfterOpenFilesEvent += SetPlayTrackAfterOpenFilesEvent;
            this.view.SetPreviewPercentageEvent += SetPreviewPercentageEvent;
            this.view.SetShortTrackColouringEvent += SetShortTrackColouringEvent;
            this.view.SetShortTrackColouringThresholdEvent += SetShortTrackColouringThresholdEvent;

        }

        

        private bool HasVirtualDj()
        {
            bool result = false;

            String letters = "ABCDEFGHIJKLMNOPQRSTIJKLMNOPQRSTUVWXYZ";
            String vdjDatabaseFilePath = String.Empty;

            foreach (char drive in letters)
            {
                vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";
                if (File.Exists(vdjDatabaseFilePath))
                {
                    result = true;
                    break;
                }
            }
            
            vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VirtualDJ\\database.xml";
            if (File.Exists(vdjDatabaseFilePath))
            {
                result = true;
            }

            return result;
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
        private void SetAutomaticBpmImportEvent(object sender, Messenger e)
        {
            this.automaticBpmImport = e.BooleanField1;            
        }
        private void SetAutomaticKeyImportEvent(object sender, Messenger e)
        {
            this.automaticKeyImport = e.BooleanField1;
        }
        private void SetVirtualDjDatabasePathEvent(object sender, Messenger e)
        {
            this.virtualDjDatabasePath = e.StringField1;
        }
        private void SetPlayTrackAfterOpenFilesEvent(object sender, Messenger e)
        {
            this.playTrackAfterOpenFiles = e.BooleanField1;
        }

        private void SetPreviewPercentageEvent(object sender, Messenger e)
        {
            this.previewPercentage = Convert.ToInt32(e.DecimalField1);
        }

        private void SetShortTrackColouringThresholdEvent(object sender, Messenger e)
        {
           this.isShortTrackColouringEnabled = e.BooleanField1;
        }

        private void SetShortTrackColouringEvent(object sender, Messenger e)
        {
            this.shortTrackColouringThreshold = Convert.ToInt32(e.DecimalField1);
        }
        private void CloseViewWithOkEvent(object sender, EventArgs e)
        {
           /* if (!File.Exists(this.virtualDjDatabasePath))
            {
                MessageBox.Show("VirtualDJ database file does not exists!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.view.SetImportSettings(this.automaticBpmImport, this.automaticKeyImport, this.virtualDjDatabasePath, this.playTrackAfterOpenFiles);
            }
            else
            {
                
            }*/
            this.settingDao.SetBooleanSetting(Settings.AutomaticBpmImport.ToString(), this.automaticBpmImport);
            this.settingDao.SetBooleanSetting(Settings.AutomaticKeyImport.ToString(), this.automaticKeyImport);
            this.settingDao.SetStringSetting(Settings.VirtualDjDatabasePath.ToString(), this.virtualDjDatabasePath);
            this.settingDao.SetBooleanSetting(Settings.PlayTrackAfterOpenFiles.ToString(), this.playTrackAfterOpenFiles);
            this.settingDao.SetBooleanSetting(Settings.PlayTrackAfterOpenFiles.ToString(), this.playTrackAfterOpenFiles);
            this.settingDao.SetBooleanSetting(Settings.IsShortTrackColouringEnabled.ToString(), this.isShortTrackColouringEnabled);
            this.settingDao.SetDecimalSetting(Settings.ShortTrackColouringThreshold.ToString(), this.shortTrackColouringThreshold);

            ((PreferencesView)this.view).DialogResult = DialogResult.OK;
            ((PreferencesView)this.view).Close();

        }
        private void CloseViewWithCancelEvent(object sender, EventArgs e)
        {
            ((PreferencesView)this.view).DialogResult = DialogResult.Cancel;
            ((PreferencesView)this.view).Close();
        }
    }
}
