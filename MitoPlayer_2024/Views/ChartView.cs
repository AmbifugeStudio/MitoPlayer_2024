using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static MitoPlayer_2024.Presenters.ModelTrainerPresenter;

namespace MitoPlayer_2024.Views
{
    public partial class ChartView : Form, IChartView
    {
        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonColor = System.Drawing.ColorTranslator.FromHtml("#292a2d");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");
        Color GridLineColor1 = System.Drawing.ColorTranslator.FromHtml("#131315");
        Color GridLineColor2 = System.Drawing.ColorTranslator.FromHtml("#212224");
        Color GridPlayingColor = System.Drawing.ColorTranslator.FromHtml("#4d4d4d");
        Color GridSelectionColor = System.Drawing.ColorTranslator.FromHtml("#626262");
        Color ActiveButtonColor = System.Drawing.ColorTranslator.FromHtml("#FFBF80");

        private List<Tag> tagList { get; set; }
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        private BindingSource tracklistBindingSource { get; set; }
        private bool isAnalysationInProgress { get; set; }

        public event EventHandler<EventArgs> AnalyseTrackEvent;
        public event EventHandler<EventArgs> CancelAnalysationEvent;
        public event EventHandler<Messenger> SetCurrentFeatureTypeEvent;

        public ChartView()
        {
            this.InitializeComponent();
            this.SetControlColors();
            this.CenterToScreen();

            this.tracklistBindingSource = new BindingSource();

            this.lblInProgress.Hide();
            this.btnCancel.Enabled = false;

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }

        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.dgvTracklist.BackgroundColor = this.ButtonColor;
            this.dgvTracklist.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvTracklist.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvTracklist.EnableHeadersVisualStyles = false;
            this.dgvTracklist.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvTracklist.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;

            this.grbFeatures.ForeColor = this.FontColor;

            this.btnAnalyse.BackColor = this.BackgroundColor;
            this.btnAnalyse.ForeColor = this.FontColor;
            this.btnAnalyse.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnClose.BackColor = this.BackgroundColor;
            this.btnClose.ForeColor = this.FontColor;
            this.btnClose.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnCancel.BackColor = this.BackgroundColor;
            this.btnCancel.ForeColor = this.FontColor;
            this.btnCancel.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.prbAnalyseProgress.BackColor = this.ButtonColor;
            this.prbAnalyseProgress.ForeColor = this.ActiveButtonColor;

