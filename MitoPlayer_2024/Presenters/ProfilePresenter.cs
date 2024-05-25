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
        private IPlaylistDao playlistDao;
        private ITrackDao trackDao;
        private BindingSource profileListBindingSource { get; set; }
        private DataTable profileListTable { get; set; }
        private Profile activeProfile { get; set; }

        public ProfilePresenter(IProfileView profileView, IProfileDao profileDao, ISettingDao settingDao, IPlaylistDao playlistDao, ITrackDao trackDao)
        {
            this.profileView = profileView;
            this.profileDao = profileDao;
            this.settingDao = settingDao;
            this.playlistDao = playlistDao;
            this.trackDao = trackDao;

            this.InitializeDataTable();

            this.profileView.CreateProfileEvent += CreateProfileEvent;
            this.profileView.SetProfileAsActiveEvent += SetProfileAsActiveEvent;
            this.profileView.RenameProfileEvent += RenameProfileEvent;
            this.profileView.DeleteProfileEvent += DeleteProfileEvent;
            this.profileView.CloseProfileViewEvent += CloseProfileViewEvent;
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
                    profileListTable.Rows.Add(profile.Id, profile.IsActive, profile.Name);
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

                    Profile profile = this.profileDao.GetProfile(Convert.ToInt32(this.profileListTable.Rows[profileIndex]["Id"]));
                    profile.IsActive = true;
                    this.profileDao.UpdateProfile(profile);

                    this.profileListTable.Rows[profileIndex]["Active"] = true;

                    ((ProfileView)this.profileView).DialogResult = DialogResult.OK;
                }
            }
        }
        private void RenameProfileEvent(object sender, ListEventArgs e)
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
                    Profile profile = new Profile();
                    profile.Id = (int)profileRow["Id"];
                    profile.Name = (string)profileRow["Name"];
                    profile.IsActive = (bool)profileRow["Active"];

                    int profileIndex = this.profileListTable.Rows.IndexOf(profileRow);

                    ProfileEditorView profileEditorView = new ProfileEditorView();
                    ProfileEditorPresenter presenter = new ProfileEditorPresenter(profileEditorView, this.profileDao, this.settingDao, profile);
                   
                    if (profileEditorView.ShowDialog((ProfileView)this.profileView) == DialogResult.OK)
                    {
                        this.profileDao.UpdateProfile(presenter.newProfile);
                        this.profileListTable.Rows[profileIndex]["Name"] = presenter.newProfile?.Name;

                        this.profileListBindingSource.DataSource = profileListTable;
                        this.profileView.SetProfileListBindingSource(this.profileListBindingSource);
                    }
                }
            }
        }

        private void DeleteProfileEvent(object sender, ListEventArgs e)
        {
            if (e.IntegerField1 == this.activeProfile.Id)
            {
                MessageBox.Show("Active profile cannot be deleted!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Do you really want to delete the profile? All playlist, track and metadata related to this profile will be deleted!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    this.playlistDao.DeleteAllPlaylistFromProfile(e.IntegerField1);
                    this.playlistDao.DeleteAllPlaylistContentFromProfile(e.IntegerField1);
                    this.trackDao.DeleteAllTrackFromProfile(e.IntegerField1);
                    this.profileDao.DeleteProfile(e.IntegerField1);

                    DataRow profileRow = this.profileListTable.Select("Id = " + e.IntegerField1).First();
                    this.profileListTable.Rows.Remove(profileRow);
                    this.profileListBindingSource.DataSource = profileListTable;
                    this.profileView.SetProfileListBindingSource(this.profileListBindingSource);
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
