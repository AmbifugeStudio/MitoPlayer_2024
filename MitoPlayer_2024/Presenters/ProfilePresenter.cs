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
        private IProfileView view;
        private IProfileDao profileDao;
        private ISettingDao settingDao;
        private ITrackDao trackDao;
        private ITagDao tagDao;
        private BindingSource profileListBindingSource { get; set; }
        private DataTable profileListTable { get; set; }
        private Profile activeProfile { get; set; }

        public ProfilePresenter(IProfileView view, IProfileDao profileDao, ISettingDao settingDao, ITrackDao trackDao, ITagDao tagDao)
        {
            this.view = view;
            this.profileDao = profileDao;
            this.settingDao = settingDao;
            this.trackDao = trackDao;
            this.tagDao = tagDao;

            this.InitializeDataTable();

            this.view.CreateProfileEvent += CreateProfileEvent;
            this.view.SetProfileAsActiveEvent += SetProfileAsActiveEvent;
            this.view.RenameProfileEvent += RenameProfileEvent;
            this.view.DeleteProfileEvent += DeleteProfileEvent;
            this.view.CloseProfileViewEvent += CloseProfileViewEvent;
        }

        private void InitializeDataTable()
        {
            this.activeProfile = this.profileDao.GetActiveProfile();

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
                    this.profileListTable.Rows.Add(profile.Id, profile.IsActive, profile.Name);
                }
            }

            this.profileListBindingSource.DataSource = this.profileListTable;
            this.view.SetProfileListBindingSource(this.profileListBindingSource);
        }

        private void CreateProfileEvent(object sender, EventArgs e)
        {
            ProfileEditorView profileEditorView = new ProfileEditorView();
            ProfileEditorPresenter presenter = new ProfileEditorPresenter(profileEditorView, this.profileDao, this.settingDao);
            if (profileEditorView.ShowDialog((ProfileView)this.view) == DialogResult.OK)
            {
                this.profileDao.CreateProfile(presenter.newProfile);
                this.profileListTable.Rows.Add(presenter.newProfile.Id, presenter.newProfile.IsActive, presenter.newProfile.Name);
            }
        }

        private void SetProfileAsActiveEvent(object sender, Messenger e)
        {
            if(MessageBox.Show("Do you really want to change the profile? All playlist, track and metadata will be change!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                DataRow profileRow = this.profileListTable.Select("Id = " + e.IntegerField1).First();
                int profileIndex = this.profileListTable.Rows.IndexOf(profileRow);

                if ((bool)profileRow["Active"])
                {
                    MessageBox.Show("Profile is aready active!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    this.activeProfile.IsActive = false;
                    this.profileDao.UpdateProfile(this.activeProfile);

                    for (int i = 0; i <= this.profileListTable.Rows.Count - 1; i++)
                    {
                        this.profileListTable.Rows[i]["Active"] = false;
                    }

                    Profile profile = this.profileDao.GetProfile((int)profileRow["Id"]);
                    profile.IsActive = true;
                    this.profileDao.UpdateProfile(profile);

                    this.profileListTable.Rows[profileIndex]["Active"] = true;

                    ((ProfileView)this.view).DialogResult = DialogResult.OK;
                }
            }
        }
        private void RenameProfileEvent(object sender, Messenger e)
        {

            if (e.IntegerField1 == this.activeProfile.Id)
            {
                MessageBox.Show("Active profile cannot be renamed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DataRow profileRow = this.profileListTable.Select("Id = " + e.IntegerField1).First();
                if (profileRow["Name"].Equals("Default Profile"))
                {
                    MessageBox.Show("Default profile cannot be renamed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Profile profile = this.profileDao.GetProfile((int)profileRow["Id"]);
                    int profileIndex = this.profileListTable.Rows.IndexOf(profileRow);

                    ProfileEditorView profileEditorView = new ProfileEditorView();
                    ProfileEditorPresenter presenter = new ProfileEditorPresenter(profileEditorView, this.profileDao, this.settingDao, profile);
                   
                    if (profileEditorView.ShowDialog((ProfileView)this.view) == DialogResult.OK)
                    {
                        this.profileDao.UpdateProfile(presenter.newProfile);
                        this.profileListTable.Rows[profileIndex]["Name"] = presenter.newProfile?.Name;

                        this.profileListBindingSource.DataSource = this.profileListTable;
                        this.view.SetProfileListBindingSource(this.profileListBindingSource);
                    }
                }
            }
        }

        private void DeleteProfileEvent(object sender, Messenger e)
        {
            if (e.IntegerField1 == this.activeProfile.Id)
            {
                MessageBox.Show("Active profile cannot be deleted!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Do you really want to delete the profile? All playlist, track and metadata related to this profile will be deleted!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    this.settingDao.DeleteAllTrackProperty();
                    this.tagDao.DeleteAllTagValue();
                    this.tagDao.DeleteAllTag();
                    this.trackDao.DeleteAllPlaylistContent();
                    this.trackDao.DeleteAllTrackTagValue();
                    this.trackDao.DeleteAllTrack();
                    this.trackDao.DeleteAllPlaylist();
                    this.settingDao.DeleteSettings();

                    this.profileDao.DeleteProfile(e.IntegerField1);

                    DataRow profileRow = this.profileListTable.Select("Id = " + e.IntegerField1).First();
                    this.profileListTable.Rows.Remove(profileRow);
                    this.profileListBindingSource.DataSource = this.profileListTable;
                    this.view.SetProfileListBindingSource(this.profileListBindingSource);
                }
            }
        }

        private void CloseProfileViewEvent(object sender, EventArgs e)
        {
            ((ProfileView)this.view).DialogResult = DialogResult.Cancel;
            ((ProfileView)this.view).Close();
        }

 

    }
}