            this.lblInProgress.ForeColor = this.ActiveButtonColor;
        }
        public void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<String, Dictionary<String, Color>> tagValueDictionary)
        {
            this.tagList = tagList;
            this.tagValueDictionary = tagValueDictionary;
        }
        public void InitializeTrackList(DataTableModel model)
        {
            if (model.BindingSource != null)
            {
                this.tracklistBindingSource.DataSource = model.BindingSource;
                this.dgvTracklist.DataSource = this.tracklistBindingSource.DataSource;
            }
            this.tracklistBindingSource.ResetBindings(false);
            this.UpdateTracklistColor();
        }
        public void SetInputTrackListBindingSource(BindingSource trackList)
        {
            this.tracklistBindingSource = new BindingSource();
            this.tracklistBindingSource.DataSource = trackList;
            this.dgvTracklist.DataSource = this.tracklistBindingSource.DataSource;
            this.UpdateTracklistColor();
        }
        private void dgvTrackList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.UpdateTracklistColor();
        }
        public void UpdateTracklistColor()
        {
            this.dgvTracklist.ClearSelection();

            bool isTagValueColoringEnabled = this.tagList != null && this.tagList.Count > 0 && this.tagValueDictionary != null && this.tagValueDictionary.Count > 0;

            if (this.dgvTracklist != null && this.dgvTracklist.Rows != null && this.dgvTracklist.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dgvTracklist.Rows)
                {
                    row.DefaultCellStyle.BackColor = (row.Index % 2 == 0 ? this.GridLineColor1 : this.GridLineColor2);

                    if (isTagValueColoringEnabled)
                    {
                        foreach (var tag in this.tagList)
                        {
                            string tagName = tag.Name;
                            string tagValueName = row.Cells[tagName].Value?.ToString() ?? string.Empty;

                            if (tag.HasMultipleValues)
                            {
                                tagValueName = tagName;
                            }

                            if (!string.IsNullOrEmpty(tagValueName))
                            {
                                if (!tag.HasMultipleValues)
                                {
                                    if (tag.TextColoring)
                                    {
                                        row.Cells[tagName].Style.ForeColor = this.tagValueDictionary[tagName][tagValueName];
                                        row.Cells[tagName].Style.BackColor = (row.Index % 2 == 0 ? this.GridLineColor1 : this.GridLineColor2);
                                    }
                                    else
                                    {
                                        Color color = this.tagValueDictionary[tagName][tagValueName];
                                        row.Cells[tagName].Style.BackColor = color;
                                        row.Cells[tagName].Style.ForeColor = (color.R < 100 && color.G < 100) ||
                                        (color.R < 100 && color.B < 100) ||
                                        (color.B < 100 && color.G < 100)
                                        ? Color.White : Color.Black;
                                    }
                                }
                            }
                            else
                            {
                                row.Cells[tagName].Style.BackColor = (row.Index % 2 == 0 ? this.GridLineColor1 : this.GridLineColor2);
                            }
                        }
                    }
                }
            }
        }
        public void UpdatePlot(PlotView plot)
        {
            this.pnlPlot.Controls.Clear();
            this.pnlPlot.Controls.Add(plot);
        }
        public void UpdateSoundWavePlot(PlotView plot)
        {
            this.pnlFrequency.Controls.Clear();
            this.pnlFrequency.Controls.Add(plot);
        }

        private void rdbChroma_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbMFCCs_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbHPCP_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbHPSS_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbSpectralContrast_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbSpectralCentroid_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbSpectralBandwidth_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbTonnetz_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbZCR_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbRMS_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void rdbPitch_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChanged(((RadioButton)sender));
        }
        private void RadioButtonChanged(RadioButton currentRadioButton)
        {
            if (currentRadioButton.Checked)
            {
                foreach (var control in this.grbFeatures.Controls)
                {
                    if (control is RadioButton && ((RadioButton)control).Text != currentRadioButton.Text)
                    {
                        ((RadioButton)control).Checked = false;
                    }
                }
                this.SetCurrentFeatureTypeEvent?.Invoke(this, new Messenger() { StringField1 = ((RadioButton)currentRadioButton).Text });
            }
        }
        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            this.AnalyseTrackEvent?.Invoke(this, new EventArgs());
        }

        public void StartAnalysation()
        {
            this.btnAnalyse.Enabled = false;
            this.btnCancel.Enabled = true;
            this.lblInProgress.Enabled = true;
            this.prbAnalyseProgress.Value = 0;
            this.isAnalysationInProgress = true;
            this.lblInProgress.Show();
        }
        public void UpdateProgressOnView(Messenger e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => UpdateProgress(e)));
            }
            else
            {
                UpdateProgress(e);
            }
        }
        public void UpdateProgress(Messenger e)
        {
            this.prbAnalyseProgress.Value = e.IntegerField1;
        }
        public void FinishAnalysation()
        {
            this.btnAnalyse.Enabled = true;
            this.btnCancel.Enabled = false;
            this.lblInProgress.Enabled = false;
            this.prbAnalyseProgress.Value = 0;
            this.isAnalysationInProgress = false;
            this.lblInProgress.Hide();
        }
        public void UpdateViewAfterCancel()
        {
            this.btnAnalyse.Enabled = true;
            this.btnCancel.Enabled = false;
            this.lblInProgress.Enabled = false;
            this.prbAnalyseProgress.Value = 0;
            this.isAnalysationInProgress = false;
            this.lblInProgress.Hide();
        }
        private void bnCancel_Click(object sender, EventArgs e)
        {
            if (this.isAnalysationInProgress)
            {
                // Handle the event here
                DialogResult result = MessageBox.Show("Analysation is in progress. Are you sure you want to cancel the analysation?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.CancelAnalysationEvent?.Invoke(this, new EventArgs());
                }
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.isAnalysationInProgress)
            {
                // Handle the event here
                DialogResult result = MessageBox.Show("Analysation is in progress. Are you sure you want to cancel the analysation?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.CancelAnalysationEvent?.Invoke(this, new EventArgs());
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isAnalysationInProgress)
            {
                // Handle the event here
                DialogResult result = MessageBox.Show("Analysation is in progress. Are you sure you want to cancel the generation?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.CancelAnalysationEvent?.Invoke(this, new EventArgs());
                }
                else
                {
                    e.Cancel = true; // Cancel the close event
                }
            }
        }

    }
}
