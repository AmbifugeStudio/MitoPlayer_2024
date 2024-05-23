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
        private IProfileEditorView profileEditorView;
        private IProfileDao profileDao;
        private ISettingDao settingDao;

        private bool isEditMode = false;
        public Profile newProfile;
        private int lastGeneratedProfileId;

        public ProfileEditorPresenter(IProfileEditorView profileEditorView, IProfileDao profileDao, ISettingDao settingDao)
        {
            this.profileEditorView = profileEditorView;
            this.profileDao = profileDao;
            this.settingDao = settingDao;
            this.isEditMode = false;

            this.lastGeneratedProfileId = this.settingDao.GetIntegerSetting(Settings.LastGeneratedProfileId.ToString(),true);
           
            this.lastGeneratedProfileId = this.lastGeneratedProfileId + 1;
            ((ProfileEditorView)this.profileEditorView).SetProfileName("New Profile " + this.lastGeneratedProfileId.ToString());

            this.settingDao.SetIntegerSetting(Settings.LastGeneratedProfileId.ToString(), this.lastGeneratedProfileId, true);

            this.profileEditorView.CreateOrEditProfile += CreateOrEditProfile;
            this.profileEditorView.CloseProfileEditor += CloseProfileEditor;
        }

        public ProfileEditorPresenter(IProfileEditorView profileEditorView, IProfileDao profileDao, ISettingDao settingDao, Profile profile)
        {
            this.profileEditorView = profileEditorView;
            this.profileDao = profileDao;
            this.settingDao = settingDao;
            this.newProfile = profile;
            this.isEditMode = true;

            ((ProfileEditorView)this.profileEditorView).SetProfileName(profile.Name, true);

            this.profileEditorView.CreateOrEditProfile += CreateOrEditProfile;
            this.profileEditorView.CloseProfileEditor += CloseProfileEditor;
        }
        
        private void CreateOrEditProfile(object sender, Helpers.ListEventArgs e)
        {
            ((ProfileEditorView)this.profileEditorView).DialogResult = DialogResult.None;

            if (this.isEditMode)
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    if (e.StringField1.Equals(this.newProfile.Name))
                    {
                        ((ProfileEditorView)this.profileEditorView).DialogResult = DialogResult.OK;
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
                            try
                            {
                                this.newProfile.Name = e.StringField1;
                                ((ProfileEditorView)this.profileEditorView).DialogResult = DialogResult.OK;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Playlist hasn't been updated!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
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
                            try
                            {
                                Profile profile = new Profile();
                                profile.Id = this.GetNewProfileId();
                                profile.Name = e.StringField1;
                                this.newProfile = profile;
                                ((ProfileEditorView)this.profileEditorView).DialogResult = DialogResult.OK;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Profile hasn't been created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                    }
                    else
                    {
                        try
                        {
                            Profile profile = new Profile();
                            profile.Id = this.GetNewProfileId();
                            profile.Name = e.StringField1;
                            this.newProfile = profile;
                            ((ProfileEditorView)this.profileEditorView).DialogResult = DialogResult.OK;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Profile hasn't been created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Profile name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }



        }
        private int GetNewProfileId()
        {
            int id = this.profileDao.GetLastObjectId(TableName.Profile.ToString());
            if (id == -1)
                return 0;
            else
                return id + 1;
        }
        private void CloseProfileEditor(object sender, EventArgs e)
        {
            ((ProfileEditorView)this.profileEditorView).DialogResult = DialogResult.Cancel;
            ((ProfileEditorView)this.profileEditorView).Close();
        }
    }
}
