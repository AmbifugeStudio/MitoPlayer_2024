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
            this.InitializeComponent();
            this.SetControlColors();
            this.CenterToScreen();
        }

        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonColor = System.Drawing.ColorTranslator.FromHtml("#292a2d");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");
        Color GridHeaderColor = System.Drawing.ColorTranslator.FromHtml("#36373a");
        Color GridLineColor1 = System.Drawing.ColorTranslator.FromHtml("#131315");
        Color GridLineColor2 = System.Drawing.ColorTranslator.FromHtml("#212224");
        Color WhiteColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        Color GridPlayingColor = System.Drawing.ColorTranslator.FromHtml("#4d4d4d");
        Color GridSelectionColor = System.Drawing.ColorTranslator.FromHtml("#626262");

        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.btnCreate.BackColor = this.BackgroundColor;
            this.btnCreate.ForeColor = this.FontColor;
            this.btnCreate.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnRename.BackColor = this.BackgroundColor;
            this.btnRename.ForeColor = this.FontColor;
            this.btnRename.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnSetAsActive.BackColor = this.BackgroundColor;
            this.btnSetAsActive.ForeColor = this.FontColor;
            this.btnSetAsActive.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnDelete.BackColor = this.BackgroundColor;
            this.btnDelete.ForeColor = this.FontColor;
            this.btnDelete.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.dgvProfileList.BackgroundColor = this.ButtonColor;
            this.dgvProfileList.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvProfileList.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvProfileList.EnableHeadersVisualStyles = false;
            this.dgvProfileList.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvProfileList.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;

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
