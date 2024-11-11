using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MitoPlayer_2024.Presenters.ModelTrainerPresenter;

namespace MitoPlayer_2024.Views
{
    public partial class ModelTrainerView : Form, IModelTrainerView
    {
        public event EventHandler CloseViewWithOk;
        public event EventHandler<ListEventArgs> SelectTag;
        public event EventHandler<ListEventArgs> SelectPlaylist;
        public event EventHandler<ListEventArgs> SelectTemplate;
        public event EventHandler<ListEventArgs> IsChromaFeaturesEnabled;
        public event EventHandler<ListEventArgs> IsMFCCsEnabled;
        public event EventHandler<ListEventArgs> IsSpectralContrastEnabled;
        public event EventHandler<ListEventArgs> IsHPCPEnabled;
        public event EventHandler<ListEventArgs> IsHPSEnabled;
        public event EventHandler<ListEventArgs> IsSpectralCentroidEnabled;
        public event EventHandler<ListEventArgs> IsTonnetzFeaturesEnabled;
        public event EventHandler<ListEventArgs> IsSpectralBandwidthEnabled;
        public event EventHandler<ListEventArgs> IsZCREnabled;
        public event EventHandler<ListEventArgs> IsRMSEnabled;
        public event EventHandler<ListEventArgs> IsPitchEnabled;
        public event EventHandler<ListEventArgs> BatchProcessChanged;
        public event EventHandler GenerateTrainingData;
        public event EventHandler CancelGenerationEvent;
        public event EventHandler<ListEventArgs> LoadTrainingDataEvent;
        public event EventHandler<ListEventArgs> DeleteTrainingDataEvent;
        public event EventHandler<ListEventArgs> SetIsTracklistDetailsDisplayedEvent;
        public event EventHandler TrainModelEvent;
        public event EventHandler AddTrainingDataEvent;
        public event EventHandler CalculateDataSetQualityEvent;

        private BindingSource inputTrackListBindingSource { get; set; }
        private BindingSource trainingDataListBindingSource { get; set; }
        public ModelTrainerView()
        {
            InitializeComponent();
            this.SetControlColors();
            this.CenterToScreen();
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

        }

        

    Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonColor = System.Drawing.ColorTranslator.FromHtml("#292a2d");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");
        Color GridLineColor1 = System.Drawing.ColorTranslator.FromHtml("#131315");
        Color GridLineColor2 = System.Drawing.ColorTranslator.FromHtml("#212224");
        Color GridPlayingColor = System.Drawing.ColorTranslator.FromHtml("#4d4d4d");
        Color GridSelectionColor = System.Drawing.ColorTranslator.FromHtml("#626262");
        Color ActiveButtonColor = System.Drawing.ColorTranslator.FromHtml("#FFBF80");


        //Playlist and the values of the Tag are formed together the example set to create the training dataset. The application can extract features from the tracks and these features will be labeled with the tag values. The training dataset describes this mapping. 
        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.btnOk.BackColor = this.BackgroundColor;
            this.btnOk.ForeColor = this.FontColor;
            this.btnOk.FlatAppearance.BorderColor = this.ButtonBorderColor;

          /* this.btnBrowse.BackColor = this.BackgroundColor;
            this.btnBrowse.ForeColor = this.FontColor;
            this.btnBrowse.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnCancel.BackColor = this.BackgroundColor;
            this.btnCancel.ForeColor = this.FontColor;
            this.btnCancel.FlatAppearance.BorderColor = this.ButtonBorderColor;*/

            this.dgvInputTrackList.BackgroundColor = this.ButtonColor;
            this.dgvInputTrackList.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvInputTrackList.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvInputTrackList.EnableHeadersVisualStyles = false;
            this.dgvInputTrackList.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvInputTrackList.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;

            this.dgvTrainingDataList.BackgroundColor = this.ButtonColor;
            this.dgvTrainingDataList.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvTrainingDataList.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvTrainingDataList.EnableHeadersVisualStyles = false;
            this.dgvTrainingDataList.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvTrainingDataList.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;

            this.gpbFeatures.ForeColor = this.FontColor;
            this.grbResult.ForeColor = this.FontColor;

            this.lblGeneratingIsInProgress.ForeColor = this.ActiveButtonColor;

            this.prbProcessedTracks.BackColor = this.ButtonColor;
            this.prbProcessedTracks.ForeColor = this.ActiveButtonColor;

