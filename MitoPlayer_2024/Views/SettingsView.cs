using MitoPlayer_2024.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class SettingsView : Form, ISettingsView
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
        public event EventHandler<Messenger> SetImportBpmFromVirtualDjEvent;
        public event EventHandler<Messenger> SetImportKeyFromVirtualDjEvent;

        public event EventHandler<Messenger> SetLogMessageDisplayTimeEvent;
        public event EventHandler<Messenger> SetLogMessageEnabledEvent;



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
            this.grbTracklist.ForeColor = this.FontColor;
            this.grbLog.ForeColor = this.FontColor;

        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearDatabaseEvent?.Invoke(this, EventArgs.Empty);

        }



        private void SetPreviewPercentage()
        {
            this.SetPreviewPercentageEvent?.Invoke(this, new Messenger { DecimalField1 = this.nmdPreviewPercentage.Value });
        }
        private void SetLogMessageDisplayTime()
        {
            this.SetLogMessageDisplayTimeEvent?.Invoke(this, new Messenger { DecimalField1 = this.nmdLogMessageDisplayTime.Value });
        }
        private void chbAutomaticBpmImport_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chbAutomaticBpmImport.Checked)
            {
                this.rdbImportBpmFromVirtualDj.Enabled = true;
                this.rdbImportBpmFromVirtualDj.Checked = true;

            }
            else
            {
                this.rdbImportBpmFromVirtualDj.Enabled = false;
            }
            this.SetAutomaticBpmImportEvent?.Invoke(this,new Messenger { BooleanField1 = this.chbAutomaticBpmImport.Checked });
        }
        private void chbAutomaticKeyImport_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chbAutomaticKeyImport.Checked)
            {
                this.rdbImportKeyFromVirtualDj.Enabled = true;
                this.rdbImportKeyFromMixedInKey.Enabled = true;
            }
            else
            {
                this.rdbImportKeyFromVirtualDj.Enabled = false;
                this.rdbImportKeyFromMixedInKey.Enabled = false;
            }
            this.SetAutomaticKeyImportEvent?.Invoke(this, new Messenger { BooleanField1 = this.chbAutomaticKeyImport.Checked });
        }
        public void InitializeSettings(Messenger msg)
        {
            this.chbAutomaticBpmImport.Checked = msg.BooleanField1;
            this.chbAutomaticKeyImport.Checked = msg.BooleanField2;

            if (msg.BooleanField3)
            {
                this.rdbImportBpmFromVirtualDj.Checked = true;
            }
            else
            {
                this.rdbImportBpmFromVirtualDj.Checked = false;
            }
            if (msg.BooleanField4)
            {
                this.rdbImportKeyFromVirtualDj.Checked = true;
                this.rdbImportKeyFromMixedInKey.Checked = false;
            }
            else
            {
                this.rdbImportKeyFromVirtualDj.Checked = false;
                this.rdbImportKeyFromMixedInKey.Checked = true;
            }
            if (this.chbAutomaticBpmImport.Checked)
            {
                this.rdbImportBpmFromVirtualDj.Enabled = true;
                this.rdbImportBpmFromVirtualDj.Checked = true;
            }
            else
            {
                this.rdbImportBpmFromVirtualDj.Enabled = false;
            }
            if (this.chbAutomaticKeyImport.Checked)
            {
                this.rdbImportKeyFromVirtualDj.Enabled = true;
                this.rdbImportKeyFromMixedInKey.Enabled = true;
            }
            else
            {
                this.rdbImportKeyFromVirtualDj.Enabled = false;
                this.rdbImportKeyFromMixedInKey.Enabled = false;
            }
            if (msg.BooleanField5)
            {
                this.chbPlayTrackAfterOpenFiles.Checked = true;
            }
            else
            {
                this.chbPlayTrackAfterOpenFiles.Checked = false;
            }
            if (msg.BooleanField6)
            {
                this.chbShortTrackColouring.Checked = true;
            }
            else
            {
                this.chbShortTrackColouring.Checked = false;
            }
            if (msg.BooleanField7)
            {
                this.chbLogMessageEnabled.Checked = true;
            }
            else
            {
                this.chbLogMessageEnabled.Checked = false;
            }
            if(msg.IntegerField1 > 0)
            {
                this.nmdPreviewPercentage.Value = msg.IntegerField1;
            }
            if (msg.DecimalField1 > 0)
            {
                this.txtbShortTrackColouringThreshold.Text = msg.DecimalField1.ToString("N2");
            }
            else
            {
                this.txtbShortTrackColouringThreshold.Text = String.Empty;
            }
            if (msg.DecimalField2 > 0)
            {
                this.nmdLogMessageDisplayTime.Value = msg.DecimalField2;
            }
            else
            {
                this.nmdLogMessageDisplayTime.Value = 1;
            }
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
            if (chbShortTrackColouring.Checked)
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseWithOk();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseWithCancel();
        }
        private void CloseWithOk()
        {
            this.SetShortTrackColouringThreshold();
            this.SetPreviewPercentage();
            this.CloseViewWithOkEvent?.Invoke(this, EventArgs.Empty);
        }
        private void CloseWithCancel()
        {
            this.CloseViewWithCancelEvent?.Invoke(this, EventArgs.Empty);
        }

        private void rdbImportBpmFromVirtualDj_CheckedChanged(object sender, EventArgs e)
        {
            this.SetImportBpmFromVirtualDjEvent?.Invoke(this, new Messenger { BooleanField1 = this.rdbImportBpmFromVirtualDj.Checked });
        }

        private void rdbImportBpmFromMixedInKey_CheckedChanged(object sender, EventArgs e)
        {
            this.SetImportBpmFromVirtualDjEvent?.Invoke(this, new Messenger { BooleanField1 = this.rdbImportBpmFromVirtualDj.Checked });
        }

        private void rdbImportKeyFromVirtualDj_CheckedChanged(object sender, EventArgs e)
        {
            this.SetImportKeyFromVirtualDjEvent?.Invoke(this, new Messenger { BooleanField1 = this.rdbImportKeyFromVirtualDj.Checked });
        }

        private void rdbImportKeyFromMixedInKey_CheckedChanged(object sender, EventArgs e)
        {
            this.SetImportKeyFromVirtualDjEvent?.Invoke(this, new Messenger { BooleanField1 = this.rdbImportKeyFromVirtualDj.Checked });
        }
        



        private void nmdLogMessageDisplayTime_ValueChanged(object sender, EventArgs e)
        {
            this.SetLogMessageDisplayTimeEvent?.Invoke(this, new Messenger { DecimalField1 = this.nmdLogMessageDisplayTime.Value });
        }
        private void chbLogMessageEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chbLogMessageEnabled.Checked)
            {
                this.nmdLogMessageDisplayTime.Enabled = false;
            }
            else
            {
                this.nmdLogMessageDisplayTime.Enabled = true;
            }
            this.SetLogMessageEnabledEvent?.Invoke(this, new Messenger { BooleanField1 = this.chbLogMessageEnabled.Checked });
        }
    }
}
