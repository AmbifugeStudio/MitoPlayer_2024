using FlacLibSharp;
using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Presenters
{
    public class MainPresenter
    {
        private IMainView mainView { get; set; }
        private IProfileDao profileDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private ITrackDao trackDao { get; set; }
        private ITagDao tagDao { get; set; }
        private MediaPlayerComponent mediaPlayerComponent { get; set; }
        private object actualView { get; set; }
        private IProfileView profileView { get; set; }
        private IProfileEditorView profileEditorView { get; set; }
        private IPlaylistView playlistView { get; set; }
        private ITagValueView tagValueView { get; set; }

        //private ISelectorView selectorView { get; set; }
        private IRuleEditorView ruleEditorView { get; set; }
        private ITrackEditorView trackEditorView { get; set; }
        private ITemplateEditorView templateEditorView { get; set; }
        private IHarmonizerView harmonizerView { get; set; }
        private ISettingsView preferencesView { get; set; }
        private IAboutView aboutView { get; set; }
        private ISetupView setupView { get; set; }
        private ProfileEditorPresenter profileEditorPresenter { get; set; }
        private PlaylistPresenter playlistPresenter { get; set; }
        private TagValuePresenter tagValueEditorPresenter { get; set; }
        private RuleEditorPresenter ruleEditorPresenter { get; set; }
        private TrackEditorPresenter trackEditorPresenter { get; set; }
        private TemplateEditorPresenter templateEditorPresenter { get; set; }
        private HarmonizerPresenter harmonizerPresenter { get; set; }
        private SettingsPresenter preferencesPresenter { get; set; }
        private AboutPresenter aboutPresenter { get; set; }
        private string[] scannedFileNames { get; set; }
        private string connectionString { get; set; }

        public MainPresenter(IMainView mainView, String connectionString, SettingDao settingDao)
        {
            this.mainView = mainView;
            this.connectionString = connectionString;

            //VIEW CONTROLS
            this.mainView.ShowProfileEditorView += ShowProfileEditorView;
            this.mainView.ShowPlaylistView += ShowPlaylistView;
            this.mainView.ShowTagValueEditorView += ShowTagValueEditorView;
            this.mainView.ShowRuleEditorView += ShowRuleEditorView;
            this.mainView.ShowTrackEditorView += ShowTrackEditorView;
            this.mainView.ShowTemplateEditorView += ShowTemplateEditorView;
            this.mainView.ShowHarmonizerView += ShowHarmonizerView;
            this.mainView.ShowPreferencesView += ShowSettingsView;
            this.mainView.ShowAboutView += ShowAboutView;
            this.mainView.Settings += Preferences;
            this.mainView.About += About;

            //FUNCTIONS
            //Media Player
            this.mainView.PlayTrack += PlayTrack;
            this.mainView.PauseTrack += PauseTrack;
            this.mainView.StopTrack += StopTrack;
            this.mainView.PrevTrack += PrevTrack;
            this.mainView.NextTrack += NextTrack;
            this.mainView.RandomTrack += RandomTrack;
            this.mainView.ChangeVolume += ChangeVolume;
            this.mainView.ChangeProgress += ChangeProgress;
            this.mainView.ChangeMute += ChangeMute;
            this.mainView.ChangeShuffle += ChangeShuffle;
            this.mainView.ChangePreview += ChangePreview;
            //Tracklist
            this.mainView.OpenFiles += OpenFiles;
            this.mainView.OpenDirectory += OpenDirectory;
            this.mainView.ScanFiles += ScanFiles;
            this.mainView.RemoveMissingTracks += RemoveMissingTracks;
            this.mainView.RemoveDuplicatedTracks += RemoveDuplicatedTracks;
            this.mainView.OrderByTitle += OrderByTitle;
            this.mainView.OrderByArtist += OrderByArtist;
            this.mainView.OrderByFileName += OrderByFileName;
            this.mainView.Reverse += Reverse;
            this.mainView.Shuffle += Shuffle;
            this.mainView.Clear += Clear;
            //PLaylist
            this.mainView.CreatePlaylist += CreatePlaylist;
            this.mainView.LoadPlaylist += LoadPlaylist;
            this.mainView.RenamePlaylist += RenamePlaylist;
            this.mainView.DeletePlaylist += DeletePlaylist;
            this.mainView.ExportToM3U += ExportToM3U;
            this.mainView.ExportToTXT += ExportToTXT;
            this.mainView.ExportToDirectory += ExportToDirectory;
            this.mainView.GetMediaPlayerProgressStatusEvent += GetMediaPlayerProgressStatusEvent;

            //Other
            this.mainView.Exit += Exit;

            this.settingDao = settingDao;
            this.profileDao = new ProfileDao(connectionString);
            this.trackDao = new TrackDao(connectionString);
            this.tagDao = new TagDao(connectionString);

            this.InitializeProfileAndPlaylist();
            this.InitializeViewsAndPresenters();
        }

        public void InitializeProfileAndPlaylist()
        {
            ResultOrError result = new ResultOrError();

            this.actualView = null;
            this.mediaPlayerComponent = null;
            this.mediaPlayerComponent = new MediaPlayerComponent(((MainView)this.mainView).mediaPlayer, this.settingDao);

            Profile profile = null;
            ResultOrError<Profile> profResult = this.profileDao.GetActiveProfile();
            if (profResult)
            {
                if(profResult.Value == null)
                {
                    profile = new Profile();
                    profile.Id = 1;
                    profile.Name = DefaultName.Profile.ToString();
                    profile.IsActive = true;

                    result = this.profileDao.CreateProfile(profile);
                }
                else
                {
                    profile = profResult.Value;
                }
            }
            else
            {
                result.AddError(profResult.ErrorMessage);
            }

            if (result)
            {
                this.settingDao.SetProfileId(profile.Id);
                this.trackDao.SetProfileId(profile.Id);
                this.tagDao.SetProfileId(profile.Id);
            }

            if (result)
            {
                result = this.settingDao.InitializeProfileSettings();
            }

            Playlist pls = null;
            if (result)
            {
                //Default playlist initialize
                ResultOrError<Playlist> plsResult = this.trackDao.GetActivePlaylist();
                if (plsResult)
                {
                    if (plsResult.Value == null)
                    {
                        pls = new Playlist();
                        pls.Id = 1;
                        pls.Name = "Default Playlist";
                        pls.OrderInList = 0;
                        pls.QuickListGroup = 0;
                        pls.IsActive = true;
                        pls.ProfileId = profile.Id;

                        result = this.trackDao.CreatePlaylist(pls);

                        if (result)
                            result = this.CreateTestData(profile.Id);
                        if (result)
                            result = this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), 1);
                    }
                    else
                    {
                        pls = plsResult.Value;
                    }
                }
                else
                {
                    result.AddError(plsResult.ErrorMessage);
                }
                
            }
            if (result)
            {
                //this.ShowPlaylistView(this, new EventArgs());
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        public void InitializeViewsAndPresenters()
        {
            this.playlistView = PlaylistView.GetInstance((MainView)this.mainView);
            this.playlistPresenter = new PlaylistPresenter(this.playlistView, this.trackDao, this.tagDao, this.settingDao);

            this.tagValueView = TagValueView.GetInstance((MainView)this.mainView);
            this.tagValueEditorPresenter = new TagValuePresenter(this.tagValueView, this.tagDao, this.trackDao, this.settingDao);

            //this.selectorView = SelectorView.GetInstance((MainView)this.mainView);
           // this.playlistPresenter = new PlaylistPresenter(this.playlistView, this.trackDao, this.tagDao, this.settingDao);

            this.ShowPlaylistView(this, new EventArgs());
        }

       

        private ResultOrError CreateTestData(int profileId)
        {
            ResultOrError result = new ResultOrError();

            String[] keyNameArray = new String[0];
            String[] keyColorArray = new String[0];
            this.settingDao.InitializeKeys(ref keyNameArray,ref keyColorArray);

            Tag tag = new Tag()
            {
                Id = this.settingDao.GetNextId(TableName.Tag.ToString()),
                Name = "Key",
                TextColoring = true,
                HasMultipleValues = false,
                IsIntegrated = true,
                ProfileId = profileId
            };

            result = this.tagDao.CreateTag(tag);

            if (result)
            {
                TrackProperty tp = new TrackProperty()
                {
                    Id =  this.settingDao.GetNextId(TableName.TrackProperty.ToString()),
                    Name = tag.Name,
                    Type = "System.String",
                    IsEnabled = true,
                    ColumnGroup = ColumnGroup.TracklistColumns.ToString(),
                    SortingId = this.settingDao.GetNextTrackPropertySortingId().Value,
                };

                result = this.settingDao.CreateTrackProperty(tp);

                tp = new TrackProperty()
                {
                    Id = this.settingDao.GetNextId(TableName.TrackProperty.ToString()),
                    Name = tag.Name + "TagValueId",
                    Type = "System.Int32",
                    IsEnabled = false,
                    ColumnGroup = ColumnGroup.TracklistColumns.ToString(),
                    SortingId = this.settingDao.GetNextTrackPropertySortingId().Value,
                };

                result = this.settingDao.CreateTrackProperty(tp);

            }

            if (result)
            {
                TagValue tv = null;
                for (int i = 0; i <= keyNameArray.Count() - 1; i++)
                {
                    tv = new TagValue()
                    {
                        Id = this.settingDao.GetNextId(TableName.TagValue.ToString()),
                        TagId = tag.Id,
                        TagName = tag.Name,
                        Name = keyNameArray[i],
                        Color = this.HexToColor(keyColorArray[i]),
                        ProfileId = profileId
                    };

                    result = this.tagDao.CreateTagValue(tv);

                    if (!result)
                    {
                        break;
                    }
                }
            }

            if (result)
            {
                tag = new Tag()
                {
                    Id = this.settingDao.GetNextId(TableName.Tag.ToString()),
                    Name = "Bpm",
                    TextColoring = true,
                    HasMultipleValues = true,
                    IsIntegrated = true,
                    ProfileId = profileId
                };

                result = this.tagDao.CreateTag(tag);
            }

            if (result)
            {
                TrackProperty tp = new TrackProperty()
                {
                    Id = this.settingDao.GetNextId(TableName.TrackProperty.ToString()),
                    Name = tag.Name,
                    Type = "System.String",
                    IsEnabled = true,
                    ColumnGroup = ColumnGroup.TracklistColumns.ToString(),
                    SortingId = this.settingDao.GetNextTrackPropertySortingId().Value
                };

                result = this.settingDao.CreateTrackProperty(tp);
            }

            if (result)
            {
                TrackProperty tp = new TrackProperty()
                {
                    Id = this.settingDao.GetNextId(TableName.TrackProperty.ToString()),
                    Name = tag.Name + "TagValueId",
                    Type = "System.Decimal",
                    IsEnabled = false,
                    ColumnGroup = ColumnGroup.TracklistColumns.ToString(),
                    SortingId = this.settingDao.GetNextTrackPropertySortingId().Value,
                };

                result = this.settingDao.CreateTrackProperty(tp);
            }

            if (result)
            {
                TagValue tv = new TagValue()
                {
                    Id = this.settingDao.GetNextId(TableName.TagValue.ToString()),
                    TagId = tag.Id,
                    TagName = tag.Name,
                    Name = "Bpm",
                    Color = Color.White,
                    ProfileId = profileId
                };

                result = this.tagDao.CreateTagValue(tv);
            }

            if (result)
            {
                TrainingData trainingData = new TrainingData()
                {
                    Name = "VocalTemplate",
                    CreateDate = DateTime.Now,
                    SampleCount = 0,
                    Balance = 0,
                    ExtractMFCCs = true,
                    ExtractChromaFeatures = true,
                    ExtractSpectralContrast = true,
                    ExtractZeroCrossingRate = true,
                    ExtractRmsEnergy = true,
                    ExtractPitch = true,
                    ExtractSpectralCentroid = true,
                    ExtractSpectralBandwidth = true,
                    IsTemplate = true,
                    ProfileId = profileId

                };

                //this.trackDao.CreateTrainingData(trainingData);
            }

            return result;    

           /* List<TrackProperty> defaultTpList = new List<TrackProperty>();
            List<TrackProperty> customTpList = new List<TrackProperty>();
            defaultTpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString(), true);
            customTpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString());
            if (defaultTpList != null && defaultTpList.Count > 0)
            {
                if (customTpList != null && customTpList.Count > 0)
                {
                    defaultTpList.AddRange(customTpList);
                }

                if (defaultTpList != null && defaultTpList.Count > 0)
                {
                    defaultTpList = defaultTpList.OrderBy(x => x.SortingId).ToList();
                    for (int i = 0; i <= defaultTpList.Count -1; i++)
                    {
                        this.settingDao.UpdateTrackProperty(defaultTpList[i], true);
                    }
                }
            }*/
        }
        private Color HexToColor(string hexValue)
        {
            return System.Drawing.ColorTranslator.FromHtml(hexValue);
        }

        private void ReloadMainView()
        {
            

            /* if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                 ((PlaylistView)this.playlistView).CallStopTrackEvent();
             else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                 ((TrackEditorView)this.trackEditorView).CallStopTrackEvent();
             else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                 ((HarmonizerView)this.harmonizerView).CallStopTrackEvent();*/

            // this.InitializeProfileAndPlaylist();
            // this.ShowPlaylistView();

            // this.InitializeProfileAndPlaylist();
            // this.InitializeViewsAndPresenters();

            //  if (this.playlistView != null && this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
            // {


            /* bool automaticKeyImport = this.settingDao.GetBooleanSetting(Settings.AutomaticKeyImport.ToString()).Value;
             bool automaticBpmImport = this.settingDao.GetBooleanSetting(Settings.AutomaticBpmImport.ToString()).Value;

             if ((automaticKeyImport == false && automaticBpmImport == false) || (automaticKeyImport == true || automaticBpmImport == true))
             {
                 ((PlaylistView)this.playlistView).SetKeyAndBpmAnalization(this.HasVirtualDj() && (automaticKeyImport || automaticBpmImport));
             }*/
            // }
        }

        //VIEWS
        private void ShowProfileEditorView(object sender, EventArgs e)
        {
            ProfileView profileView = new ProfileView();
            ProfilePresenter presenter = new ProfilePresenter(profileView, this.profileDao, this.settingDao, this.trackDao, this.tagDao);
            if(profileView.ShowDialog((MainView)this.mainView) == DialogResult.OK)
            {
                this.ReloadMainView();
            }
        }
        private void HideAllForm()
        {
            ((PlaylistView)this.playlistView).Hide();
            ((TagValueView)this.tagValueView).Hide();
           /* ((TrackEditorView)this.trackEditorView).Hide();
            ((RuleEditorView)this.ruleEditorView).Hide();
            ((TemplateEditorView)this.templateEditorView).Hide();
            ((HarmonizerView)this.harmonizerView).Hide();*/
        }
        private void ShowPlaylistView(object sender, EventArgs e)
        {
            this.ShowPlaylistView();
        }
        private void ShowPlaylistView() {
            this.HideAllForm();

            this.playlistPresenter.Initialize(this.mediaPlayerComponent);
            ((PlaylistView)this.playlistView).Show();

            this.actualView = this.playlistView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
        }



        private void ShowTagValueEditorView(object sender, EventArgs e)
        {
            this.HideAllForm();

            this.tagValueEditorPresenter.Initialize();
            ((TagValueView)this.tagValueView).Show();

            this.tagValueEditorPresenter.ReloadData();

            this.actualView = this.tagValueView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
        }
        private void ShowTrackEditorView(object sender, EventArgs e)
        {
            this.actualView = TrackEditorView.GetInstance((MainView)mainView);
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.trackEditorPresenter = new TrackEditorPresenter((ITrackEditorView)this.actualView, this.mediaPlayerComponent, this.trackDao, this.settingDao);
        }
        private void ShowRuleEditorView(object sender, EventArgs e)
        {
            this.actualView = RuleEditorView.GetInstance((MainView)mainView);
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.ruleEditorPresenter = new RuleEditorPresenter((IRuleEditorView)this.actualView, this.trackDao, this.settingDao);
        }
        private void ShowTemplateEditorView(object sender, EventArgs e)
        {
            this.actualView = TemplateEditorView.GetInstance((MainView)mainView);
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.templateEditorPresenter = new TemplateEditorPresenter((ITemplateEditorView)this.actualView, this.trackDao, this.settingDao);
        }
        private void ShowHarmonizerView(object sender, EventArgs e)
        {
            this.actualView = HarmonizerView.GetInstance((MainView)mainView);
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.harmonizerPresenter = new HarmonizerPresenter((IHarmonizerView)this.actualView, this.mediaPlayerComponent, this.trackDao, this.settingDao);
        }
        private void ShowSettingsView(object sender, EventArgs e)
        {
            SettingsView preferencesView = new SettingsView();
            SettingsPresenter presenter = new SettingsPresenter(preferencesView, this.trackDao, this.tagDao, this.profileDao, this.settingDao);
            preferencesView.ShowDialog((SettingsView)this.preferencesView);

            

           // this.ReloadMainView();

            /*if (presenter.databaseCleared)
            {
                presenter.databaseCleared = false;
               
            }*/
        }
        private void ShowAboutView(object sender, EventArgs e)
        {
            AboutView aboutView = new AboutView();
            AboutPresenter presenter = new AboutPresenter(aboutView);
            aboutView.ShowDialog((AboutView)this.aboutView);
        }

        //MENU STRIP
        //FILE
        private void OpenFiles(object sender, EventArgs e)
        {
            //megnyílik egy fájl böngésző, a setting-ből beállítódik az utoljára beolvasott fájlkiterjesztés
            //a kiválasztott fájlokból Track-eket csinálunk
            //lementjük setting-be az utoljára beállított fájl kiterjesztést
            //a létrejött Track-eket hozzáadjuk az aktuális TrackList-hez

            List<Model.Track> trackList = new List<Model.Track>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Audio files (*.mp3,*.wav,*.flac)|*.mp3;*.wav;*.flac|Playlist files (*.m3u)|*.m3u";
            ofd.FilterIndex = this.settingDao.GetIntegerSetting(Settings.LastOpenFilesFilterIndex.ToString()).Value;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                trackList = this.ReadFiles(ofd.FileNames);
            }
            this.settingDao.SetIntegerSetting(Settings.LastOpenFilesFilterIndex.ToString(), ofd.FilterIndex);

            this.AddTracksToTrackList(trackList);

        }

        

        private void OpenDirectory(object sender, EventArgs e)
        {
            scannedFileNames = null;
            List<Model.Track> trackList = new List<Model.Track>();

            using (var fbd = new FolderBrowserDialog())
            {
                String lastDirectoryPath = this.settingDao.GetStringSetting(Settings.LastOpenDirectoryPath.ToString()).Value;

                if (Directory.Exists(lastDirectoryPath))
                    fbd.SelectedPath = lastDirectoryPath;

                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    this.ScanDirectory(fbd.SelectedPath);
                    trackList = this.ReadFiles(scannedFileNames);
                }

                this.settingDao.SetStringSetting(Settings.LastOpenDirectoryPath.ToString(), fbd.SelectedPath);
            }

            this.AddTracksToTrackList(trackList);
        }
       

        private void CreatePlaylist(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallCreatePlaylistEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallCreatePlaylistEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallCreatePlaylistEvent();
        }
        private void LoadPlaylist(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallLoadPlaylistEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallLoadPlaylistEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallLoadPlaylistEvent();
        }
        private void RenamePlaylist(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallRenamePlaylistEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallRenamePlaylistEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallRenamePlaylistEvent();
        }
        private void DeletePlaylist(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallDeletePlaylistEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallDeletePlaylistEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallDeletePlaylistEvent();
        }
        private void ExportToM3U(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallExportToM3UEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallExportToM3UEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallExportToM3UEvent();
        }
        private void ExportToTXT(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallExportToTXTEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallExportToTXTEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallExportToTXTEvent();
        }
        private void ExportToDirectory(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallExportToDirectoryEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallExportToDirectoryEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallExportToDirectoryEvent();
        }
        private void Preferences(object sender, EventArgs e)
        {
            this.ShowSettingsView(this, new EventArgs());
        }
        private void Exit(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        //EDIT
        private void RemoveMissingTracks(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.RemoveMissingTracks();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.RemoveMissingTracks();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.RemoveMissingTracks();
        }
        private void RemoveDuplicatedTracks(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.RemoveDuplicatedTracks();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.RemoveDuplicatedTracks();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.RemoveDuplicatedTracks();
        }
        private void OrderByTitle(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.OrderByTitle();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.OrderByTitle();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.OrderByTitle();
        }
        private void OrderByArtist(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.OrderByArtist();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.OrderByArtist();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.OrderByArtist();
        }
        private void OrderByFileName(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.OrderByFileName();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.OrderByFileName();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.OrderByFileName();
        }
        private void Shuffle(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.Shuffle();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.Shuffle();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.Shuffle();
        }
        private void Reverse(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.Reverse();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.Reverse();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.Reverse();
        }
        private void Clear(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.Clear();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.Clear();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.Clear();
        }

        //PLAYBACK
        private void PlayTrack(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallPlayTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallPlayTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallPlayTrackEvent();
        }
        private void PauseTrack(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallPauseTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallPauseTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallPauseTrackEvent();
        }
        private void StopTrack(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallStopTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallStopTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallStopTrackEvent();
        }
        private void PrevTrack(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallPrevTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallPrevTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallPrevTrackEvent();
        }
        private void NextTrack(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallNextTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallNextTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallNextTrackEvent();
        }
        private void RandomTrack(object sender, EventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.actualView).CallRandomTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.actualView).CallRandomTrackEvent();
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.actualView).CallRandomTrackEvent();
        }
        private void ChangeProgress(object sender, Messenger e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallChangeProgressEvent(e.IntegerField1, e.IntegerField2);
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallChangeProgressEvent(e.IntegerField1, e.IntegerField2);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallChangeProgressEvent(e.IntegerField1, e.IntegerField2);
        }
        private void ChangeVolume(object sender, Messenger e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallChangeVolumeEvent(e.IntegerField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallChangeVolumeEvent(e.IntegerField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallChangeVolumeEvent(e.IntegerField1);
        }
        private void ChangeMute(object sender, Messenger e)
        {



            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallChangeMuteEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallChangeMuteEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallChangeMuteEvent(e.BooleanField1);
        }
        private void ChangeShuffle(object sender, Messenger e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallChangeShuffleEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallChangeShuffleEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallChangeShuffleEvent(e.BooleanField1);
        }
        private void ChangePreview(object sender, Messenger e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallChangePreviewEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallChangePreviewEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallChangePreviewEvent(e.BooleanField1);
        }

        //HELP
        private void About(object sender, EventArgs e)
        {
            this.ShowAboutView(this, new EventArgs());
        }

        //ADD FILES
        private void AddTracksToTrackList(List<Model.Track> trackList, int dragIndex = -1)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                this.playlistPresenter.CallAddTrackToTrackListEvent(trackList, dragIndex);
            }
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallAddTrackToTrackListEvent(trackList, dragIndex);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallAddTrackToTrackListEvent(trackList, dragIndex);
        }
        private void ScanDirectory(string path)
        {
            string[] files = Directory.GetFiles(path);
            files = files.Where(x => x.Contains(".mp3") || x.Contains(".flac") || x.Contains(".wav")).ToArray();

            if (files != null && files.Length > 0)
            {
                if (scannedFileNames == null)
                    scannedFileNames = files;
                else
                    scannedFileNames = scannedFileNames.Concat(files).ToArray();
            }

            string[] directories = Directory.GetDirectories(path);
            foreach (String dir in directories)
            {
                this.ScanDirectory(dir);
            }
        }
        
        private List<Model.Track> ReadFiles(string[] fileNames)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
               
                loadingDialog.SetProcessDescription("Read files and creating/loading tracks...");
                loadingDialog.Refresh();

                List<String> filePathList = new List<String>();
                List<Model.Track> trackList = new List<Model.Track>();

                if (fileNames != null && fileNames.Length > 0)
                {
                    foreach (String path in fileNames)
                    {
                        if (path.EndsWith(".mp3") || path.EndsWith(".wav") || path.EndsWith(".flac"))
                        {
                            filePathList.Add(path);
                        }
                        else if (path.Contains(".m3u"))
                        {
                            const Int32 BufferSize = 128;
                            char[] firstThreeCharacter;
                            using (var fileStream = System.IO.File.OpenRead(path))
                            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                            {
                                String line;
                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    if (line.Length > 2 && line[1] == ':' && line[2] == '\\')
                                    {
                                        filePathList.Add(line);
                                    }
                                }
                            }
                        }
                    }

                    List<Tag> tagList = tagDao.GetAllTag().Value;

                    foreach (var path in filePathList)
                    {
                        var track = new Model.Track() { Path = path };
                        var fileName = Path.GetFileNameWithoutExtension(path);
                        track.FileName = fileName;

                        if (!System.IO.File.Exists(path))
                        {
                            track.Artist = fileName;
                            track.IsMissing = true;
                        }
                        else
                        {
                            Model.Track trackFromDb = this.trackDao.GetTrackWithTagsByPath(track.Path, tagList).Value;
                            if (trackFromDb != null)
                            {
                                track = trackFromDb;
                                track.IsNew = false;
                            }
                            else
                            {
                                if (track.Path.EndsWith(".mp3"))
                                {
                                    using (var file = TagLib.File.Create(track.Path))
                                    {
                                        track.Artist = file.Tag.FirstArtist ?? file.Tag.FirstAlbumArtist ?? file.Tag.Artists.FirstOrDefault() ?? fileName;
                                        track.Album = file.Tag.Album;
                                        track.Title = file.Tag.Title;
                                        track.Year = (int)file.Tag.Year;
                                        track.Length = file.Properties.Duration.TotalSeconds;
                                        track.Comment = file.Tag.Comment;
                                    }
                                }
                                else if (path.EndsWith(".wav"))
                                {
                                    using (var wf = new WaveFileReader(path))
                                    {
                                        track.Artist = fileName;
                                        track.Length = wf.TotalTime.TotalSeconds;
                                    }
                                }
                                else if (path.EndsWith(".flac"))
                                {
                                    using (var file = new FlacFile(path))
                                    {
                                        var vorbisComment = file.VorbisComment;
                                        if (vorbisComment != null)
                                        {
                                            track.Artist = vorbisComment.Artist.FirstOrDefault() ?? fileName;
                                            track.Album = vorbisComment.Album.FirstOrDefault();
                                            track.Title = vorbisComment.Title.FirstOrDefault();
                                            track.Year = int.TryParse(vorbisComment.Date.FirstOrDefault()?.Substring(0, 4), out int year) ? year : 0;
                                            
                                            var comment = vorbisComment["COMMENT"];
                                            if (comment != null && comment.Count > 0)
                                            {
                                                track.Comment = comment[0];
                                            }
                                        }
                                        track.Length = file.StreamInfo.Duration;
                                    }
                                }
                                track.IsNew = true;

                                track.Id = this.trackDao.GetNextId(TableName.Track.ToString());

                                try
                                {
                                    this.trackDao.CreateTrack(track);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                track.TrackTagValues = new List<TrackTagValue>();

                                foreach (Tag tag in tagList)
                                {
                                    TrackTagValue ttv = new TrackTagValue();
                                    ttv.TrackId = track.Id;
                                    ttv.TagId = tag.Id;
                                    ttv.TagName = tag.Name;
                                    ttv.TagValueId = -1;
                                    ttv.TagValueName = String.Empty;
                                    ttv.Value = "-1";
                                    ttv.HasMultipleValues = tag.HasMultipleValues;
                                    ttv.ProfileId = this.trackDao.GetProfileId();
                                    this.trackDao.CreateTrackTagValue(ttv);
                                    track.TrackTagValues.Add(ttv);
                                }


                            }
                        }
                        lock (trackList)
                        {
                            trackList.Add(track);
                        }
                    }

                    this.ImportKeysAndBpmsFromExternalSource(ref trackList, tagList);

                }
                return trackList;
            }
        }

        private void ImportKeysAndBpmsFromExternalSource(ref List<Model.Track> trackList,List<Tag> tagList)
        {
            bool automaticKeyImport = this.settingDao.GetBooleanSetting(Settings.AutomaticKeyImport.ToString()).Value.Value;
            bool automaticBpmImport = this.settingDao.GetBooleanSetting(Settings.AutomaticBpmImport.ToString()).Value.Value;
            bool importKeyFromVirtualDj = this.settingDao.GetBooleanSetting(Settings.ImportKeyFromVirtualDj.ToString()).Value.Value;
            bool importBpmFromVirtualDj = this.settingDao.GetBooleanSetting(Settings.ImportBpmFromVirtualDj.ToString()).Value.Value;

            if (automaticKeyImport && automaticBpmImport && importKeyFromVirtualDj && importBpmFromVirtualDj)
            {
                if (this.HasVirtualDj())
                {
                    List<TagValue> keyTagValueList = this.tagDao.GetTagValuesByTagId(tagList.Find(x => x.Name == "Key")?.Id ?? 0).Value;
                    List<TagValue> bpmTagValueList = this.tagDao.GetTagValuesByTagId(tagList.Find(x => x.Name == "Bpm")?.Id ?? 0).Value;
                    var result = VirtualDJReader.Instance.ReadKeysAndBpmsFromVirtualDJDatabase(ref trackList, this.trackDao, keyTagValueList, bpmTagValueList);
                    if (!result.Success)
                    {
                        MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                if (automaticKeyImport)
                {
                    List<TagValue> keyTagValueList = this.tagDao.GetTagValuesByTagId(tagList.Find(x => x.Name == "Key")?.Id ?? 0).Value;

                    if (importKeyFromVirtualDj)
                    {
                        if (this.HasVirtualDj())
                        {
                            var result = VirtualDJReader.Instance.ReadKeysFromVirtualDJDatabase(ref trackList, this.trackDao, keyTagValueList);
                            if (!result.Success)
                            {
                                MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        TagValue tv = null;
                        String tagValueName = String.Empty;
                        String keyCode = String.Empty;
                        String[] parts = null;

                        foreach (Track track in trackList)
                        {
                            if (!String.IsNullOrEmpty(track.Comment))
                            {
                                parts = track.Comment.Split('-');
                                if (parts != null && parts.Length > 0)
                                {
                                    keyCode = parts[0].TrimEnd();

                                    if (keyCode.Count() == 2)
                                    {
                                        keyCode = "0" + keyCode;
                                    }

                                    tv = keyTagValueList.Find(x => x.Name == keyCode);

                                    if (tv != null)
                                    {
                                        if (track.TrackTagValues != null && track.TrackTagValues.Count > 0)
                                        {
                                            TrackTagValue ttv = track.TrackTagValues.Find(x => x.TagName == "Key");
                                            if (ttv != null)
                                            {
                                                ttv.TagValueId = tv.Id;
                                                ttv.TagValueName = tv.Name;
                                                this.trackDao.UpdateTrackTagValue(ttv);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                if (automaticBpmImport)
                {
                    if (importBpmFromVirtualDj)
                    {
                        if (this.HasVirtualDj())
                        {
                            List<TagValue> bpmTagValueList = this.tagDao.GetTagValuesByTagId(tagList.Find(x => x.Name == "Bpm")?.Id ?? 0).Value;
                            var result = VirtualDJReader.Instance.ReadBpmsFromVirtualDJDatabase(ref trackList, this.trackDao, bpmTagValueList);
                            if (!result.Success)
                            {
                                MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }

            }
        }

        private bool HasVirtualDj()
        {
            bool result = false;

            String letters = "ABCDEFGHIJKLMNOPQRSTIJKLMNOPQRSTUVWXYZ";
            String vdjDatabaseFilePath = String.Empty;

            foreach (char drive in letters)
            {
                vdjDatabaseFilePath = drive + ":\\VirtualDJ\\database.xml";
                if (File.Exists(vdjDatabaseFilePath))
                {
                    result = true;
                    break;
                }
            }

            vdjDatabaseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\VirtualDJ\\database.xml";
            if (File.Exists(vdjDatabaseFilePath))
            {
                result = true;
            }

            return result;
        }

        private String[] scannedFiles;
        private List<Model.Track> trackList = new List<Model.Track>();
        private void ScanFiles(object sender, Messenger e)
        {
            string[] mediaFiles;
            string[] directories;
            trackList = new List<Model.Track>();
            int dragIndex = e.IntegerField1;
            if (e.DragAndDropFilePathArray != null && e.DragAndDropFilePathArray.Length > 0)
            {
                mediaFiles = e.DragAndDropFilePathArray.Where(x => x.EndsWith(".mp3") || x.EndsWith(".wav") || x.EndsWith(".flac") || x.EndsWith(".m3u")).ToArray();
                directories = e.DragAndDropFilePathArray.Where(x => !x.EndsWith(".mp3") && !x.EndsWith(".wav") && !x.EndsWith(".flac") && !x.EndsWith(".m3u")).ToArray();

                if (mediaFiles != null && mediaFiles.Length > 0)
                {
                    trackList = this.ReadFiles(mediaFiles);
                }
                if (directories != null && directories.Length > 0)
                {
                    scannedFileNames = null;
                    foreach (string dir in directories)
                    {
                        this.ScanDirectory(dir);
                        trackList.AddRange(this.ReadFiles(scannedFileNames));
                    }
                }

                this.AddTracksToTrackList(trackList, dragIndex);
            }
        }


        private void GetMediaPlayerProgressStatusEvent(object sender, EventArgs e)
        {
            if (this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.mainView.UpdateMediaPlayerProgressStatus(
                    this.mediaPlayerComponent.GetDuration(),
                    this.mediaPlayerComponent.GetDurationString(),
                    this.mediaPlayerComponent.GetCurrentPosition(),
                    this.mediaPlayerComponent.GetCurrentPositionString());
            }
            else
            {
                this.mainView.ResetMediaPlayerProgressStatus();
                
            }
/*
            if (this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying ||
                this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                bool isFormOpen = Application.OpenForms.OfType<ChartView>().Any();
                if (this.chartView != null && isFormOpen)
                {
                    this.chartPresenter.UpdateLinePosition(this.mediaPlayerComponent.MediaPlayer.Ctlcontrols.currentPosition);
                }
            }
            else
            {
                bool isFormOpen = Application.OpenForms.OfType<ChartView>().Any();
                if (this.chartView != null && isFormOpen)
                {
                    this.chartPresenter.UpdateLinePosition(0);
                }
            }*/
        }

       /* private ChartView chartView { get; set; }
        private ChartPresenter chartPresenter { get; set; }
        private void OpenChartEvent(object sender, EventArgs e)
        {
            if (this.mediaPlayerComponent != null && this.mediaPlayerComponent.CurrentTrackIdInPlaylist > -1)
            {
                if (this.chartView == null || this.chartView.IsDisposed)
                {
                    this.chartView = new ChartView();
                    this.chartPresenter = new ChartPresenter(chartView, this.mediaPlayerComponent, this.tagDao, this.trackDao, this.settingDao);
                    this.chartView.Show();
                }
                else
                {
                    bool isFormOpen = Application.OpenForms.OfType<ChartView>().Any();
                    if (!isFormOpen)
                    {
                        this.chartView.BringToFront();
                    }
                }
            }
        }*/
    }
}