            this.btnCancelGeneration.BackColor = this.ButtonColor;
            this.btnCancelGeneration.ForeColor = this.FontColor;
            this.btnCancelGeneration.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnGenerate.BackColor = this.ButtonColor;
            this.btnGenerate.ForeColor = this.FontColor;
            this.btnGenerate.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnOk.BackColor = this.ButtonColor;
            this.btnOk.ForeColor = this.FontColor;
            this.btnOk.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnResult.BackColor = this.ButtonColor;
            this.btnResult.ForeColor = this.FontColor;
            this.btnResult.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnDeleteTrainingData.BackColor = this.ButtonColor;
            this.btnDeleteTrainingData.ForeColor = this.FontColor;
            this.btnDeleteTrainingData.FlatAppearance.BorderColor = this.ButtonBorderColor;

            //this.pbrTrackProgress.BackColor = this.FontColor;
            // this.pbrTrackProgress.ForeColor = this.BackgroundColor;

            this.btnResult.Enabled = false;
            this.btnCancelGeneration.Hide();
            this.lblGeneratingIsInProgress.Hide();
        }

        public void InitializeView(List<Tag> tagList, List<Playlist> plalyistList, List<TrainingData> templateList, int batchSize, bool isTracklistDetailsDisplayed)
        {
            this.cmbTags.Items.Clear();
            this.cmbPlaylists.Items.Clear();
            this.cmbTemplates.Items.Clear();

            if(batchSize > 0)
            {
                this.nmpBatchedProcess.Value = batchSize;
            }
            else
            {
                this.nmpBatchedProcess.Value = 1;
            }

            foreach (Tag tag in tagList)
            {
                this.cmbTags.Items.Add(tag.Name);
            }
            foreach (Playlist playlist in plalyistList)
            {
                this.cmbPlaylists.Items.Add(playlist.Name);
            }
            foreach (TrainingData template in templateList)
            {
                this.cmbTemplates.Items.Add(template.Name);
            }

            this.chbIsTracklistDetailsDisplayed.Checked = isTracklistDetailsDisplayed;
        }
        public void InitializeFeatureSettings(
            bool extractChromaFeatures,
            bool extractHarmonicPercussiveSeparation,
            bool extractMFCCs, 
            bool extractHPCP, 
            bool extractSpectralContrast, 
            bool extractSpectralCentroid, 
            bool extractSpectralBandwidth,
            bool extractTonnetzFeatures,
            bool extractZeroCrossingRate, 
            bool extractPitch, 
            bool extractRmsEnergy)
        {
            this.chbChroma.Checked = extractChromaFeatures;
            this.chbHps.Checked = extractHarmonicPercussiveSeparation;
            this.chbMfccs.Checked = extractMFCCs;
            this.chbHpcp.Checked = extractHPCP;
            this.chbSpectralContrast.Checked = extractSpectralContrast;
            this.chbSpectralCentroid.Checked = extractSpectralCentroid;
            this.chbSpectralBandwidth.Checked = extractSpectralBandwidth;
            this.chbTonnetz.Checked = extractTonnetzFeatures;
            this.chbZcr.Checked = extractZeroCrossingRate;
            this.chbPitch.Checked = extractPitch;
            this.chbRms.Checked = extractRmsEnergy;

        }
        public void SetInputTrackListBindingSource(BindingSource trackList)
        {
            this.inputTrackListBindingSource = new BindingSource();
            this.inputTrackListBindingSource.DataSource = trackList;
            this.dgvInputTrackList.DataSource = this.inputTrackListBindingSource.DataSource;
            this.btnResult.Enabled = true;
            this.SetInputTrackListColors();
        }
        private void dgvInputTrackList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.SetInputTrackListColors();
        }
        private List<Tag> tagList { get; set; }
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        public void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<String, Dictionary<String, Color>> tagValueDictionary)
        {
            this.tagList = tagList;
            this.tagValueDictionary = tagValueDictionary;
        }
        public void SetInputTrackListColors()
        {
            bool isTagValueColoringEnabled = this.tagList != null && this.tagList.Count > 0 && this.tagValueDictionary != null && this.tagValueDictionary.Count > 0;

            if (this.dgvInputTrackList != null && this.dgvInputTrackList.Rows != null && this.dgvInputTrackList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvInputTrackList.Rows.Count; i++)
                {
                    if (i == 0 || i % 2 == 0)
                    {
                        this.dgvInputTrackList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor1;
                    }
                    else
                    {
                        this.dgvInputTrackList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor2;
                    }

                    if (isTagValueColoringEnabled)
                    {
                        foreach (var tag in this.tagList)
                        {
                            string tagName = tag.Name;
                            if (this.dgvInputTrackList.Columns.Contains(tag.Name))
                            {
                                string tagValueName = this.dgvInputTrackList.Rows[i].Cells[tagName].Value?.ToString() ?? string.Empty;

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
                                            this.dgvInputTrackList.Rows[i].Cells[tagName].Style.ForeColor = this.tagValueDictionary[tagName][tagValueName];
                                            this.dgvInputTrackList.Rows[i].Cells[tagName].Style.BackColor = this.dgvInputTrackList.Rows[i].Index % 2 == 0 ? this.GridLineColor1 : this.GridLineColor2;
                                        }
                                        else
                                        {
                                            Color color = this.tagValueDictionary[tagName][tagValueName];
                                            this.dgvInputTrackList.Rows[i].Cells[tagName].Style.BackColor = color;
                                            this.dgvInputTrackList.Rows[i].Cells[tagName].Style.ForeColor = (color.R < 100 && color.G < 100) ||
                                            (color.R < 100 && color.B < 100) ||
                                            (color.B < 100 && color.G < 100)
                                            ? Color.White : Color.Black;
                                        }
                                    }

                                }
                                else
                                {
                                    this.dgvInputTrackList.Rows[i].Cells[tagName].Style.BackColor = this.dgvInputTrackList.Rows[i].Index % 2 == 0 ? this.GridLineColor1 : this.GridLineColor2;
                                }
                            }
                           

                            
                        }
                    }
                }
            }
        }
        public void SetTraningDataListBindingSource(BindingSource trainingDataList)
        {
            this.trainingDataListBindingSource = new BindingSource();
            this.trainingDataListBindingSource.DataSource = trainingDataList;
            this.dgvTrainingDataList.DataSource = this.trainingDataListBindingSource.DataSource;
            this.dgvTrainingDataList.Columns["Id"].Visible = false;
            this.SetTrainingDataListColors();
        }
        private void dgvTrainingDataList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.SetTrainingDataListColors();
        }
        public void SetTrainingDataListColors()
        {

            if (this.dgvTrainingDataList != null && this.dgvTrainingDataList.Rows != null && this.dgvTrainingDataList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvTrainingDataList.Rows.Count; i++)
                {
                    if (i == 0 || i % 2 == 0)
                    {
                        this.dgvTrainingDataList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor1;
                    }
                    else
                    {
                        this.dgvTrainingDataList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor2;
                    }
                }
            }
        }
        private void cmbTags_SelectedIndexChanged(object sender, EventArgs e)
        {

                this.SelectTag?.Invoke(this, new ListEventArgs() { StringField1 = this.cmbTags.SelectedItem.ToString() });
        }
        private void cmbPlaylists_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectPlaylist?.Invoke(this, new ListEventArgs() { StringField1 = this.cmbPlaylists.SelectedItem.ToString() });
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseViewWithOk?.Invoke(this, new EventArgs());
        }

        private void chbChroma_CheckedChanged(object sender, EventArgs e)
        {
            this.IsChromaFeaturesEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbChroma.Checked });
        }

        private void chbMfccs_CheckedChanged(object sender, EventArgs e)
        {
            this.IsMFCCsEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbMfccs.Checked });
        }

        private void chbSpectralContrast_CheckedChanged(object sender, EventArgs e)
        {
            this.IsSpectralContrastEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbSpectralContrast.Checked });
        }

        private void chbHpcp_CheckedChanged(object sender, EventArgs e)
        {
            this.IsHPCPEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbHpcp.Checked });
        }

        private void chbHps_CheckedChanged(object sender, EventArgs e)
        {
            this.IsHPSEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbHps.Checked });
        }

        private void chbSpectralCentroid_CheckedChanged(object sender, EventArgs e)
        {
            this.IsSpectralCentroidEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbSpectralCentroid.Checked });
        }

        private void chbTonnetz_CheckedChanged(object sender, EventArgs e)
        {
            this.IsTonnetzFeaturesEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbTonnetz.Checked });
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.isGenerationInProgress = true;
            this.GenerateTrainingData?.Invoke(this, new EventArgs());
        }

        private void cmbTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectTemplate?.Invoke(this, new ListEventArgs() { StringField1 = this.cmbTemplates.SelectedItem.ToString() });
        }

        private void chbSpectralBandwidth_CheckedChanged(object sender, EventArgs e)
        {
            this.IsSpectralBandwidthEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbSpectralBandwidth.Checked });
        }

        private void chbZcr_CheckedChanged(object sender, EventArgs e)
        {
            this.IsZCREnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbZcr.Checked });
        }

        private void chbRms_CheckedChanged(object sender, EventArgs e)
        {
            this.IsRMSEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbZcr.Checked });
        }

        private void chbPitch_CheckedChanged(object sender, EventArgs e)
        {
            this.IsPitchEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbZcr.Checked });
        }

        private void nmpBatchedProcess_ValueChanged(object sender, EventArgs e)
        {
            this.BatchProcessChanged?.Invoke(this, new ListEventArgs() { IntegerField1 = (int)this.nmpBatchedProcess.Value });
        }

        private bool isGenerating { get; set; }
        private bool isTraining { get; set; }
        public void ChangeGeneratingStatus(bool isGenerating)
        {
            this.isGenerating = isGenerating;

            if (this.isGenerating)
            {
                this.btnCancelGeneration.Show();
                this.btnGenerate.Enabled = false;
                this.btnOk.Enabled = false;

                this.lblGeneratingIsInProgress.Show();

                this.cmbTags.Enabled = false;
                this.cmbPlaylists.Enabled = false;
                this.cmbTemplates.Enabled = false;
                this.chbChroma.Enabled = false;
                this.chbHpcp.Enabled = false;
                this.chbHps.Enabled = false;
                this.chbMfccs.Enabled = false;
                this.chbPitch.Enabled = false;
                this.chbRms.Enabled = false;
                this.chbSpectralBandwidth.Enabled = false;
                this.chbSpectralCentroid.Enabled = false;
                this.chbSpectralContrast.Enabled = false;
                this.chbTonnetz.Enabled = false;
                this.chbZcr.Enabled = false;
                this.nmpBatchedProcess.Enabled = false;

            }
            else
            {
                this.btnCancelGeneration.Hide();
                this.btnGenerate.Enabled = true;
                this.btnOk.Enabled = true;

                this.lblGeneratingIsInProgress.Hide();

                this.cmbTags.Enabled = true;
                this.cmbPlaylists.Enabled = true;
                this.cmbTemplates.Enabled = true;
                this.chbChroma.Enabled = true;
                this.chbHpcp.Enabled = true;
                this.chbHps.Enabled = true;
                this.chbMfccs.Enabled = true;
                this.chbPitch.Enabled = true;
                this.chbRms.Enabled = true;
                this.chbSpectralBandwidth.Enabled = true;
                this.chbSpectralCentroid.Enabled = true;
                this.chbSpectralContrast.Enabled = true;
                this.chbTonnetz.Enabled = true;
                this.chbZcr.Enabled = true;
                this.nmpBatchedProcess.Enabled = true;
            }
        }
        public void ChangeTrainingStatus(bool isTraining)
        {
            this.isTraining = isTraining;

            if (this.isTraining)
            {
                this.btnCancelGeneration.Show();
                this.btnGenerate.Enabled = false;
                this.btnOk.Enabled = false;

                this.lblGeneratingIsInProgress.Show();

                this.cmbTags.Enabled = false;
                this.cmbPlaylists.Enabled = false;
                this.cmbTemplates.Enabled = false;
                this.chbChroma.Enabled = false;
                this.chbHpcp.Enabled = false;
                this.chbHps.Enabled = false;
                this.chbMfccs.Enabled = false;
                this.chbPitch.Enabled = false;
                this.chbRms.Enabled = false;
                this.chbSpectralBandwidth.Enabled = false;
                this.chbSpectralCentroid.Enabled = false;
                this.chbSpectralContrast.Enabled = false;
                this.chbTonnetz.Enabled = false;
                this.chbZcr.Enabled = false;
                this.nmpBatchedProcess.Enabled = false;

            }
            else
            {
                this.btnCancelGeneration.Hide();
                this.btnGenerate.Enabled = true;
                this.btnOk.Enabled = true;

                this.lblGeneratingIsInProgress.Hide();

                this.cmbTags.Enabled = true;
                this.cmbPlaylists.Enabled = true;
                this.cmbTemplates.Enabled = true;
                this.chbChroma.Enabled = true;
                this.chbHpcp.Enabled = true;
                this.chbHps.Enabled = true;
                this.chbMfccs.Enabled = true;
                this.chbPitch.Enabled = true;
                this.chbRms.Enabled = true;
                this.chbSpectralBandwidth.Enabled = true;
                this.chbSpectralCentroid.Enabled = true;
                this.chbSpectralContrast.Enabled = true;
                this.chbTonnetz.Enabled = true;
                this.chbZcr.Enabled = true;
                this.nmpBatchedProcess.Enabled = true;

                trainingLogDic = new Dictionary<String, String>();
            }
        }
        public void UpdateProgressOnView(MessageTest e)
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

        private bool isGenerationInProgress { get; set; }
        Dictionary<String, String> trainingLogDic = new Dictionary<String, String>();
        public void UpdateProgress(MessageTest e)
        {
            if (isGenerationInProgress)
            {
                if (e.LogState == LogState.ParallelProcess)
                {
                    if (e.RemainingTime != null && e.RemainingTime.TotalMilliseconds > 0)
                    {
                        // Format remaining time
                        string formattedRemainingTime = string.Format("{0:D2}h {1:D2}m {2:D2}s",
                        e.RemainingTime.Hours, e.RemainingTime.Minutes, e.RemainingTime.Seconds);
                        this.lblRemainingTime.Text = formattedRemainingTime;
                    }

                    if (e.Total > 0)
                    {
                        this.lblProcessedTracks.Text = e.Sum.ToString() + "/" + e.Total.ToString();
                        this.prbProcessedTracks.Value = (int)((decimal)e.Sum / (decimal)e.Total * 100);
                    }
                }
                else if (e.LogState == LogState.FileReading)
                {
                    if (e.EstimatedSize > 0)
                    {
                        this.lblEstimatedSize.Text = e.EstimatedSize.ToString("N0") + " MB";
                    }
                }
                else if (e.LogState == LogState.Extraction)
                {
                    String log = $"File: {e.FilePath}\n" +
                        $"- {e.ExtractionProgressValue}% - complete (Sample {e.CurrentSample} of {e.TotalSamples})\n";

                    if (!trainingLogDic.ContainsKey(e.FilePath))
                    {
                        trainingLogDic.Add(e.FilePath, log);
                    }
                    else
                    {
                        if (e.ExtractionProgressValue == 100)
                        {
                            trainingLogDic.Remove(e.FilePath);
                        }
                        else
                        {
                            trainingLogDic[e.FilePath] = log;
                        }
                    }

                    if (trainingLogDic.Count > 0)
                    {
                        this.lblLog.Text = String.Empty;
                        foreach (KeyValuePair<string, string> kvp in trainingLogDic)
                        {
                            this.lblLog.Text += kvp.Value;
                        }
                    }
                }
                else if (e.LogState == LogState.Finish)
                {
                    this.lblLog.Text = e.Log;
                }
                else if (e.LogState == LogState.Canceled)
                {
                    this.lblProcessedTracks.Text = "0/0";
                    this.lblRemainingTime.Text = "-";
                    this.lblEstimatedSize.Text = "-";
                    this.prbProcessedTracks.Value = 0;
                    this.lblLog.Text = e.Log;
                    this.trainingLogDic = new Dictionary<String, String>();
                    this.isGenerationInProgress = false;
                }
            }
        }

        private void btnCancelGeneration_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel the generation?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.CancelGenerationEvent?.Invoke(this, new EventArgs());
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isGenerationInProgress)
            {
                // Handle the event here
                DialogResult result = MessageBox.Show("Generation is in progress. Are you sure you want to cancel the generation?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.CancelGenerationEvent?.Invoke(this, new EventArgs());
                }
                else
                {
                    e.Cancel = true; // Cancel the close event
                }
            }

        }


        private void btnDeleteTrainingData_Click(object sender, EventArgs e)
        {
            if (dgvTrainingDataList.SelectedRows.Count > 0)
            {
                this.DeleteTrainingDataEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = (int)dgvTrainingDataList.SelectedRows[0].Cells["Id"].Value });
            }
        }

        private void dgvTrainingDataList_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void btnLoadTrainingData_Click(object sender, EventArgs e)
        {
            if (dgvTrainingDataList.SelectedRows.Count > 0)
            {
                this.LoadTrainingDataEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = (int)dgvTrainingDataList.SelectedRows[0].Cells["Id"].Value });
            }
        }

        private void dgvTrainingDataList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = this.dgvTrainingDataList.Rows[e.RowIndex];
                this.LoadTrainingDataEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = (int)selectedRow.Cells["Id"].Value });
            }
        }

        private void btnTrainModel_Click(object sender, EventArgs e)
        {
            this.TrainModelEvent?.Invoke(this, new EventArgs());
        }

        private void btnAddTrainingData_Click(object sender, EventArgs e)
        {
            this.AddTrainingDataEvent?.Invoke(this, new EventArgs());
        }

        private void chbIsTracklistDetailsDisplayed_CheckedChanged(object sender, EventArgs e)
        {
            this.SetIsTracklistDetailsDisplayedEvent?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbIsTracklistDetailsDisplayed.Checked });
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            this.CalculateDataSetQualityEvent?.Invoke(this, new EventArgs());
        }
    }
}
