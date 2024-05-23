using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Presenters
{
    public class ProfilePresenter
    {
        private IProfileView profileView;
        private IProfileDao profileDao;
        private ISettingDao settingDao;
        private BindingSource profileListBindingSource { get; set; }
        private DataTable profileListTable { get; set; }
        private int currentProfileId { get; set; }

        public ProfilePresenter(IProfileView profileView, IProfileDao profileDao, ISettingDao settingDao)
        {
            this.profileView = profileView;
            this.profileDao = profileDao;
            this.settingDao = settingDao;

            this.InitializeDataTable();

            this.profileView.CreateProfileEvent += CreateProfileEvent;
            this.profileView.SetProfileAsActiveEvent += SetProfileAsActiveEvent;
            this.profileView.RenameProfileEvent += RenameProfileEvent;
            this.profileView.DeleteProfileEvent += DeleteProfileEvent;
            this.profileView.CloseProfileViewEvent += CloseProfileViewEvent;
        }

        private void InitializeDataTable()
        {
            this.currentProfileId = this.settingDao.GetIntegerSetting(Settings.CurrentProfileId.ToString(), true);

            this.profileListBindingSource = new BindingSource();
            this.profileListTable = new DataTable();
            this.profileListTable.Columns.Add("Id", typeof(Int32));
            this.profileListTable.Columns.Add("Active", typeof(bool));
            this.profileListTable.Columns.Add("Name", typeof(String));

            List<Profile> profileList = this.profileDao.GetAllProfile();
            if(profileList != null && profileList.Count > 0)
            {
                foreach(Profile profile in profileList)
                {
                    if(profile.Id == currentProfileId)
                    {
                        profileListTable.Rows.Add(profile.Id, true, profile.Name);
                    }
                    else
                    {
                        profileListTable.Rows.Add(profile.Id, false, profile.Name);
                    }
                }
            }

            this.profileListBindingSource.DataSource = profileListTable;
            this.profileView.SetProfileListBindingSource(this.profileListBindingSource);
        }

        private void CreateProfileEvent(object sender, EventArgs e)
        {
            ProfileEditorView profileEditorView = new ProfileEditorView();
            ProfileEditorPresenter presenter = new ProfileEditorPresenter(profileEditorView, this.profileDao, this.settingDao);
            if (profileEditorView.ShowDialog((ProfileView)this.profileView) == DialogResult.OK)
            {
                this.profileDao.CreateProfile(presenter.newProfile);
                this.profileListTable.Rows.Add(presenter.newProfile.Id, presenter.newProfile.IsActive, presenter.newProfile.Name);
            }
        }

        private void SetProfileAsActiveEvent(object sender, ListEventArgs e)
        {
            if(MessageBox.Show("Do you really want to change the profile? All playlist, track and metadata will be change!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                for (int i = 0; i <= this.profileListTable.Rows.Count - 1; i++)
                {
                    if (Convert.ToInt32(this.profileListTable.Rows[i]["Id"]) == e.IntegerField1)
                    {
                        if (Convert.ToBoolean(this.profileListTable.Rows[i]["Active"]))
                        {
                            MessageBox.Show("Profile is aready active!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            this.profileListTable.Rows[i]["Active"] = true;
                        }

                    }
                    else
                    {
                        this.profileListTable.Rows[i]["Active"] = false;
                    }
                }

                this.settingDao.SetIntegerSetting(Settings.CurrentProfileId.ToString(), e.IntegerField1, true);
                this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), -1, true);

                ((ProfileView)this.profileView).DialogResult = DialogResult.OK;
            }
            
        }
        private void RenameProfileEvent(object sender, ListEventArgs e)
        {
            Profile profile = null;
            int profileIndex = 0;
            for (int i = 0; i <= this.profileListTable.Rows.Count - 1; i++)
            {
                if (Convert.ToInt32(this.profileListTable.Rows[i]["Id"]) == e.IntegerField1)
                {
                    profile = new Profile();
                    profile.Id = Convert.ToInt32(this.profileListTable.Rows[i]["Id"]);
                    profile.IsActive = Convert.ToBoolean(this.profileListTable.Rows[i]["Active"]);
                    profile.Name = this.profileListTable.Rows[i]["Name"].ToString();
                    profileIndex = i;
                    break;
                }
            }

            String defaultProfileName = this.settingDao.GetStringSetting(Settings.DefaultProfileName.ToString(), true);
            if(profile != null && profile.Name.Equals(defaultProfileName))
            {
                MessageBox.Show("Default profile cannot be renamed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                ProfileEditorView profileEditorView = new ProfileEditorView();
                ProfileEditorPresenter presenter = new ProfileEditorPresenter(profileEditorView, this.profileDao, this.settingDao, profile);
                if (profileEditorView.ShowDialog((ProfileView)this.profileView) == DialogResult.OK)
                {
                    this.profileDao.UpdateProfile(presenter.newProfile);
                    this.profileListTable.Rows[profileIndex]["Name"] = presenter.newProfile?.Name;
                    this.profileListBindingSource.DataSource = profileListTable;
                    this.profileView.SetProfileListBindingSource(this.profileListBindingSource);
                }}

            
        }

        private void DeleteProfileEvent(object sender, ListEventArgs e)
        {
            for (int i = 0; i <= this.profileListTable.Rows.Count - 1; i++)
            {
                if (Convert.ToInt32(this.profileListTable.Rows[i]["Id"]) == e.IntegerField1)
                {
                    this.profileDao.DeleteProfile(e.IntegerField1);
                    this.profileListTable.Rows.RemoveAt(i);
                    this.profileListBindingSource.DataSource = profileListTable;
                    this.profileView.SetProfileListBindingSource(this.profileListBindingSource);
                    break;
                }
            }

        }

        private void CloseProfileViewEvent(object sender, EventArgs e)
        {
            ((ProfileView)this.profileView).DialogResult = DialogResult.Cancel;
            ((ProfileView)this.profileView).Close();
        }

    }
}
