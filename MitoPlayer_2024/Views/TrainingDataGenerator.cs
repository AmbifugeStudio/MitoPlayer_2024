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
    public partial class TrainingDataGenerator : Form, ITrainingDataGenerator
    {
        public event EventHandler CloseViewWithOk;
        public event EventHandler CloseViewWithCancel;
        private BindingSource trackListBindingSource { get; set; }
        public TrainingDataGenerator()
        {
            InitializeComponent();
            this.SetControlColors();
            this.CenterToScreen();
        }
        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonColor = System.Drawing.ColorTranslator.FromHtml("#292a2d");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");
        Color GridLineColor1 = System.Drawing.ColorTranslator.FromHtml("#131315");
        Color GridLineColor2 = System.Drawing.ColorTranslator.FromHtml("#212224");
        Color GridPlayingColor = System.Drawing.ColorTranslator.FromHtml("#4d4d4d");
        Color GridSelectionColor = System.Drawing.ColorTranslator.FromHtml("#626262");
        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

          /*this.btnOk.BackColor = this.BackgroundColor;
            this.btnOk.ForeColor = this.FontColor;
            this.btnOk.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnBrowse.BackColor = this.BackgroundColor;
            this.btnBrowse.ForeColor = this.FontColor;
            this.btnBrowse.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnCancel.BackColor = this.BackgroundColor;
            this.btnCancel.ForeColor = this.FontColor;
            this.btnCancel.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.dgvTrackList.BackgroundColor = this.ButtonColor;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvTrackList.EnableHeadersVisualStyles = false;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvTrackList.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;*/

        }

        public void InitializeView()
        {
            throw new NotImplementedException();
        }

        public void SetTrackListBindingSource(BindingSource trackList)
        {
            this.trackListBindingSource = new BindingSource();
            this.trackListBindingSource.DataSource = trackList;
            this.dgvTrackList.DataSource = this.trackListBindingSource.DataSource;
            this.SetTrackListColors();
        }
        private void dgvTrackList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.SetTrackListColors();
        }
        public void SetTrackListColors()
        {
            if (this.dgvTrackList != null && this.dgvTrackList.Rows != null && this.dgvTrackList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
                {
                    if (i == 0 || i % 2 == 0)
                    {
                        this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor1;
                    }
                    else
                    {
                        this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor2;
                    }
                }
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseViewWithOk?.Invoke(this, new EventArgs());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseViewWithCancel?.Invoke(this, new EventArgs());
        }
    }
}
