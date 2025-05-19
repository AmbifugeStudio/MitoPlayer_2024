using MitoPlayer_2024.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class ExportToDirectoryView : Form, IExportToDirectoryView
    {
        public event EventHandler CloseViewWithOk;
        public event EventHandler CloseViewWithCancel;
        public event EventHandler BrowseEvent;
        public event EventHandler<Messenger> SetRowNumberEvent;
        public event EventHandler<Messenger> SetKeyCodeEvent;
        public event EventHandler<Messenger> SetBpmNumberEvent;
        public event EventHandler<Messenger> SetTrunkBpmEvent;
        public event EventHandler<Messenger> SetTrunkedArtistEvent;
        public event EventHandler<Messenger> SetTrunkedTitleEvent;
        public event EventHandler<Messenger> SetArtistMinimumCharacterEvent;
        public event EventHandler<Messenger> SetTitleMinimumCharacterEvent;
        private BindingSource trackListBindingSource { get; set; }
        public ExportToDirectoryView()
        {
            this.InitializeComponent();
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

            this.btnOk.BackColor = this.BackgroundColor;
            this.btnOk.ForeColor = this.FontColor;
            this.btnOk.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnBrowse.BackColor = this.BackgroundColor;
            this.btnBrowse.ForeColor = this.FontColor;
            this.btnBrowse.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnCancel.BackColor = this.BackgroundColor;
            this.btnCancel.ForeColor = this.FontColor;
            this.btnCancel.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.grbFileNamePrefix.ForeColor = this.FontColor;
            this.grbFileName.ForeColor = this.FontColor;

            this.dgvTrackList.BackgroundColor = this.ButtonColor;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvTrackList.EnableHeadersVisualStyles = false;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvTrackList.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;

        }

        public void InitializeView(
            String path,
            bool isRowNumberChecked,
            bool isKeyCodeChecked,
            bool isBpmNumberChecked, 
            bool isTrunkBpmChecked,
            bool isTrunkedArtistChecked,
            bool isTrunkedTitleChecked, 
            decimal artistMinimumCharacter, 
            decimal titleMinimumCharacter)
        {
            this.txtBoxPath.Text = path;
            this.chbRowNumber.Checked = isRowNumberChecked;
            this.chbKeyCode.Checked = isKeyCodeChecked;
            this.chbBpmNumber.Checked = isBpmNumberChecked;
            this.chbTrunkBpm.Checked = isTrunkBpmChecked;
            this.chbTrunkArtist.Checked = isTrunkedArtistChecked;
            this.chbTrunkTitle.Checked = isTrunkedTitleChecked;
            this.numArtist.Value = artistMinimumCharacter;
            this.numTitle.Value = titleMinimumCharacter;
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.BrowseEvent?.Invoke(this, new EventArgs() );
        }

        private void chbRowNumber_CheckedChanged(object sender, EventArgs e)
        {
            this.SetRowNumberEvent?.Invoke(this, new Messenger() { BooleanField1 = this.chbRowNumber.Checked });
        }

        private void chbKeyCode_CheckedChanged(object sender, EventArgs e)
        {
            this.SetKeyCodeEvent?.Invoke(this, new Messenger() { BooleanField1 = this.chbKeyCode.Checked });
        }

        private void chbBpmNumber_CheckedChanged(object sender, EventArgs e)
        {
            this.SetBpmNumberEvent?.Invoke(this, new Messenger() { BooleanField1 = this.chbBpmNumber.Checked });
        }

        private void chbTrunkBpm_CheckedChanged(object sender, EventArgs e)
        {
            this.SetTrunkBpmEvent?.Invoke(this, new Messenger() { BooleanField1 = this.chbTrunkBpm.Checked });
        }
        private void chbTrunkArtist_CheckedChanged(object sender, EventArgs e)
        {
            this.SetTrunkedArtistEvent?.Invoke(this, new Messenger() { BooleanField1 = this.chbTrunkArtist.Checked });
        }

        private void chbTrunkTitle_CheckedChanged(object sender, EventArgs e)
        {
            this.SetTrunkedTitleEvent?.Invoke(this, new Messenger() { BooleanField1 = this.chbTrunkTitle.Checked });
        }

        private void numArtist_ValueChanged(object sender, EventArgs e)
        {
            this.SetArtistMinimumCharacterEvent?.Invoke(this, new Messenger() { DecimalField1 = this.numArtist.Value });
        }

        private void numTitle_ValueChanged(object sender, EventArgs e)
        {
            this.SetTitleMinimumCharacterEvent?.Invoke(this, new Messenger() { DecimalField1 = this.numTitle.Value });
        }

        private void ExportToDirectoryView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                ;
            }
        }
    }

}
