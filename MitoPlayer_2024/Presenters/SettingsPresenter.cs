using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    internal class SettingsPresenter
    {
        private ISettingsView view;
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
        private bool isLogMessageEnabled;
        private decimal logMessageDisplayTime;

        public bool databaseCleared = false;
        public SettingsPresenter(ISettingsView view, ITrackDao trackDao,ITagDao tagDao, IProfileDao profileDao, ISettingDao settingDao)
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
            this.isLogMessageEnabled = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value;
            this.logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString());

            this.hasVirtualDj = this.HasVirtualDj();

            this.view.SetImportSettings(
                this.automaticBpmImport, 
                this.automaticKeyImport, 
                this.virtualDjDatabasePath,
                this.playTrackAfterOpenFiles,
                this.hasVirtualDj,
                this.previewPercentage,
                this.isShortTrackColouringEnabled,
                this.shortTrackColouringThreshold,
                this.isLogMessageEnabled,
                this.logMessageDisplayTime);

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
            this.view.SetLogMessageDisplayTimeEvent += SetLogMessageDisplayTimeEvent;
            this.view.SetLogMessageEnabledEvent += SetLogMessageEnabledEvent;
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
           
            this.shortTrackColouringThreshold = e.DecimalField1;
        }

        private void SetShortTrackColouringEvent(object sender, Messenger e)
        {
            this.isShortTrackColouringEnabled = e.BooleanField1;
        }
        private void SetLogMessageEnabledEvent(object sender, Messenger e)
        {
            this.isLogMessageEnabled = e.BooleanField1;
        }

        private void SetLogMessageDisplayTimeEvent(object sender, Messenger e)
        {
            this.logMessageDisplayTime = Convert.ToInt32(e.DecimalField1);
        }
        private void CloseViewWithOkEvent(object sender, EventArgs e)
        {
            ResultOrError result = new ResultOrError();

            if (this.isShortTrackColouringEnabled)
            {
                if(this.shortTrackColouringThreshold <= 0)
                {
                    String error = "Track colouring threshold must be set!";
                    MessageBox.Show(error, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    result.AddError(error);
                }
            }
            else
            {
                this.shortTrackColouringThreshold = 0;
            }
            if (result.Success)
            {
                if (MessageBox.Show("Application restart is required.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    this.settingDao.SetBooleanSetting(Settings.AutomaticBpmImport.ToString(), this.automaticBpmImport);
                    this.settingDao.SetBooleanSetting(Settings.AutomaticKeyImport.ToString(), this.automaticKeyImport);
                    this.settingDao.SetStringSetting(Settings.VirtualDjDatabasePath.ToString(), this.virtualDjDatabasePath);
                    this.settingDao.SetBooleanSetting(Settings.PlayTrackAfterOpenFiles.ToString(), this.playTrackAfterOpenFiles);
                    this.settingDao.SetBooleanSetting(Settings.PlayTrackAfterOpenFiles.ToString(), this.playTrackAfterOpenFiles);
                    this.settingDao.SetIntegerSetting(Settings.PreviewPercentage.ToString(), this.previewPercentage);

                    this.settingDao.SetBooleanSetting(Settings.IsShortTrackColouringEnabled.ToString(), this.isShortTrackColouringEnabled);
                    this.settingDao.SetDecimalSetting(Settings.ShortTrackColouringThreshold.ToString(), this.shortTrackColouringThreshold);
                    this.settingDao.SetBooleanSetting(Settings.IsLogMessageEnabled.ToString(), this.isLogMessageEnabled);
                    this.settingDao.SetDecimalSetting(Settings.LogMessageDisplayTime.ToString(), this.logMessageDisplayTime);

                    Process.Start(Application.ExecutablePath);
                    Application.Exit();
                }
                
            }
            /* if (!File.Exists(this.virtualDjDatabasePath))
             {
                 MessageBox.Show("VirtualDJ database file does not exists!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 this.view.SetImportSettings(this.automaticBpmImport, this.automaticKeyImport, this.virtualDjDatabasePath, this.playTrackAfterOpenFiles);
             }
             else
             {

             }*/
            

        }
        private void CloseViewWithCancelEvent(object sender, EventArgs e)
        {
            ((SettingsView)this.view).DialogResult = DialogResult.Cancel;
            ((SettingsView)this.view).Close();
        }
    }
}
