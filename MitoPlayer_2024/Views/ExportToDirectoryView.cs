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
    public partial class ExportToDirectoryView : Form, IExportToDirectoryView
    {
        public event EventHandler CloseViewWithOk;
        public event EventHandler CloseViewWithCancel;
        public event EventHandler BrowseEvent;
        public event EventHandler<ListEventArgs> SetRowNumberEvent;
        public event EventHandler<ListEventArgs> SetKeyCodeEvent;
        public event EventHandler<ListEventArgs> SetBpmNumberEvent;
        public event EventHandler<ListEventArgs> SetTrunkBpmEvent;
        public event EventHandler<ListEventArgs> SetTrunkedArtistEvent;
        public event EventHandler<ListEventArgs> SetTrunkedTitleEvent;
        public event EventHandler<ListEventArgs> SetArtistMinimumCharacterEvent;
        public event EventHandler<ListEventArgs> SetTitleMinimumCharacterEvent;
        private BindingSource trackListBindingSource { get; set; }
        public ExportToDirectoryView()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public void InitializeView(String path,bool isRowNumberChecked,bool isKeyCodeChecked,bool isBpmNumberChecked, bool isTrunkBpmChecked,
            bool isTrunkedArtistChecked,bool isTrunkedTitleChecked, decimal artistMinimumCharacter, decimal titleMinimumCharacter)
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
            this.SetRowNumberEvent?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbRowNumber.Checked });
        }

        private void chbKeyCode_CheckedChanged(object sender, EventArgs e)
        {
            this.SetKeyCodeEvent?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbKeyCode.Checked });
        }

        private void chbBpmNumber_CheckedChanged(object sender, EventArgs e)
        {
            this.SetBpmNumberEvent?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbBpmNumber.Checked });
        }

        private void chbTrunkBpm_CheckedChanged(object sender, EventArgs e)
        {
            this.SetTrunkBpmEvent?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbTrunkBpm.Checked });
        }
        private void chbTrunkArtist_CheckedChanged(object sender, EventArgs e)
        {
            this.SetTrunkedArtistEvent?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbTrunkArtist.Checked });
        }

        private void chbTrunkTitle_CheckedChanged(object sender, EventArgs e)
        {
            this.SetTrunkedTitleEvent?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbTrunkTitle.Checked });
        }

        private void numArtist_ValueChanged(object sender, EventArgs e)
        {
            this.SetArtistMinimumCharacterEvent?.Invoke(this, new ListEventArgs() { DecimalField1 = this.numArtist.Value });
        }

        private void numTitle_ValueChanged(object sender, EventArgs e)
        {
            this.SetTitleMinimumCharacterEvent?.Invoke(this, new ListEventArgs() { DecimalField1 = this.numTitle.Value });
        }

        
    }

}
