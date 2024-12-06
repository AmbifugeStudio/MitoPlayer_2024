using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.IViews;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    internal class LiveStreamAnimationSettingPresenter
    {
        private ILiveStreamAnimationSettingView view;
        private ISettingDao settingDao { get; set; }

        private String imagePath;

        public LiveStreamAnimationSettingPresenter(ILiveStreamAnimationSettingView liveStreamAnimationView, ISettingDao settingDao)
        {
            this.view = liveStreamAnimationView;
            this.settingDao = settingDao;

            this.imagePath = this.settingDao.GetStringSetting(Settings.LiveStreamAnimationImagePath.ToString());

            this.view.SetImagePathEvent += SetImagePathEvent;
            this.view.BrowseDirectoryEvent += BrowseDirectoryEvent;
            this.view.CloseViewWithOkEvent += CloseViewWithOkEvent;
            this.view.CloseViewWithCancelEvent += CloseViewWithCancelEvent;

            this.InitializeView();
        }
        private void InitializeView()
        {
            Messenger messenger = new Messenger();
            messenger.StringField1 = this.imagePath;

            this.view.InitializeView(messenger);
        }
        private void BrowseDirectoryEvent(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                String lastDirectoryPath = this.settingDao.GetStringSetting(Settings.LiveStreamAnimationImagePath.ToString());

                if (Directory.Exists(lastDirectoryPath))
                    fbd.SelectedPath = lastDirectoryPath;

                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if(this.imagePath != fbd.SelectedPath)
                    {
                        bool imagesFound = false;
                        foreach (string extension in imageExtensions)
                        {
                            string[] files = Directory.GetFiles(fbd.SelectedPath, "*" + extension);
                            if (files.Length > 0)
                            {
                                imagesFound = true;
                                break;
                            }
                        }
                        if (imagesFound)
                        {
                            this.imagePath = fbd.SelectedPath;
                        }
                        else
                        {
                            MessageBox.Show("Directory does not have images!\n", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.imagePath = null;
                        }

                        this.InitializeView();
                    }
                   
                }

                this.settingDao.SetStringSetting(Settings.LiveStreamAnimationImagePath.ToString(), fbd.SelectedPath);
            }

        }
        private string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        private void SetImagePathEvent(object sender, Messenger e)
        {
            if (Directory.Exists(e.StringField1))
            {
                bool imagesFound = false;
                foreach (string extension in imageExtensions)
                {
                    string[] files = Directory.GetFiles(e.StringField1, "*" + extension);
                    if (files.Length > 0)
                    {
                        imagesFound = true;
                        break;
                    }
                }
                if (imagesFound)
                {
                    this.imagePath = e.StringField1;
                }
                else
                {
                    MessageBox.Show("Directory does not have images!\n", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.InitializeView();
                }
                   
            }
            else
            {
                MessageBox.Show("Directory does not exist.\n", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.InitializeView();
            }
        }

        
        private void CloseViewWithOkEvent(object sender, EventArgs e)
        {
            this.settingDao.SetStringSetting(Settings.LiveStreamAnimationImagePath.ToString(), this.imagePath);
            ((LiveStreamAnimationSettingView)this.view).DialogResult = DialogResult.OK;
            ((LiveStreamAnimationSettingView)this.view).Close();
        }
        private void CloseViewWithCancelEvent(object sender, EventArgs e)
        {
            ((LiveStreamAnimationSettingView)this.view).DialogResult = DialogResult.Cancel;
            ((LiveStreamAnimationSettingView)this.view).Close();
        }

        
    }
    
}
