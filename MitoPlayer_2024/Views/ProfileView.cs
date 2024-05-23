using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class ProfileView : Form, IProfileView
    {
        private BindingSource profiletListBindingSource { get; set; }
        public event EventHandler CreateProfileEvent;
        public event EventHandler<ListEventArgs> SetProfileAsActiveEvent;
        public event EventHandler<ListEventArgs> RenameProfileEvent;
        public event EventHandler<ListEventArgs> DeleteProfileEvent;
        public event EventHandler CloseProfileViewEvent;

        public ProfileView()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public void SetProfileListBindingSource(BindingSource profileList)
        {
            this.profiletListBindingSource = new BindingSource();
            this.profiletListBindingSource.DataSource = profileList;
            this.dgvProfileList.DataSource = this.profiletListBindingSource.DataSource;
            this.dgvProfileList.Columns["Id"].Visible = false;
            this.dgvProfileList.Columns["Active"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvProfileList.Columns["Active"].Width = 50;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.CreateProfileEvent?.Invoke(this, EventArgs.Empty);
        }

        private void btnSetAsActive_Click(object sender, EventArgs e)
        {
            if(this.dgvProfileList.SelectedRows != null && this.dgvProfileList.SelectedRows.Count > 0)
            {
                this.SetProfileAsActiveEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvProfileList.SelectedRows[0].Cells["Id"].Value) });
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (this.dgvProfileList.SelectedRows != null && this.dgvProfileList.SelectedRows.Count > 0)
            {
                this.RenameProfileEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvProfileList.SelectedRows[0].Cells["Id"].Value) });
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvProfileList.SelectedRows != null && this.dgvProfileList.SelectedRows.Count > 0)
            {
                this.DeleteProfileEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvProfileList.SelectedRows[0].Cells["Id"].Value) });
            }
        }

        private void ProfileView_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyCode == Keys.Escape)
            {
                this.CloseProfileViewEvent?.Invoke(this, new EventArgs());
            }
        }
    }
}
