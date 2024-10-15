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
    public partial class PreferencesView : Form, IPreferencesView
    {
        public event EventHandler CloseViewWithOkEvent;
        public event EventHandler CloseViewWithCancelEvent;
        public event EventHandler ClearDatabaseEvent;
        public event EventHandler<ListEventArgs> SetAutomaticBpmImportEvent;
        public event EventHandler<ListEventArgs> SetAutomaticKeyImportEvent;
        public event EventHandler<ListEventArgs> SetVirtualDjDatabasePathEvent;
        public event EventHandler<ListEventArgs> SetPlayTrackAfterOpenFilesEvent;



        public PreferencesView()
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

            this.tabGeneral.BackColor = this.BackgroundColor;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearDatabaseEvent?.Invoke(this, EventArgs.Empty);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseViewWithOkEvent?.Invoke(this, EventArgs.Empty);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseViewWithCancelEvent?.Invoke(this, EventArgs.Empty);
        }

        private void chbAutomaticBpmImport_CheckedChanged(object sender, EventArgs e)
        {
            this.SetAutomaticBpmImportEvent?.Invoke(this,new ListEventArgs { BooleanField1 = this.chbAutomaticBpmImport.Checked });
        }

        private void chbAutomaticKeyImport_CheckedChanged(object sender, EventArgs e)
        {
            this.SetAutomaticKeyImportEvent?.Invoke(this, new ListEventArgs { BooleanField1 = this.chbAutomaticKeyImport.Checked });
        }
        public void SetImportSettings(bool automaticBpmImport, bool automaticKeyImport, String virtualDjDatabasePath, bool playTrackAfterOpenFiles, bool hasVirtualDj)
        {
            this.chbAutomaticBpmImport.Checked = automaticBpmImport;
            this.chbAutomaticKeyImport.Checked = automaticKeyImport;
            this.txtBoxVirtualDjDatabasePath.Text = virtualDjDatabasePath;
            this.chbPlayTrackAfterOpenFiles.Checked = playTrackAfterOpenFiles;
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
            this.SetVirtualDjDatabasePathEvent?.Invoke(this, new ListEventArgs { StringField1 = this.txtBoxVirtualDjDatabasePath.Text });
        }

        private void chbPlayTrackAfterOpenFiles_CheckedChanged(object sender, EventArgs e)
        {
            this.SetPlayTrackAfterOpenFilesEvent?.Invoke(this, new ListEventArgs { BooleanField1 = this.chbPlayTrackAfterOpenFiles.Checked });
        }
    }
}
