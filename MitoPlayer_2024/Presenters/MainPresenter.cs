using FlacLibSharp;
using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static Mysqlx.Expect.Open.Types.Condition.Types;
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
        private IRuleEditorView ruleEditorView { get; set; }
        private ITrackEditorView trackEditorView { get; set; }
        private ITemplateEditorView templateEditorView { get; set; }
        private IHarmonizerView harmonizerView { get; set; }
        private IPreferencesView preferencesView { get; set; }
        private IAboutView aboutView { get; set; }
        private ISetupView setupView { get; set; }
        private ProfileEditorPresenter profileEditorPresenter { get; set; }
        private PlaylistPresenter playlistPresenter { get; set; }
        private TagValuePresenter tagValueEditorPresenter { get; set; }
        private RuleEditorPresenter ruleEditorPresenter { get; set; }
        private TrackEditorPresenter trackEditorPresenter { get; set; }
        private TemplateEditorPresenter templateEditorPresenter { get; set; }
        private HarmonizerPresenter harmonizerPresenter { get; set; }
        private PreferencesPresenter preferencesPresenter { get; set; }
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
            this.mainView.ShowPreferencesView += ShowPreferencesView;
            this.mainView.ShowAboutView += ShowAboutView;
            this.mainView.Preferences += Preferences;
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
            //Other
            this.mainView.Exit += Exit;

            this.settingDao = settingDao;
            this.profileDao = new ProfileDao(connectionString);
            this.trackDao = new TrackDao(connectionString);
            this.tagDao = new TagDao(connectionString);

            this.InitializeProfileAndPlaylist();
            this.InitializeViewsAndPresenters();
        }
        public void InitializeViewsAndPresenters()
        {
            this.playlistView = PlaylistView.GetInstance((MainView)this.mainView);
            this.playlistPresenter = new PlaylistPresenter(this.playlistView, this.trackDao, this.tagDao, this.settingDao);

            this.tagValueView = TagValueView.GetInstance((MainView)this.mainView);
            this.tagValueEditorPresenter = new TagValuePresenter(this.tagValueView, this.tagDao, this.trackDao, this.settingDao);

            this.ShowPlaylistView(this, new EventArgs());
        }

        public void InitializeProfileAndPlaylist()
        {
            ResultOrError result = new ResultOrError();

            this.actualView = null;
            this.mediaPlayerComponent = null;
            this.mediaPlayerComponent = new MediaPlayerComponent(((MainView)this.mainView).mediaPlayer);

            Profile profile = this.profileDao.GetActiveProfile();
            if (profile == null)
            {
                profile = new Profile();
                profile.Id = 0;
                profile.Name = DefaultName.Profile.ToString();
                profile.IsActive = true;
                result = this.profileDao.CreateProfile(profile);
            }

            if (result.Success)
            {
                this.settingDao.SetProfileId(profile.Id);
                this.trackDao.SetProfileId(profile.Id);
                this.tagDao.SetProfileId(profile.Id);
            }

            if (result.Success)
            {
                result = this.settingDao.InitializeProfileSettings();
            }

            if (result.Success)
            {
                //Default playlist inicializálás
                Playlist pls = this.trackDao.GetActivePlaylist();
                if (pls == null)
                {
                    pls = new Playlist();
                    pls.Id = this.trackDao.GetNextId(TableName.Playlist.ToString());
                    pls.Name = "Default Playlist";
                    pls.OrderInList = 0;
                    pls.QuickListGroup = 0;
                    pls.IsActive = true;
                    pls.ProfileId = profile.Id;
                    result = this.trackDao.CreatePlaylist(pls);
                    if (result.Success)
                    {
                        result = this.CreateTestData(profile.Id);
                    }
                    this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), pls.Id);
                }
            }
            if (result.Success)
            {
                //this.ShowPlaylistView(this, new EventArgs());
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            } 
        }

        private ResultOrError CreateTestData(int profileId)
        {
            ResultOrError result = new ResultOrError();

            String[] keyNameArray = new String[0];
            String[] keyColorArray = new String[0];
            this.settingDao.InitializeKeys(ref keyNameArray,ref keyColorArray);

            Tag tag = new Tag()
            {
                Id = this.tagDao.GetNextId(TableName.Tag.ToString()),
                Name = "Key",
                CellOnly = true,
                HasMultipleValues = false,
                Integrated = true,
                ProfileId = profileId
            };

            result = this.tagDao.CreateTag(tag);

            if (result.Success)
            {
                TrackProperty tp = new TrackProperty()
                {
                    Id = this.trackDao.GetNextId(TableName.TrackProperty.ToString()),
                    Name = tag.Name,
                    Type = "System.String",
                    IsEnabled = true,
                    ColumnGroup = ColumnGroup.TracklistColumns.ToString(),
                    SortingId = this.settingDao.GetNextTrackPropertySortingId(),
                };

                result = this.settingDao.CreateTrackProperty(tp);

            }

            if (result.Success)
            {
                TagValue tv = null;
                for (int i = 0; i <= keyNameArray.Count() - 1; i++)
                {
                    tv = new TagValue()
                    {
                        TagId = tag.Id,
                        TagName = tag.Name,
                        Id = this.tagDao.GetNextId(TableName.TagValue.ToString()),
                        Name = keyNameArray[i],
                        Color = this.HexToColor(keyColorArray[i]),
                        ProfileId = profileId
                    };

                    result = this.tagDao.CreateTagValue(tv);

                    if (!result.Success)
                    {
                        break;
                    }
                }
            }

            if (result.Success)
            {
                tag = new Tag()
                {
                    Id = this.tagDao.GetNextId(TableName.Tag.ToString()),
                    Name = "Bpm",
                    CellOnly = true,
                    HasMultipleValues = true,
                    Integrated = true,
                    ProfileId = profileId
                };

                result = this.tagDao.CreateTag(tag);
            }

            if (result.Success)
            {
                TrackProperty tp = new TrackProperty()
                {
                    Id = this.trackDao.GetNextId(TableName.TrackProperty.ToString()),
                    Name = tag.Name,
                    Type = "System.String",
                    IsEnabled = true,
                    ColumnGroup = ColumnGroup.TracklistColumns.ToString(),
                    SortingId = this.settingDao.GetNextTrackPropertySortingId()
                };

                result = this.settingDao.CreateTrackProperty(tp);
            }

            if (result.Success)
            {
                TagValue tv = new TagValue()
                {
                    TagId = tag.Id,
                    TagName = tag.Name,
                    Id = this.tagDao.GetNextId(TableName.TagValue.ToString()),
                    Name = "Bpm",
                    Color = Color.White,
                    ProfileId = profileId
                };

                result = this.tagDao.CreateTagValue(tv);
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

            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallStopTrackEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallStopTrackEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallStopTrackEvent();

            this.InitializeProfileAndPlaylist();
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
            this.HideAllForm();

            this.playlistPresenter.Initialize();
            ((PlaylistView)this.playlistView).Show();

            this.playlistPresenter.ReloadData(this.mediaPlayerComponent);

            this.actualView = this.playlistView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
        }
        
        private void ShowTagValueEditorView(object sender, EventArgs e)
        {
            this.HideAllForm();

            this.tagValueEditorPresenter.InitializeTagDataTable();
            ((TagValueView)this.tagValueView).Show();

            this.tagValueEditorPresenter.SelectFirstTagValue();

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
        private void ShowPreferencesView(object sender, EventArgs e)
        {
            PreferencesView preferencesView = new PreferencesView();
            PreferencesPresenter presenter = new PreferencesPresenter(preferencesView, this.trackDao, this.tagDao, this.profileDao, this.settingDao);
            preferencesView.ShowDialog((PreferencesView)this.preferencesView);

            bool automaticKeyImport = this.settingDao.GetBooleanSetting(Settings.AutomaticKeyImport.ToString()).Value;
            bool automaticBpmImport = this.settingDao.GetBooleanSetting(Settings.AutomaticBpmImport.ToString()).Value;

            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                if((automaticKeyImport == false && automaticBpmImport == false) ||(automaticKeyImport == true || automaticBpmImport == true))
                {
                    ((PlaylistView)this.playlistView).SetKeyAndBpmAnalization(this.HasVirtualDj() && (automaticKeyImport || automaticBpmImport));
                }
            }
                

            /*if (presenter.databaseCleared)
            {
                presenter.databaseCleared = false;
                this.ReloadMainView();
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

            List<Track> trackList = new List<Track>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Audio files (*.mp3,*.wav,*.flac)|*.mp3;*.wav;*.flac|Playlist files (*.m3u)|*.m3u";
            ofd.FilterIndex = this.settingDao.GetIntegerSetting(Settings.LastOpenFilesFilterIndex.ToString());
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
            List<Track> trackList = new List<Track>();

            using (var fbd = new FolderBrowserDialog())
            {
                String lastDirectoryPath = this.settingDao.GetStringSetting(Settings.LastOpenDirectoryPath.ToString());

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
            this.ShowPreferencesView(this, new EventArgs());
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
        private void ChangeProgress(object sender, ListEventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallChangeProgressEvent(e.IntegerField1, e.IntegerField2);
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallChangeProgressEvent(e.IntegerField1, e.IntegerField2);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallChangeProgressEvent(e.IntegerField1, e.IntegerField2);
        }
        private void ChangeVolume(object sender, ListEventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallChangeVolumeEvent(e.IntegerField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallChangeVolumeEvent(e.IntegerField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallChangeVolumeEvent(e.IntegerField1);
        }
        private void ChangeMute(object sender, ListEventArgs e)
        {



            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallChangeMuteEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallChangeMuteEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallChangeMuteEvent(e.BooleanField1);
        }
        private void ChangeShuffle(object sender, ListEventArgs e)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallChangeShuffleEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.CallChangeShuffleEvent(e.BooleanField1);
            else if (this.actualView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.CallChangeShuffleEvent(e.BooleanField1);
        }

        //HELP
        private void About(object sender, EventArgs e)
        {
            this.ShowAboutView(this, new EventArgs());
        }

        //ADD FILES
        private void AddTracksToTrackList(List<Track> trackList, int dragIndex = -1)
        {
            if (this.actualView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.CallAddTrackToTrackListEvent(trackList, dragIndex);
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
        
        private List<Track> ReadFiles(string[] fileNames)
        {
            List<String> filePathList = new List<String>();
            List<Track> trackList = new List<Track>();
            if (fileNames != null && fileNames.Length > 0)
            {
                //PATH-OK ÖSSZEGYŰJTÉSE
                //kapunk egy csomó fájlnevet, ezek path-ok lesznek
                //végigmegyünk a fájlneveken, ami mp3,wav,flac, azt hozzáadjuk külön egy path listához
                //ami m3u, azt beolvassuk. Végigmegyünk az m3u fájl tartalmán és azokat a sorokat megvizgsáljuk, ahol a második karakter kettőspont, a harmadik pedig visszaperjel
                //ezek a sorok valamilyen útvonalat jelölnek (pl.: "D:\..."), ezért ezeket is hozzáadjuk a fenti path listához
                //tehát audio fájlok útvonalai, valamint m3u-ban levő audio fájlok útvonalait összegyűjtjük

                foreach (String path in fileNames)
                {
                    if (path.Contains(".mp3"))
                    {
                        filePathList.Add(path);
                    }
                    else if (path.Contains(".wav"))
                    {
                        filePathList.Add(path);
                    }
                    else if (path.Contains(".flac"))
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
                                firstThreeCharacter = line.Substring(0, 3).ToCharArray();

                                if (firstThreeCharacter[1] == ':' && firstThreeCharacter[2] == '\\')
                                {
                                    filePathList.Add(line);
                                }
                            }
                        }
                    }
                }

                List<Tag> tagList = tagDao.GetAllTag();

                //FÁJLOKBÓL TRACK
                //végigmegyünk a path list-en, csinálunk egy új Track objektumot, belerakjuk a path-t
                //levágjuk a path-ból az útvonal elejét és a kiterjesztést, ez lesz a fájlnév, ezt is a Track-be rakjuk
                //Q: mire használjuk ezt?

                foreach (string path in filePathList)
                {
                    string fileName = "";

                    Track track = new Track();
                    track.Path = path;

                    int extCharCount = path.EndsWith("flac") ? 5 : 4;
                    fileName = path.Substring(path.LastIndexOf(@"\") + 1);
                    fileName = fileName.Remove(fileName.LastIndexOf("."), extCharCount);

                    track.FileName = fileName;
                    track.Album = "";
                    track.Title = "";

                    //METAADATOK GYŰJTÉSE
                    //ha a path nem létező fájlra mutat, akkor a Track-ben true-ra rakjuk az IsMissing flag-et
                    //ha létező fájlra mutat, akkor megnézzük, hogy szerepel-e már az adatbázisunkban
                    //ha szerepel, akkor csak betöltjük
                    //ha nem szerepel, akkor a fájlból megsejtjük a különböző metaadatokat

                    if (!System.IO.File.Exists(path))
                    {
                        track.Artist = fileName;
                        track.IsMissing = true;
                    }
                    else
                    {
                        //ha a fájlt korábban már beolvastuk, akkor létezik az adatbázisban
                        //ebben az esetben a metaadatokat onnan szedjük

                        Track trackFromDb = this.trackDao.GetTrackByPath(track.Path, tagList);
                        if (trackFromDb != null)
                        {
                            track = trackFromDb;
                            track.IsNew = false;
                        }
                        else
                        {
                            //ahol az artist nem sikerül beolvasni, ott az artist a fájlnév lesz
                            //mp3 esetén TagLib-bel olvassuk be a metaadatokat (artist, title, album, year, length)
                            //wav esetén csak a számhosszt olvassuk be a WaveFileReader-el
                            //TODO: flac-nek jó lenne számhossz

                           

                            if (track.Path.Contains(".mp3"))
                            {
                                TagLib.File file = TagLib.File.Create(track.Path);
                                if (!String.IsNullOrEmpty(file.Tag.FirstArtist))
                                    track.Artist = file.Tag.FirstArtist;
                                else if (!String.IsNullOrEmpty(file.Tag.FirstAlbumArtist))
                                    track.Artist = file.Tag.FirstAlbumArtist;
                                else if (file.Tag.Artists != null && file.Tag.Artists.Count() > 0)
                                    track.Artist = file.Tag.Artists[0];
                                track.Album = file.Tag.Album;
                                track.Title = file.Tag.Title;
                                track.Year = (int)file.Tag.Year;
                                track.Length = file.Properties.Duration.TotalSeconds;
                                file.Dispose();

                                if (String.IsNullOrEmpty(track.Artist))
                                    track.Artist = fileName;
                            }
                            else if (path.Contains(".wav"))
                            {
                                WaveFileReader wf = new WaveFileReader(path);
                                track.Artist = fileName;
                                track.Length = wf.TotalTime.TotalSeconds;
                            }
                            else if (path.Contains(".flac"))
                            {
                                using (FlacFile file = new FlacFile(path))
                                {
                                    
                                    var vorbisComment = file.VorbisComment;
                                    if (vorbisComment != null)
                                    {
                                        foreach (var value in vorbisComment.Artist)
                                        {
                                            if (value != null)
                                            {
                                                track.Artist = value;
                                                break;
                                            }
                                        }
                                        foreach (var value in vorbisComment.Album)
                                        {
                                            if (value != null)
                                            {
                                                track.Album = value;
                                                break;
                                            }
                                        }
                                        foreach (var value in vorbisComment.Title)
                                        {
                                            if (value != null)
                                            {
                                                track.Title = value;
                                                break;
                                            }
                                        }
                                        foreach (var value in vorbisComment.Date)
                                        {
                                            if (value != null)
                                            {
                                                String date = ""; 
                                                if (value.Length > 4)
                                                {
                                                    date = value.Substring(0, 4);
                                                    track.Year = Int32.Parse(date);
                                                }
                                                else
                                                {
                                                    track.Year = Int32.Parse(value);
                                                }
                                               
                                                break;
                                            }
                                        }
                                    }
                                    track.Length = file.StreamInfo.Duration;
                                }
                                if (String.IsNullOrEmpty(track.Artist))
                                {
                                    track.Artist = fileName;
                                }
                               
                            }
                            track.IsNew = true;

                            track.Id = this.trackDao.GetNextId(TableName.Track.ToString());
                            this.trackDao.CreateTrack(track);

                            track.TrackTagValues = new List<TrackTagValue>();

                            foreach (Tag tag in tagList)
                            {
                                TrackTagValue ttv = new TrackTagValue();
                                ttv.Id = this.trackDao.GetNextId(TableName.TrackTagValue.ToString());
                                ttv.TrackId = track.Id;
                                ttv.TagId = tag.Id;
                                ttv.TagName = tag.Name;
                                ttv.TagValueId = -1;
                                ttv.TagValueName = String.Empty;
                                ttv.Value = String.Empty;
                                ttv.HasValue = false;
                                ttv.ProfileId = this.trackDao.GetProfileId();
                                this.trackDao.CreateTrackTagValue(ttv);
                                track.TrackTagValues.Add(ttv);
                            }

                           
                        }



                    }

                    trackList.Add(track);
                }

                if (this.HasVirtualDj())
                {

                    bool automaticKeyImport = this.settingDao.GetBooleanSetting(Settings.AutomaticKeyImport.ToString()).Value;
                    bool automaticBpmImport = this.settingDao.GetBooleanSetting(Settings.AutomaticBpmImport.ToString()).Value;

                    List<TagValue> keyTagValueList = new List<TagValue>();
                    List<TagValue> bpmTagValueList = new List<TagValue>();

                    if (automaticKeyImport)
                    {
                        Tag tag = tagList.Find(x => x.Name == "Key");
                        if (tag != null)
                        {
                            keyTagValueList = this.tagDao.GetTagValuesByTagId(tag.Id);
                        }
                    }
                    if (automaticBpmImport)
                    {
                        Tag tag = tagList.Find(x => x.Name == "Bpm");
                        if (tag != null)
                        {
                            bpmTagValueList = this.tagDao.GetTagValuesByTagId(tag.Id);
                        }
                    }

                    ResultOrError result = new ResultOrError();
                    result = VirtualDJReader.Instance.ReadKeyAndBpmFromVirtualDJDatabase(ref trackList, this.trackDao, keyTagValueList, bpmTagValueList);
                    if (!result.Success)
                    {
                        MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return trackList;
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
        private List<Track> trackList = new List<Track>();
        private void ScanFiles(object sender, ListEventArgs e)
        {
            string[] mediaFiles;
            string[] directories;
            trackList = new List<Track>();
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

  

    }
}
