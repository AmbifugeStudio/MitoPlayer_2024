using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class ExportToDirectoryPresenter
    {
        private IExportToDirectoryView view;
        private ITagDao tagDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private String exportPath { get; set; }
        private bool isRowNumberChecked { get; set; }
        private bool isKeyCodeChecked { get; set; }
        private bool isBpmNumberChecked { get; set; }
        private bool isTrunkBpmChecked { get; set; }
        private bool isTrunkedArtistChecked { get; set; }
        private bool isTrunkedTitleChecked { get; set; }
        private decimal artistMinimumCharacter { get; set; }
        private decimal titleMinimumCharacter { get; set; }
        private List<Model.Track> trackList { get; set; }
        private DataTable trackTable { get; set; }
        private BindingSource trackListBindingSource { get; set; }

        public ExportToDirectoryPresenter(IExportToDirectoryView exportToDirectoryView,List<Model.Track> trackList, ITagDao tagDao, ISettingDao settingDao)
        {
            this.view = exportToDirectoryView;
            this.tagDao = tagDao;
            this.settingDao = settingDao;
            this.trackList = trackList;

            this.view.CloseViewWithOk += CloseViewWithOk;
            this.view.CloseViewWithCancel += CloseViewWithCancel;
            this.view.BrowseEvent += BrowseEvent;
            this.view.SetRowNumberEvent += SetRowNumberEvent;
            this.view.SetKeyCodeEvent += SetKeyCodeEvent;
            this.view.SetBpmNumberEvent += SetBpmNumberEvent;
            this.view.SetTrunkBpmEvent += SetTrunkBpmEvent;
            this.view.SetTrunkedArtistEvent += SetTrunkedArtistEvent;
            this.view.SetTrunkedTitleEvent += SetTrunkedTitleEvent;
            this.view.SetArtistMinimumCharacterEvent += SetArtistMinimumCharacterEvent;
            this.view.SetTitleMinimumCharacterEvent += SetTitleMinimumCharacterEvent;
        }

        public void Initialize()
        {
            this.exportPath = this.settingDao.GetStringSetting(Settings.LastExportDirectoryPath.ToString());
            this.isRowNumberChecked = this.settingDao.GetBooleanSetting(Settings.IsRowNumberChecked.ToString()).Value;
            this.isKeyCodeChecked = this.settingDao.GetBooleanSetting(Settings.IsKeyCodeChecked.ToString()).Value;
            this.isBpmNumberChecked = this.settingDao.GetBooleanSetting(Settings.IsBpmNumberChecked.ToString()).Value;
            this.isTrunkBpmChecked = this.settingDao.GetBooleanSetting(Settings.IsTrunkedBpmChecked.ToString()).Value;
            this.isTrunkedArtistChecked = this.settingDao.GetBooleanSetting(Settings.IsTrunkedArtistChecked.ToString()).Value;
            this.isTrunkedTitleChecked = this.settingDao.GetBooleanSetting(Settings.IsTrunkedTitleChecked.ToString()).Value;
            this.artistMinimumCharacter = this.settingDao.GetDecimalSetting(Settings.ArtistMinimumCharacter.ToString());
            this.titleMinimumCharacter = this.settingDao.GetDecimalSetting(Settings.TitleMinimumCharacter.ToString());

            this.InitializeSettings();
            this.InitializeDataTable();
        }
        private void InitializeSettings()
        {
            this.view.InitializeView(this.exportPath, this.isRowNumberChecked, this.isKeyCodeChecked, this.isBpmNumberChecked, this.isTrunkBpmChecked,
                 this.isTrunkedArtistChecked,this.isTrunkedTitleChecked, this.artistMinimumCharacter, this.titleMinimumCharacter);
        }
        private void InitializeDataTable()
        {
            this.trackListBindingSource = new BindingSource();
            this.trackTable = new DataTable();
            this.trackTable.Columns.Add("Filename", typeof(string));

            String rowNumber = String.Empty;
            String keyCode = String.Empty;
            String bpmNumber = String.Empty;
            String newFileName = String.Empty;
            String extension = String.Empty;
            String hasVocal = String.Empty;

            if (this.trackList != null && this.trackList.Count > 0)
            {
                for (int i = 0; i <= this.trackList.Count - 1; i++)
                {
                    newFileName = this.trackList[i].FileName;
                    rowNumber = String.Empty;
                    keyCode = String.Empty;
                    bpmNumber = String.Empty;
                    hasVocal = String.Empty;

                    if (newFileName.Contains("_"))
                    {
                        String[] parts = newFileName.Split('_');

                        //sorszam_key_bpm_title
                        //key_bpm_title

                        if (parts.Length == 4)
                        {
                            String removable = parts[0] + "_" + parts[1] + "_" + parts[2] + "_";
                            newFileName = newFileName.Replace(removable, "");
                        }
                        else if (parts.Length == 3)
                        {
                            String removable = parts[0] + "_" + parts[1] + "_";
                            newFileName = newFileName.Replace(removable, "");
                        }
                    }

                    if(!String.IsNullOrEmpty(this.trackList[i].Artist) && !String.IsNullOrEmpty(this.trackList[i].Title))
                    {
                        String artist = String.Empty;
                        String title = String.Empty;

                        if (this.isTrunkedArtistChecked)
                        {
                            if(this.trackList[i].Artist.Length > this.artistMinimumCharacter)
                            {
                                artist = this.trackList[i].Artist.Substring(0, Convert.ToInt32(this.artistMinimumCharacter));
                            }
                            else
                            {
                                artist = this.trackList[i].Artist;
                            }
                        }
                        else
                        {
                            artist = this.trackList[i].Artist;
                        }
                        if (this.isTrunkedTitleChecked)
                        {
                            if (this.trackList[i].Title.Length > this.titleMinimumCharacter)
                            {
                                title = this.trackList[i].Title.Substring(0, Convert.ToInt32(this.titleMinimumCharacter));
                            }
                            else
                            {
                                title = this.trackList[i].Title;
                            }
                        }
                        else
                        {
                            title = this.trackList[i].Title;
                        }
                        newFileName = artist + " - " + title;
                    }

                    if (this.isRowNumberChecked)
                    {
                        if(this.trackList.Count < 100)
                        {
                            if(i < 9)
                            {
                                rowNumber = "0" + (i + 1).ToString();
                            }
                            else
                            {
                                rowNumber = (i + 1).ToString();
                            }
                        }
                        else if (this.trackList.Count >= 99 && this.trackList.Count < 999)
                        {
                            if (i < 9)
                            {
                                rowNumber = "00" + (i + 1).ToString();
                            }
                            else if(i >= 9 && i < 99)
                            {
                                rowNumber = "0" + (i + 1).ToString();
                            }
                            else
                            {
                                rowNumber = (i + 1).ToString();
                            }
                        }
                        else if (this.trackList.Count >= 999 && this.trackList.Count < 9999)
                        {
                            if (i < 9)
                            {
                                rowNumber = "000" + (i + 1).ToString();
                            }
                            else if (i >= 9 && i < 99)
                            {
                                rowNumber = "00" + (i + 1).ToString();
                            }
                            else if (i >= 99 && i < 999)
                            {
                                rowNumber = "0" + (i + 1).ToString();
                            }
                            else
                            {
                                rowNumber = (i + 1).ToString();
                            }
                        }


                    }
                    if (this.isKeyCodeChecked)
                    {
                        TrackTagValue ttv = this.trackList[i].TrackTagValues.Find(x => x.TagName == "Key");
                        if(ttv != null && !String.IsNullOrEmpty(ttv.TagValueName))
                        {
                            keyCode = ttv.TagValueName;
                        }
                    }
                    if (this.isBpmNumberChecked)
                    {
                        TrackTagValue ttv = this.trackList[i].TrackTagValues.Find(x => x.TagName == "Bpm");
                        if (ttv != null && !String.IsNullOrEmpty(ttv.Value))
                        {
                            bpmNumber = ttv.Value;
                            if (bpmNumber.Contains(','))
                            {
                                String[] parts = bpmNumber.Split(',');
                                if(parts.Length > 1)
                                {
                                    if (parts[1] == "0")
                                    {
                                        bpmNumber = parts[0];
                                    }
                                }
                            }

                            if (this.isTrunkBpmChecked)
                            {
                                decimal bpmNum = Convert.ToDecimal(ttv.Value);
                                if (bpmNum < 100)
                                {
                                    bpmNumber = bpmNumber.Remove(0, 1);
                                }
                            }
                        }
                    }
                    if (true)
                    {
                        TrackTagValue ttv = this.trackList[i].TrackTagValues.Find(x => x.TagName == "Vocal");
                        if (ttv != null && !String.IsNullOrEmpty(ttv.TagValueName) && (ttv.TagValueName.Equals("Full vocal") || ttv.TagValueName.Equals("Partial vocal")))
                        {
                            hasVocal = "V";
                        }
                    }

                    if (!String.IsNullOrEmpty(hasVocal))
                    {
                        newFileName = hasVocal + "_" + newFileName;
                    }
                    if (!String.IsNullOrEmpty(bpmNumber))
                    {
                        if (!String.IsNullOrEmpty(hasVocal))
                        {
                            newFileName = bpmNumber + newFileName;
                        }
                        else
                        {
                            newFileName = bpmNumber + "_" + newFileName;
                        }
                           
                    }
                    if (!String.IsNullOrEmpty(keyCode))
                    {
                        newFileName = keyCode + "_" + newFileName;
                    }
                    if (!String.IsNullOrEmpty(rowNumber))
                    {
                        newFileName = rowNumber + "_" + newFileName;
                    }

                    if (this.trackList[i].Path.EndsWith(Extension.mp3.ToString()))
                    {
                        newFileName = newFileName + "." + Extension.mp3.ToString();
                    }
                    else if (this.trackList[i].Path.EndsWith(Extension.wav.ToString()))
                    {
                        newFileName = newFileName + "." + Extension.wav.ToString();
                    }
                    else if (this.trackList[i].Path.EndsWith(Extension.ogg.ToString()))
                    {
                        newFileName = newFileName + "." + Extension.ogg.ToString();
                    }
                    else if (this.trackList[i].Path.EndsWith(Extension.flac.ToString()))
                    {
                        newFileName = newFileName + "." + Extension.flac.ToString();
                    }

                    this.trackTable.Rows.Add(newFileName);
                }
            }

            this.trackListBindingSource.DataSource = this.trackTable;
            this.view.SetTrackListBindingSource(this.trackListBindingSource);
        }

        private void BrowseEvent(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (Directory.Exists(this.exportPath))
                    fbd.SelectedPath = this.exportPath;

                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    this.exportPath = fbd.SelectedPath;
                    this.InitializeSettings();
                }

                
            }

        }
        private void SetRowNumberEvent(object sender, Messenger e)
        {
            this.isRowNumberChecked = e.BooleanField1;
            this.InitializeDataTable();
        }
        private void SetKeyCodeEvent(object sender, Messenger e)
        {
            this.isKeyCodeChecked = e.BooleanField1;
            this.InitializeDataTable();
        }
        private void SetBpmNumberEvent(object sender, Messenger e)
        {
            this.isBpmNumberChecked = e.BooleanField1;
            this.InitializeDataTable();
        }
        private void SetTrunkBpmEvent(object sender, Messenger e)
        {
            this.isTrunkBpmChecked = e.BooleanField1;
            this.InitializeDataTable();
        }
        private void SetTrunkedArtistEvent(object sender, Messenger e)
        {
            this.isTrunkedArtistChecked = e.BooleanField1;
            this.InitializeDataTable();
        }
        private void SetTrunkedTitleEvent(object sender, Messenger e)
        {
            this.isTrunkedTitleChecked = e.BooleanField1;
            this.InitializeDataTable();
        }
        private void SetArtistMinimumCharacterEvent(object sender, Messenger e)
        {
            this.artistMinimumCharacter = e.DecimalField1;
            this.InitializeDataTable();
        }
        private void SetTitleMinimumCharacterEvent(object sender, Messenger e)
        {
            this.titleMinimumCharacter = e.DecimalField1;
            this.InitializeDataTable();
        }
        private void CloseViewWithOk(object sender, EventArgs e)
        {
            this.settingDao.SetStringSetting(Settings.LastExportDirectoryPath.ToString(), this.exportPath);
            this.settingDao.SetBooleanSetting(Settings.IsRowNumberChecked.ToString(), this.isRowNumberChecked);
            this.settingDao.SetBooleanSetting(Settings.IsKeyCodeChecked.ToString(), this.isKeyCodeChecked);
            this.settingDao.SetBooleanSetting(Settings.IsBpmNumberChecked.ToString(), this.isBpmNumberChecked);
            this.settingDao.SetBooleanSetting(Settings.IsTrunkedBpmChecked.ToString(), this.isTrunkBpmChecked);
            this.settingDao.SetBooleanSetting(Settings.IsTrunkedArtistChecked.ToString(), this.isTrunkedArtistChecked);
            this.settingDao.SetBooleanSetting(Settings.IsTrunkedTitleChecked.ToString(), this.isTrunkedTitleChecked);
            this.settingDao.SetDecimalSetting(Settings.ArtistMinimumCharacter.ToString(), this.artistMinimumCharacter);
            this.settingDao.SetDecimalSetting(Settings.TitleMinimumCharacter.ToString(), this.titleMinimumCharacter);

            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            
            if (!Directory.Exists(this.exportPath))
            {
                MessageBox.Show("Export directory path is invalid!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if(this.trackTable == null || this.trackTable.Rows == null || this.trackTable.Rows.Count == 0)
                {
                    MessageBox.Show("Tracklist must be given!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        String sourcePath = String.Empty;
                        String targetDir = this.exportPath;
                        String targetFileName = String.Empty;

                        for (int i = 0; i <= this.trackTable.Rows.Count - 1; i++)
                        {
                            sourcePath = this.trackList[i].Path;
                            targetFileName = this.trackTable.Rows[i]["Filename"].ToString();
                            targetFileName = r.Replace(targetFileName, "");
                            File.Copy(sourcePath, Path.Combine(targetDir, targetFileName), true);
                        }

                     ((ExportToDirectoryView)this.view).DialogResult = DialogResult.OK;
                        ((ExportToDirectoryView)this.view).Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
            }

           
        }
        private void CloseViewWithCancel(object sender, EventArgs e)
        {
            ((ExportToDirectoryView)this.view).DialogResult = DialogResult.Cancel;
            ((ExportToDirectoryView)this.view).Close();
        }
    }
}
