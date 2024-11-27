using MitoPlayer_2024.Helpers;
using Mysqlx;
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
    public partial class SettingsView : Form, IPreferencesView
    {
        public event EventHandler CloseViewWithOkEvent;
        public event EventHandler CloseViewWithCancelEvent;
        public event EventHandler ClearDatabaseEvent;
        public event EventHandler<Messenger> SetAutomaticBpmImportEvent;
        public event EventHandler<Messenger> SetAutomaticKeyImportEvent;
        public event EventHandler<Messenger> SetVirtualDjDatabasePathEvent;
        public event EventHandler<Messenger> SetPlayTrackAfterOpenFilesEvent;
        public event EventHandler<Messenger> SetPreviewPercentageEvent;
        public event EventHandler<Messenger> SetShortTrackColouringEvent;
        public event EventHandler<Messenger> SetShortTrackColouringThresholdEvent;



        public SettingsView()
        {
            this.InitializeComponent();
            this.SetControlColors();
            this.CenterToScreen();
        }

        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");

        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.btnOk.BackColor = this.BackgroundColor;
            this.btnOk.ForeColor = this.FontColor;
            this.btnOk.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnCancel.BackColor = this.BackgroundColor;
            this.btnCancel.ForeColor = this.FontColor;
            this.btnCancel.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.grbPlayer.ForeColor = this.FontColor;
            this.grbVirtualDjImport.ForeColor = this.FontColor;
            this.chbShortTrackColouring.ForeColor = this.FontColor;

            this.tabGeneral.BackColor = this.BackgroundColor;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearDatabaseEvent?.Invoke(this, EventArgs.Empty);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.SetShortTrackColouringThreshold();
            this.SetPreviewPercentage();
            this.CloseViewWithOkEvent?.Invoke(this, EventArgs.Empty);

        }

        private void SetPreviewPercentage()
        {
            this.SetPreviewPercentageEvent?.Invoke(this, new Messenger { DecimalField1 = this.nmdPreviewPercentage.Value });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseViewWithCancelEvent?.Invoke(this, EventArgs.Empty);
        }

        private void chbAutomaticBpmImport_CheckedChanged(object sender, EventArgs e)
        {
            this.SetAutomaticBpmImportEvent?.Invoke(this,new Messenger { BooleanField1 = this.chbAutomaticBpmImport.Checked });
        }

        private void chbAutomaticKeyImport_CheckedChanged(object sender, EventArgs e)
        {
            this.SetAutomaticKeyImportEvent?.Invoke(this, new Messenger { BooleanField1 = this.chbAutomaticKeyImport.Checked });
        }
        public void SetImportSettings(
            bool automaticBpmImport, 
            bool automaticKeyImport, 
            String virtualDjDatabasePath, 
            bool playTrackAfterOpenFiles, 
            bool hasVirtualDj, 
            int previewPercentage,
            bool isShortTrackColouringEnabled,
            decimal shortTrackColouringThreshold)
        {
            this.chbAutomaticBpmImport.Checked = automaticBpmImport;
            this.chbAutomaticKeyImport.Checked = automaticKeyImport;
            this.txtBoxVirtualDjDatabasePath.Text = virtualDjDatabasePath;
            this.chbPlayTrackAfterOpenFiles.Checked = playTrackAfterOpenFiles;
            this.nmdPreviewPercentage.Value = previewPercentage;
            this.chbShortTrackColouring.Checked = isShortTrackColouringEnabled;
            this.txtbShortTrackColouringThreshold.Text = shortTrackColouringThreshold.ToString("N2");
            if (!isShortTrackColouringEnabled)
            {
                this.txtbShortTrackColouringThreshold.Enabled = false;
            }

            if (!hasVirtualDj)
            {
                this.chbAutomaticBpmImport.Checked = false;
                this.chbAutomaticKeyImport.Checked = false;
                this.chbAutomaticBpmImport.Enabled = false;
                this.chbAutomaticKeyImport.Enabled = false;
            }
        }

        private void txtBoxVirtualDjDatabasePath_TextChanged(object sender, EventArgs e)
        {
            this.SetVirtualDjDatabasePathEvent?.Invoke(this, new Messenger { StringField1 = this.txtBoxVirtualDjDatabasePath.Text });
        }

        private void chbPlayTrackAfterOpenFiles_CheckedChanged(object sender, EventArgs e)
        {
            this.SetPlayTrackAfterOpenFilesEvent?.Invoke(this, new Messenger { BooleanField1 = this.chbPlayTrackAfterOpenFiles.Checked });
        }

        private void nmdPreviewPercentage_ValueChanged(object sender, EventArgs e)
        {
            this.SetPreviewPercentageEvent?.Invoke(this, new Messenger { DecimalField1 = this.nmdPreviewPercentage.Value });
        }

        private void chbShortTrackColouring_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chbShortTrackColouring.Checked)
            {
                this.txtbShortTrackColouringThreshold.Enabled = false;
            }
            else
            {
                this.txtbShortTrackColouringThreshold.Enabled = true;
            }
            this.SetShortTrackColouringEvent?.Invoke(this, new Messenger { BooleanField1 = this.chbShortTrackColouring.Checked });
        }

        private void txtbShortTrackColouringThreshold_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                this.SetShortTrackColouringThreshold();
            }
        }
        private void SetShortTrackColouringThreshold()
        {
            String thresholdString = this.txtbShortTrackColouringThreshold.Text;
            if (String.IsNullOrEmpty(thresholdString))
            {
                MessageBox.Show("Track colouring threshold must be set!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    decimal threshold = 0;
                    Decimal.TryParse(thresholdString, out threshold);

                    this.SetShortTrackColouringThresholdEvent?.Invoke(this, new Messenger { DecimalField1 = threshold });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
