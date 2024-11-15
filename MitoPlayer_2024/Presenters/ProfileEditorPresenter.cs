using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Presenters
{
    public class ProfileEditorPresenter
    {
        private IProfileEditorView view;
        private IProfileDao profileDao;
        private ISettingDao settingDao;

        private bool isEditMode = false;
        public Profile newProfile;
        private int lastGeneratedProfileId;

        public ProfileEditorPresenter(IProfileEditorView view, IProfileDao profileDao, ISettingDao settingDao)
        {
            this.view = view;
            this.profileDao = profileDao;
            this.settingDao = settingDao;
            this.isEditMode = false;

            this.lastGeneratedProfileId = this.settingDao.GetIntegerSetting(Settings.LastGeneratedProfileId.ToString(),true);
           
            this.lastGeneratedProfileId = this.lastGeneratedProfileId + 1;
            ((ProfileEditorView)this.view).SetProfileName("New Profile " + this.lastGeneratedProfileId.ToString());

            this.settingDao.SetIntegerSetting(Settings.LastGeneratedProfileId.ToString(), this.lastGeneratedProfileId, true);

            this.view.CreateOrEditProfile += CreateOrEditProfile;
            this.view.CloseProfileEditor += CloseProfileEditor;
        }

        public ProfileEditorPresenter(IProfileEditorView view, IProfileDao profileDao, ISettingDao settingDao, Profile profile)
        {
            this.view = view;
            this.profileDao = profileDao;
            this.settingDao = settingDao;
            this.newProfile = profile;
            this.isEditMode = true;

            ((ProfileEditorView)this.view).SetProfileName(profile.Name, true);

            this.view.CreateOrEditProfile += CreateOrEditProfile;
            this.view.CloseProfileEditor += CloseProfileEditor;
        }
        
        private void CreateOrEditProfile(object sender, Helpers.Messenger e)
        {
            ((ProfileEditorView)this.view).DialogResult = DialogResult.None;

            if (this.isEditMode)
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    if (e.StringField1.Equals(this.newProfile.Name))
                    {
                        ((ProfileEditorView)this.view).DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        Profile profile = this.profileDao.GetProfileByName(e.StringField1);
                        if (profile != null)
                        {
                            MessageBox.Show("Profile name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            this.newProfile.Name = e.StringField1;
                            ((ProfileEditorView)this.view).DialogResult = DialogResult.OK;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Profile name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    List<Profile> profileList = this.profileDao.GetAllProfile();
                    if (profileList != null && profileList.Count > 0)
                    {
                        if (profileList.Exists(x => x.Name.Equals(e.StringField1)))
                        {
                            MessageBox.Show("Profile name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Profile profile = new Profile();
                            profile.Id = this.profileDao.GetNextId(TableName.Profile.ToString());
                            profile.Name = e.StringField1;
                            this.newProfile = profile;
                            ((ProfileEditorView)this.view).DialogResult = DialogResult.OK;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Profile name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void CloseProfileEditor(object sender, EventArgs e)
        {
            ((ProfileEditorView)this.view).DialogResult = DialogResult.Cancel;
            ((ProfileEditorView)this.view).Close();
        }
    }
}
