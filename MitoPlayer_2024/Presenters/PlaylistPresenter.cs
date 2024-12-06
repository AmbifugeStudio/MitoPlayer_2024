using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using NAudio.Wave;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using Type = System.Type;
using NAudio.SoundFont;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Collections;
using System.Threading;
using FlacLibSharp;

namespace MitoPlayer_2024.Presenters
{

    public class PlaylistPresenter
    {
        private IPlaylistView playlistView { get; set; }
        private ITrackDao trackDao { get; set; }
        private ITagDao tagDao { get; set; }
        private ISettingDao settingDao { get; set; }

        private bool isInitializing = true;

        public PlaylistPresenter(IPlaylistView view, ITrackDao trackDao, ITagDao tagValueDao, ISettingDao settingDao)
        {
            //INITIALIZE
            this.playlistView = view;
            this.trackDao = trackDao;
            this.tagDao = tagValueDao;
            this.settingDao = settingDao;

            //MEDIA PLAYER
            this.playlistView.SetCurrentTrackEvent += SetCurrentTrackEvent;
            this.playlistView.PlayTrackEvent += PlayTrackEvent;
            this.playlistView.PauseTrackEvent += PauseTrackEvent;
            this.playlistView.StopTrackEvent += StopTrackEvent;
            this.playlistView.PrevTrackEvent += PrevTrackEvent;
            this.playlistView.NextTrackEvent += NextTrackEvent;
            this.playlistView.RandomTrackEvent += RandomTrackEvent;

            //TRACKLIST
            this.playlistView.OrderByColumnEvent += OrderByColumnEvent;
            this.playlistView.DeleteTracksEvent += DeleteTracksEvent;

            this.playlistView.MoveTracklistRowsEvent += MoveTracklistRowsEvent;
            this.playlistView.InternalDragAndDropIntoTracklistEvent += InternalDragAndDropIntoTracklistEvent;
            this.playlistView.InternalDragAndDropIntoPlaylistEvent += InternalDragAndDropIntoPlaylistEvent;
            this.playlistView.ExternalDragAndDropIntoTracklistEvent += ExternalDragAndDropIntoTracklistEvent;
            this.playlistView.ExternalDragAndDropIntoPlaylistEvent += ExternalDragAndDropIntoPlaylistEvent;
            
            this.playlistView.ShowColumnVisibilityEditorEvent += ShowColumnVisibilityEditorEvent;
            this.playlistView.ScanKeyAndBpmEvent += ScanKeyAndBpmEvent;
            this.playlistView.ExportToDirectoryEvent += ExportToDirectoryEvent;

            //PLAYLIST
            this.playlistView.CreatePlaylist += CreatePlaylist;
            this.playlistView.EditPlaylist += EditPlaylist;
            this.playlistView.LoadPlaylistEvent += LoadPlaylistEvent;
            this.playlistView.MovePlaylistEvent += MovePlaylistEvent;
            this.playlistView.DeletePlaylistEvent += DeletePlaylistEvent;
           // this.playlistView.SetQuickListEvent += SetQuickListEvent;
            this.playlistView.ExportToM3UEvent += ExportToM3UEvent;
            this.playlistView.ExportToTXTEvent += ExportToTXTEvent;
            this.playlistView.DisplayPlaylistListEvent += DisplayPlaylistListEvent;
            this.playlistView.MovePlaylistRowEvent += MovePlaylistRowEvent;

            //TAG EDITOR
            this.playlistView.DisplayTagEditorEvent += DisplayTagComponentEvent;
            this.playlistView.SetTagValueEvent += SetTagValueEvent;
            this.playlistView.ClearTagValueEvent += ClearTagValueEvent;

            this.playlistView.EnableFilterModeEvent += EnableFilterModeEvent;
            this.playlistView.EnableSetterModeEvent += EnableSetterModeEvent;
           
            this.playlistView.ChangeOnlyPlayingRowModeEnabled += ChangeOnlyPlayingRowModeEnabled;
            this.playlistView.ChangeFilterParametersEvent += ChangeFilterParameters;
            this.playlistView.RemoveTagValueFilter += RemoveTagValueFilter;

            this.playlistView.SaveTrackListEvent += SaveTrackListEvent;

            this.playlistView.TrainKeyDetectorEvent += TrainKeyDetectorEvent;
            this.playlistView.DetectKeyEvent += DetectKeyEvent;
            this.playlistView.AddToKeyDetectorEvent += AddToKeyDetectorEvent;
            this.playlistView.CreateModelEvent += CreateModelEvent;

            this.playlistView.OpenModelTrainerEvent += OpenModelTrainerEvent;

            this.playlistView.LiveStreamAnimationEvent += LiveStreamAnimationEvent;
            this.playlistView.LiveStreamAnimationSettingEvent += LiveStreamAnimationSettingEvent;

            this.playlistView.DisplayCoverImageComponentEvent += DisplayCoverImageComponentEvent;
            this.playlistView.JumpBackwardEvent += JumpBackwardEvent;
            this.playlistView.JumpForwardEvent += JumpForwardEvent;
            this.playlistView.CopyCurrentPlayingTrackToDefaultPlaylistEvent += CopyCurrentPlayingTrackToDefaultPlaylistEvent;


            //COVER BROWSER
            this.playlistView.LoadCoversEvent += LoadCoversEvent;
        }

        private void CopyCurrentPlayingTrackToDefaultPlaylistEvent(object sender, EventArgs e)
        {
            if(this.mediaPlayerComponent!= null)
            {
                int index = this.mediaPlayerComponent.GetCurrentTrackIndex();
                int trackId = Convert.ToInt32(this.trackListTable.Rows[index]["Id"]);

                List<Track> sourceTrackList = new List<Track>();
                Model.Track track = this.trackDao.GetTrackWithTags(trackId, this.tagList);
                if(track != null)
                {
                    sourceTrackList.Add(track);
                    this.AddTracksToPlaylist(0, sourceTrackList);
                    ((PlaylistView)this.playlistView).UpdateAfterCopyTracksToPlaylist(sourceTrackList.Count(), "Default Playlist");
                }
            }
        }

        private void JumpForwardEvent(object sender, EventArgs e)
        {
            if(this.mediaPlayerComponent!= null && this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.mediaPlayerComponent.JumpForward();
            }
        }

        private void JumpBackwardEvent(object sender, EventArgs e)
        {
            if (this.mediaPlayerComponent != null && this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.mediaPlayerComponent.JumpBackward();
            }
        }

        private LiveStreamAnimationView liveStreamAnimationView { get; set; }
        private LiveStreamAnimationPresenter liveStreamAnimationPresenter { get; set; }
        private void LiveStreamAnimationEvent(object sender, EventArgs e)
        {
            if (this.liveStreamAnimationView == null || this.liveStreamAnimationView.IsDisposed)
            {
                this.liveStreamAnimationView = new LiveStreamAnimationView();
                this.liveStreamAnimationPresenter = new LiveStreamAnimationPresenter(liveStreamAnimationView, this.mediaPlayerComponent, this.tagDao, this.trackDao, this.settingDao);
                if (!this.liveStreamAnimationPresenter.ViewIsReadyToShow)
                {
                    this.liveStreamAnimationView.Dispose();
                    this.liveStreamAnimationView = null;
                }
            }
            else
            {
                bool isFormOpen = Application.OpenForms.OfType<LiveStreamAnimationView>().Any();
                if (!isFormOpen)
                {
                    this.liveStreamAnimationView.BringToFront();
                }
            }
        }
        private LiveStreamAnimationSettingView liveStreamAnimationSettingView { get; set; }
        private LiveStreamAnimationSettingPresenter liveStreamAnimationSettingPresenter { get; set; }
        private void LiveStreamAnimationSettingEvent(object sender, EventArgs e)
        {
            LiveStreamAnimationSettingView liveStreamAnimationSettingView = new LiveStreamAnimationSettingView();
            LiveStreamAnimationSettingPresenter liveStreamAnimationSettingPresenter = new LiveStreamAnimationSettingPresenter(liveStreamAnimationSettingView, this.settingDao);

            if (liveStreamAnimationSettingView.ShowDialog((LiveStreamAnimationSettingView)this.liveStreamAnimationSettingView) == DialogResult.OK)
            {

            }
        }

        private void OpenModelTrainerEvent(object sender, EventArgs e)
        {
            ModelTrainerView modelTrainerView = new ModelTrainerView();
            ModelTrainerPresenter presenter = new ModelTrainerPresenter(modelTrainerView, this.tagDao, this.trackDao, this.settingDao);

            if (modelTrainerView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
            {

            }
        }






        #region INITIALIZE

        public void Initialize(MediaPlayerComponent mediaPlayer)
        {
            isInitializing = true;
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
               
                try
                {
                    loadingDialog.SetProcessDescription("Initialize tags and tag values...");
                    loadingDialog.Refresh();

                    this.InitializeTagsAndTagValues();

                    loadingDialog.SetProcessDescription("Initialize playlist grid structure...");
                    loadingDialog.Refresh();

                    this.InitializePlaylistListColumns();
                    this.InitializePlaylistListRows();
                    this.InitializePlaylistList();

                    this.InitializeShortTrackColouring();

                    loadingDialog.SetProcessDescription("Initialize tracklist grid structure...");
                    loadingDialog.Refresh();

                    this.InitializeTracklistColumns();
                    this.InitializeTrackListRows(this.trackListTable, null);
                    this.InitializeTrackList(this.trackListTable);

                    loadingDialog.SetProcessDescription("Initialize tag editor and filter component...");
                    loadingDialog.Refresh();

                    this.InitializeTagComponent();

                    loadingDialog.SetProcessDescription("Initialize media player...");
                    loadingDialog.Refresh();

                    if (this.mediaPlayerComponent == null)
                    {
                        this.mediaPlayerComponent = mediaPlayer;
                        this.mediaPlayerComponent.Initialize(this.trackListTable);
                        this.mediaPlayerComponent.MediaPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(MediaPlayer_PlayStateChange);
                    }

                    loadingDialog.SetProcessDescription("Initialize view settings...");
                    loadingDialog.Refresh();

                    this.InitializedPlaylistList();

                    this.InitializeVolume();

                    this.InitializePostKeyAndBpmAnalization();

                    this.InitializedCoverImageComponent();

                    this.InitializeTimers();
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            isInitializing = false;
        }

        private void InitializeShortTrackColouring()
        {
            bool isColouringEnabled = this.settingDao.GetBooleanSetting(Settings.IsShortTrackColouringEnabled.ToString()).Value;
            TimeSpan timeSpan = new TimeSpan();

            if (isColouringEnabled)
            {
                decimal shortTrackColouringThreshold = this.settingDao.GetDecimalSetting(Settings.ShortTrackColouringThreshold.ToString());
                timeSpan = TimeSpan.FromMinutes((double)shortTrackColouringThreshold);
            }

            ((PlaylistView)this.playlistView).InitializeShortTrackColouring(isColouringEnabled, timeSpan);
        }

        private System.Windows.Forms.Timer playNextTrackTimer;


        private void InitializeTimers()
        {
            playNextTrackTimer = new System.Windows.Forms.Timer();
            playNextTrackTimer.Interval = 100; // 100 milliseconds delay
            playNextTrackTimer.Tick += PlayNextTrackTimer_Tick;
        }
       


        private List<Tag> tagList { get; set; }
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        private void InitializeTagsAndTagValues()
        {
            this.tagValueDictionary = new Dictionary<String, Dictionary<String, Color>>();

            List<Tag> tagList = this.tagDao.GetAllTag();
            if (tagList != null && tagList.Count > 0)
            {
                this.tagList = tagList;

                List<TagValue> tagValueList = new List<TagValue>();

                foreach (Tag tag in this.tagList)
                {
                    Dictionary<String,Color> tvDic = new Dictionary<String, Color>();

                    tagValueList = this.tagDao.GetTagValuesByTagId(tag.Id);
                    if (tagValueList != null && tagValueList.Count > 0)
                    {
                        foreach (TagValue tv in tagValueList)
                        {
                            tvDic.Add(tv.Name, tv.Color);
                        }
                        this.tagValueDictionary.Add(tag.Name, tvDic);
                    }
                }
            }
            this.playlistView.InitializeTagsAndTagValues(this.tagList, this.tagValueDictionary);
        }
        private BindingSource playlistListBindingSource { get; set; }
        private DataTable playlistListTable { get; set; }
        private bool[] playlistListColumnVisibilityArray { get; set; }
        private int currentPlaylistId { get; set; }

        ///PLAYLIST GRID
        ///lekérjük az oszlopokat (trackproperty)
        ///hozzáadjuk a playlist datatable-höz az oszlopokat
        ///lekérjük az oszlopok láthatóságát egy bool tömbbe
        ///lekérjük a current playlist id-t a setting-ből
        ///megnézzük, hogy a jelenlegi listában van-e a current playlist id
        ///bindoljuk a playlist-et, átadjuk a láthatósági tömbböt és az akutális playlist-et
        private void InitializePlaylistListColumns()
        {
            this.currentPlaylistId = 0;
            this.playlistListBindingSource = new BindingSource();
            this.playlistListTable = new DataTable();

            List<TrackProperty> tpList = new List<TrackProperty>();
            tpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.PlaylistColumns.ToString());
            if (tpList != null && tpList.Count > 0)
            {
                foreach (TrackProperty tp in tpList)
                {
                    this.playlistListTable.Columns.Add(tp.Name, Type.GetType(tp.Type));
                }
                this.playlistListColumnVisibilityArray = tpList.Select(x => x.IsEnabled).ToArray();
            }
        }
        private void InitializePlaylistListRows()
        {
            this.playlistListTable.Clear();

            List<Playlist> playlistList = this.trackDao.GetAllPlaylist();
            if (playlistList != null && playlistList.Count > 0)
            {
                foreach(Playlist playlist in playlistList)
                {
                    DataRow dataRow = this.playlistListTable.NewRow();
                    dataRow["Id"] = playlist.Id;
                    dataRow["Name"] = playlist.Name;
                    dataRow["OrderInList"] = playlist.OrderInList;
                    dataRow["ProfileId"] = playlist.ProfileId;
                    dataRow["IsActive"] = playlist.IsActive;
                    dataRow["IsModelTrainer"] = playlist.IsModelTrainer;

                    this.playlistListTable.Rows.Add(dataRow);
                }
            }
        }
        private void InitializePlaylistList()
        {
            this.playlistListBindingSource.DataSource = this.playlistListTable;
            this.currentPlaylistId = this.settingDao.GetIntegerSetting(Settings.CurrentPlaylistId.ToString());

            DataTableModel model = new DataTableModel()
            {
                BindingSource = this.playlistListBindingSource,
                ColumnVisibilityArray = this.playlistListColumnVisibilityArray,
                CurrentPlaylistId = this.currentPlaylistId
            };

            this.playlistView.InitializePlaylistList(model);
        }
        private void ReloadPlaylist()
        {
            this.InitializePlaylistList();
        }


        private BindingSource trackListBindingSource { get; set; }
        private DataTable trackListTable { get; set; }
        private List<Model.Track> tracklist { get; set; }
        private DataTable filteredTrackListTable { get; set; }
        private Dictionary<string, int> columnOrderStates { get; set; }
        private int[] trackColumnDisplayIndexArray { get; set; }
        private bool[] trackColumnVisibilityArray { get; set; }

        /// TRACKLIST GRID
        /// lekérjük az oszlopokat (trackproperty)
        /// lekérjük a sortingid-t egy int tömbbe
        /// lekérjük az oszlop sorrendet egy string-int dictionary-be
        /// hozzáadjuk a tracklist ddatatable-höz az oszlopokat
        /// lekérjük az oszlopok láthatóságát egy bool tömbbe
        /// lekérjük a current track id in playlist-t a setting-ből
        /// megnézzük, hogy a jelenlegi listában van-e a current track id in playlist
        /// bindoljuk a tracklist-et, átadjuk a láthatósági tömbböt, a sorrend dictionary-t és az current track id in playlist-et
        private void InitializeTracklistColumns()
        {
            this.trackListBindingSource = new BindingSource();
            this.trackListTable = new DataTable();
            this.filteredTrackListTable = new DataTable();
            this.columnOrderStates = new Dictionary<string, int>();

            List<TrackProperty> tpList = new List<TrackProperty>();
            tpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString());
            if (tpList != null && tpList.Count > 0)
            {
                this.trackColumnDisplayIndexArray = tpList.Select(x => x.SortingId).ToArray();

                tpList = tpList.OrderBy(x => x.SortingId).ToList();
                for (int i = 0; i <= tpList.Count - 1; i++)
                {
                    this.columnOrderStates.Add(tpList[i].Name, -1);
                    this.trackListTable.Columns.Add(tpList[i].Name, Type.GetType(tpList[i].Type));
                    this.filteredTrackListTable.Columns.Add(tpList[i].Name, Type.GetType(tpList[i].Type));
                }

                this.trackColumnVisibilityArray = tpList.Select(x => x.IsEnabled).ToArray();
            }
        }
        private void InitializeTrackListRows(DataTable tracklistTable, List<Model.Track> trackList)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
                loadingDialog.SetProcessDescription("Reload tracklist...");
                loadingDialog.Refresh();


                tracklistTable.Clear();

                if (trackList == null)
                {
                    List<Playlist> plsList = this.trackDao.GetAllPlaylist();
                    Playlist actualPlaylist = null;
                    if (plsList != null && plsList.Count > 0)
                    {
                        actualPlaylist = plsList.Find(x => x.Id == this.currentPlaylistId);
                    }
                    trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(actualPlaylist.Id, this.tagList);
                    this.tracklist = tracklist;
                }

                if (trackList != null && trackList.Count > 0)
                {
                    try
                    {
                        foreach (Model.Track track in trackList)
                        {
                            DataRow dataRow = tracklistTable.NewRow();
                            dataRow["Id"] = track.Id;
                            dataRow["Album"] = track.Album;
                            dataRow["Artist"] = track.Artist;
                            dataRow["Title"] = track.Title;
                            dataRow["Year"] = track.Year.ToString();
                            dataRow["Length"] = this.LengthToString(track.Length);
                            dataRow["IsMissing"] = track.IsMissing;
                            dataRow["Path"] = track.Path;
                            dataRow["FileName"] = track.FileName;
                            dataRow["OrderInList"] = track.OrderInList;
                            dataRow["TrackIdInPlaylist"] = track.TrackIdInPlaylist;

                            if (track.TrackTagValues != null)
                            {
                                foreach (TrackTagValue ttv in track.TrackTagValues)
                                {
                                    if (ttv.HasMultipleValues)
                                    {
                                        dataRow[ttv.TagName] = ttv.Value;
                                        dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                                    }
                                    else
                                    {
                                        dataRow[ttv.TagName] = ttv.TagValueName;
                                        dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                                    }
                                }
                            }

                            tracklistTable.Rows.Add(dataRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
            }
        }

        private void InitializeTrackList(DataTable trackListTable, int currentTrackIdInPlaylist = -1)
        {
            if(!isInitializing)
                ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);

            this.trackListBindingSource.DataSource = trackListTable;

            DataTableModel model = new DataTableModel()
            {
                BindingSource = this.trackListBindingSource,
                ColumnVisibilityArray = this.trackColumnVisibilityArray,
                ColumnDisplayIndexArray = this.trackColumnDisplayIndexArray,
                CurrentTrackIdInPlaylist = currentTrackIdInPlaylist
            };

            this.playlistView.InitializeTrackList(model);

            if (!isInitializing)
                ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);

            ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);

        }

        private MediaPlayerComponent mediaPlayerComponent { get; set; }
        private void ReloadTrackList()
        {
            int currentTrackIdInPlaylist = -1;

            if (this.mediaPlayerComponent != null)
            {
                if (this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying ||
                   this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
                {
                    currentTrackIdInPlaylist = this.mediaPlayerComponent.CurrentTrackIdInPlaylist;

                    ///elindítunk egy számot, betöltünk egy másik listát, majd visszatöltjük az eredetit, akkor meg kellene jelölni a számot
                    int trackIdInPlaylist = -1;
                    for (int i = 0; i <= this.trackListTable.Rows.Count - 1; i++)
                    {
                        trackIdInPlaylist = Convert.ToInt32(this.trackListTable.Rows[i]["TrackIdInPlaylist"]);
                        if (trackIdInPlaylist == currentTrackIdInPlaylist)
                        {
                            currentTrackIdInPlaylist = this.mediaPlayerComponent.CurrentTrackIdInPlaylist;
                            break;
                        }
                    }
                }
            }


            if (this.isFilterModeEnabled)
            {
                this.InitializeTrackList(this.filteredTrackListTable, currentTrackIdInPlaylist);
            }
            else
            {
                this.InitializeTrackList(this.trackListTable, currentTrackIdInPlaylist);
            }
        }

      /* private void ReloadTrackListDataGridView(DataTable trackListTable)
        {
            int currentTrackIdInPlaylist = -1;

            if (this.mediaPLayerComponent != null)
            {
                if (this.mediaPLayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying ||
                   this.mediaPLayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
                {
                    currentTrackIdInPlaylist = this.mediaPLayerComponent.CurrentTrackIdInPlaylist;

                    ///elindítunk egy számot, betöltünk egy másik listát, majd visszatöltjük az eredetit, akkor meg kellene jelölni a számot
                    int trackIdInPlaylist = -1;
                    for (int i = 0; i <= this.trackListTable.Rows.Count - 1; i++)
                    {
                        trackIdInPlaylist = Convert.ToInt32(trackListTable.Rows[i]["TrackIdInPlaylist"]);
                        if (trackIdInPlaylist == currentTrackIdInPlaylist)
                        {
                            currentTrackIdInPlaylist = this.mediaPLayerComponent.CurrentTrackIdInPlaylist;
                            break;
                        }
                    }
                }
            }

            this.trackListBindingSource.DataSource = trackListTable;
            this.playlistView.ReloadTrackListBindingSource(this.trackListBindingSource, this.TrackColumnVisibilityArray, this.TrackColumnDisplayIndexArray, currentTrackIdInPlaylist);
        }*/


        private bool isTagComponentDisplayed { get; set; }
        private void InitializeTagComponent()
        {
            this.isTagComponentDisplayed = this.settingDao.GetBooleanSetting(Settings.IsTagEditorComponentDisplayed.ToString()).Value;
            this.isOnlyPlayingRowModeEnabled = this.settingDao.GetBooleanSetting(Settings.IsOnlyPlayingRowModeEnabled.ToString()).Value;
            //this.isFilterModeEnabled = this.settingDao.GetBooleanSetting(Settings.IsFilterModeEnabled.ToString()).Value;

            List<List<TagValue>> tagValueListContainer = new List<List<TagValue>>();
            foreach (Tag tag in this.tagList)
            {
                List<TagValue> tagValues = new List<TagValue>();
                tagValues = this.tagDao.GetTagValuesByTagId(tag.Id);
                tagValueListContainer.Add(tagValues);
            }

            ((PlaylistView)this.playlistView).InitializeTagComponent(
                this.tagList, 
                tagValueListContainer, 
                this.isTagComponentDisplayed, 
                this.isOnlyPlayingRowModeEnabled,
                this.isFilterModeEnabled);

            

            ((PlaylistView)this.playlistView).InitializeDisplayTagComponent(this.isTagComponentDisplayed);
        }
        private bool isPlaylistListDisplayed { get; set; }
        private void InitializedPlaylistList()
        {
            this.isPlaylistListDisplayed = this.settingDao.GetBooleanSetting(Settings.IsPlaylistListDisplayed.ToString()).Value;
            ((PlaylistView)this.playlistView).InitializeDisplayPlaylistList(this.isPlaylistListDisplayed);

            //((PlaylistView)this.playlistView).ResetPlaylistList(this.isPlaylistListDisplayed);
            //((PlaylistView)this.playlistView).CallDisplayPlaylistList(this.isPlaylistListDisplayed);
        }
        private bool isCoverImageComponentDisplayed { get; set; }
        private bool firstInitializeOfCoverImageComponent { get; set; }
        private void InitializedCoverImageComponent()
        {
            this.isCoverImageComponentDisplayed = this.settingDao.GetBooleanSetting(Settings.IsCoverImageComponentDisplayed.ToString()).Value;
            ((PlaylistView)this.playlistView).InitializeCoverImageComponent(this.isCoverImageComponentDisplayed);
            if (this.isCoverImageComponentDisplayed)
            {
                if (this.trackListTable != null && this.trackListTable.Rows.Count > 0)
                {
                    this.UpdateCoverBrowser(0);
                    this.firstInitializeOfCoverImageComponent = true;
                }
            }
            else
            {
                this.firstInitializeOfCoverImageComponent = false;
            }
        }
        private void DisplayCoverImageComponentEvent(object sender, EventArgs e)
        {
            this.isCoverImageComponentDisplayed = !this.isCoverImageComponentDisplayed;
            this.settingDao.SetBooleanSetting(Settings.IsCoverImageComponentDisplayed.ToString(), this.isCoverImageComponentDisplayed);
            ((PlaylistView)this.playlistView).UpdateDisplayCoverImageComponent(this.isCoverImageComponentDisplayed);

            if(this.isCoverImageComponentDisplayed && !this.firstInitializeOfCoverImageComponent)
            {
                if (this.trackListTable != null && this.trackListTable.Rows.Count > 0)
                {
                    this.firstInitializeOfCoverImageComponent = true;
                }
            }

        }
        #endregion

        #region INITIALIZED - RELOAD

        private String LengthToString(double length)
        {
            TimeSpan t = TimeSpan.FromSeconds(length);
            String lengthInString = "";

            if (t.Hours > 0)
            {
                lengthInString = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
            }
            else
            {
                if (t.Minutes > 0)
                {
                    lengthInString = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
                }
                else
                {
                    lengthInString = string.Format("{0:D2}:{1:D2}", 0, t.Seconds);
                }

            }
            return lengthInString;
        }

        private void InitializeVolume()
        {
            int volume = this.settingDao.GetIntegerSetting(Settings.Volume.ToString());
            bool isMuted = this.settingDao.GetBooleanSetting(Settings.IsMuteEnabled.ToString()).Value;

            if (volume == -1)
                volume = 50;

            if (isMuted)
                volume = 0;

            this.playlistView.SetVolume(volume);
            this.playlistView.SetMuted(isMuted);

            this.mediaPlayerComponent.MediaPlayer.settings.volume = volume;
        }

        private void InitializePostKeyAndBpmAnalization()
        {
            bool automaticKeyImport = this.settingDao.GetBooleanSetting(Settings.AutomaticKeyImport.ToString()).Value;
            bool automaticBpmImport = this.settingDao.GetBooleanSetting(Settings.AutomaticBpmImport.ToString()).Value;
            this.playlistView.SetKeyAndBpmAnalization(this.HasVirtualDj() && (automaticKeyImport || automaticBpmImport));
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

        

        #endregion





       /* private ResultOrError InitializeColoringByTagValues()
        {
            ResultOrError result = new ResultOrError();
            try
            {
                int currentTagId = this.settingDao.GetIntegerSetting(Settings.CurrentTagIndexForTracklistColouring.ToString());
                ((PlaylistView)this.playlistView).InitializeCurrentTagValueColors(this.tagList, currentTagId);
                //this.ChangeColoringByTagValues("Key");
                //result = this.SetTrackList(this.trackListTable);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }*/
      /*  private void ChangeTracklistColorEvent(object sender, ListEventArgs e)
        {
            this.ChangeColoringByTagValues(e.StringField1);
        }*/
       /* private void ChangeColoringByTagValues(String tagName)
        {

            Dictionary<String, Color> tagValueColor = null;
            List<TagValue> tagValueList = null;
            Tag tag = null;

            if (this.tagList != null && this.tagList.Count > 0)
            {
                tag = this.tagList.Find(x => x.Name == tagName);
                if (tag != null)
                {
                    this.settingDao.SetIntegerSetting(Settings.CurrentTagIndexForTracklistColouring.ToString(), tag.Id);
                    tagValueList = this.tagDao.GetTagValuesByTagId(tag.Id);
                    if (tagValueList != null && tagValueList.Count > 0)
                    {
                        tagValueColor = new Dictionary<String, Color>();
                        foreach (TagValue tv in tagValueList)
                        {
                            tagValueColor.Add(tv.Name, tv.Color);
                        }
                    }
                }
            }
            ((PlaylistView)this.playlistView).ChangeCurrentTagValueColors(tag, tagValueColor);
            this.ReloadTrackListDataGridView(this.trackListTable);
        }*/
       


        #region DATATABLE SAVES


        private async void SavePlaylistList()
        {
            if (isSaving)
            {
                MessageBox.Show("Save is in progress, please wait!", "Tracklist Save", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    await SavePlaylistListAsync();
                }
                finally
                {
                    isSaving = false;
                    this.playlistView.ChangeSaveStatus(false);
                }
            }
        }
        public async Task SavePlaylistListAsync()
        {
            await Task.Run(() =>
            {
                String errorMessage = String.Empty;
                try
                {
                    List<Playlist> playlistlist = this.ConvertPlaylistDataTableToList(this.playlistListTable);
                    int orderInList = 0;
                    foreach (Playlist playlist in playlistlist)
                    {
                        playlist.OrderInList = orderInList;
                        this.trackDao.UpdatePlaylist(playlist);
                        orderInList++;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
                
               // ((PlaylistView)this.playlistView).UpdateAfterTracklistSave(errorMessage);
            });
        }
        private List<Playlist> ConvertPlaylistDataTableToList(DataTable dt)
        {
            List<Playlist> playlistList = new List<Playlist>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Playlist playlist = new Playlist();
                playlist.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                playlist.Name = dt.Rows[i]["Name"].ToString();
                playlist.OrderInList = Convert.ToInt32(dt.Rows[i]["OrderInList"]);
                playlist.ProfileId = Convert.ToInt32(dt.Rows[i]["ProfileId"]);
                playlist.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"]);
                playlist.IsModelTrainer = Convert.ToBoolean(dt.Rows[i]["IsModelTrainer"]);
                playlistList.Add(playlist);
            }

            return playlistList;
        }

        public async Task SaveTrackListAsync()
        {
            String errorMessage = String.Empty;

            await Task.Run(() =>
            {
                try
                {
                   this.trackDao.DeletePlaylistContentByPlaylistId(this.currentPlaylistId);
                    List<Model.Track> tracklist = this.ConvertTrackDataTableToList(this.trackListTable);
                    int orderInList = 0;
                    foreach (Model.Track track in tracklist)
                    {
                        track.OrderInList = orderInList;

                        PlaylistContent plc = new PlaylistContent();
                        plc.Id = this.trackDao.GetNextId(TableName.PlaylistContent.ToString());
                        plc.PlaylistId = this.currentPlaylistId;
                        plc.TrackId = track.Id;
                        plc.OrderInList = track.OrderInList;
                        plc.TrackIdInPlaylist = track.TrackIdInPlaylist;
                        this.trackDao.CreatePlaylistContent(plc);

                        orderInList++;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            });
            ((PlaylistView)this.playlistView).UpdateAfterTracklistSave(errorMessage);
        }
        public void SaveTrackListSync()
        {
            String errorMessage = String.Empty;
            try
            {
                this.trackDao.DeletePlaylistContentByPlaylistId(this.currentPlaylistId);
                List<Model.Track> tracklist = this.ConvertTrackDataTableToList(this.trackListTable);
                int orderInList = 0;
                foreach (Model.Track track in tracklist)
                {
                    track.OrderInList = orderInList;

                    PlaylistContent plc = new PlaylistContent();
                    plc.Id = this.trackDao.GetNextId(TableName.PlaylistContent.ToString());
                    plc.PlaylistId = this.currentPlaylistId;
                    plc.TrackId = track.Id;
                    plc.OrderInList = track.OrderInList;
                    plc.TrackIdInPlaylist = track.TrackIdInPlaylist;
                    this.trackDao.CreatePlaylistContent(plc);

                    orderInList++;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            ((PlaylistView)this.playlistView).UpdateAfterTracklistSave(errorMessage);
        }

        private bool isSaving = false;
        private bool isTrackListChanged = false;
        private void TrackListChanged()
        {
            this.isTrackListChanged = true;

            this.playlistView.ChangeSaveButtonColor(true);
            this.mediaPlayerComponent.SetWorkingTable(trackListTable);
        }

        private async void SaveTrackList()
        {
            if (this.isTrackListChanged)
            {
                this.isTrackListChanged = false;

                bool readyToSave = false;

                if (isSaving)
                {
                    MessageBox.Show("Save is in progress, please wait!", "Tracklist Save", MessageBoxButtons.OK);
                }
                else
                {
                    if (this.isFilterModeEnabled)
                    {
                        DialogResult messageBoxResult = MessageBox.Show("Filter mode enabled. Do you want to save the current tracklist?", "Tracklist Save", MessageBoxButtons.YesNo);
                        if (messageBoxResult == DialogResult.Yes)
                        {
                            readyToSave = true;
                        }
                    }
                    else
                    {
                        readyToSave = true;
                    }

                    if (readyToSave)
                    {
                        isSaving = true;
                        this.playlistView.ChangeSaveStatus(true);

                        try
                        {
                            await SaveTrackListAsync();
                        }
                        finally
                        {
                            isSaving = false;
                            this.playlistView.ChangeSaveButtonColor(false);
                            this.playlistView.ChangeSaveStatus(false);
                        }
                    }
                }
            }
        }

        private void SaveTrackListEvent(object sender, EventArgs e)
        {
            this.SaveTrackList();
        }



        private List<Model.Track> ConvertTrackDataTableToList(DataTable dt)
        {
            List<Model.Track> trackList = new List<Model.Track>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.Track track = new Model.Track();
                track.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                track.OrderInList = Convert.ToInt32(dt.Rows[i]["OrderInList"]);
                track.TrackIdInPlaylist = Convert.ToInt32(dt.Rows[i]["TrackIdInPlaylist"]);
                trackList.Add(track);
            }
            return trackList;
        }
        #endregion


        #region TRACKLIST - ORDER

        public void OrderByArtist()
        {
            this.OrderByColumn("Artist");
        }
        public void OrderByTitle()
        {
            this.OrderByColumn("Title");
        }
        public void OrderByFileName()
        {
            this.OrderByColumn("FileName");
        }
        public void Reverse()
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0 && !isSaving)
            {
                DataTable reversedDt = this.trackListTable.Clone();
                for (var row = this.trackListTable.Rows.Count - 1; row >= 0; row--)
                    reversedDt.ImportRow(this.trackListTable.Rows[row]);
                this.trackListTable = reversedDt;

                this.ReloadTrackList();
                this.TrackListChanged();
            }
        }
        public void Shuffle()
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0 && !isSaving)
            {
                DataTable shuffledDt = this.trackListTable.Copy();

                if (!shuffledDt.Columns.Contains("SortBy"))
                    shuffledDt.Columns.Add("SortBy", typeof(Int32));

                Random rnd = new Random();
                foreach (DataRow row in shuffledDt.Rows)
                {
                    row["SortBy"] = rnd.Next(1, 100);
                }
                DataView dv = shuffledDt.DefaultView;
                dv.Sort = "SortBy";
                DataTable sortedDT = dv.ToTable();
                sortedDT.Columns.Remove("SortBy");
                this.trackListTable = sortedDT;

                this.ReloadTrackList();
                this.TrackListChanged();
            }
        }
        public void OrderByColumnEvent(object sender, Messenger e)
        {
            this.OrderByColumn(e.StringField1);
        }
        private void OrderByColumn(String columnName)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0 && !isSaving)
            {
                DataView dv = new DataView();

                if (this.isFilterModeEnabled)
                    dv = this.filteredTrackListTable.DefaultView;
                else
                    dv = this.trackListTable.DefaultView;

                dv.Sort = columnName;
                DataTable sortedDT = dv.ToTable();

                if (this.columnOrderStates[columnName] == -1)
                {
                    this.columnOrderStates[columnName] = 0;

                    if (this.isFilterModeEnabled)
                        this.filteredTrackListTable = sortedDT;
                    else
                        this.trackListTable = sortedDT;
                }
                else if (this.columnOrderStates[columnName] == 0)
                {
                    this.columnOrderStates[columnName] = 1;

                    DataTable reversedDt = new DataTable();

                    if (this.isFilterModeEnabled) { 
                        reversedDt = this.filteredTrackListTable.Clone();
                        for (var row = this.filteredTrackListTable.Rows.Count - 1; row >= 0; row--)
                            reversedDt.ImportRow(this.filteredTrackListTable.Rows[row]);
                    }
                    else
                    {
                        reversedDt = this.trackListTable.Clone();
                        for (var row = this.trackListTable.Rows.Count - 1; row >= 0; row--)
                            reversedDt.ImportRow(this.trackListTable.Rows[row]);
                    }

                    if (this.isFilterModeEnabled)
                        this.filteredTrackListTable = reversedDt;
                    else
                        this.trackListTable = reversedDt;
                }
                else if (this.columnOrderStates[columnName] == 1)
                {
                    this.columnOrderStates[columnName] = 0;

                    if (this.isFilterModeEnabled)
                        this.filteredTrackListTable = sortedDT;
                    else
                        this.trackListTable = sortedDT;
                }

                this.ReloadTrackList();
                this.TrackListChanged();
            }                
        }

        #endregion


        #region TRACKLIST/PLAYLIST - DRAG AND DROP

        private void MoveTracklistRowsEvent(object sender, Messenger e)
        {

            List<int> sourceIndices = e.SelectedIndices;
            int targetIndex = e.IntegerField1;

            // Create a list to hold the rows to be moved
            List<DataRow> rowsToMove = new List<DataRow>();

            // Collect the rows to be moved
            foreach (int sourceIndex in sourceIndices.OrderByDescending(i => i))
            {
                DataRow row = trackListTable.NewRow();
                row.ItemArray = trackListTable.Rows[sourceIndex].ItemArray;
                rowsToMove.Add(row);
                trackListTable.Rows.RemoveAt(sourceIndex);
            }

            // Adjust the target index if necessary
            if (targetIndex > sourceIndices.Min())
            {
                targetIndex -= rowsToMove.Count;
            }

            // Insert the rows at the target index
            foreach (DataRow row in rowsToMove)
            {
                trackListTable.Rows.InsertAt(row, targetIndex);
                targetIndex++;
            }

            this.ReloadTrackList();
            this.TrackListChanged();
        }
       
        private void InternalDragAndDropIntoTracklistEvent(object sender, Messenger e)
        {
            List<Model.Track> sourceTrackList = new List<Model.Track>();
            if (e.SelectedRows != null && e.SelectedRows.Count > 0)
            {
                for(int i = 0;i<= e.SelectedRows.Count - 1; i++)
                {
                    int trackId = (int)e.SelectedRows[i].Cells["Id"].Value;
                    Model.Track track = this.trackDao.GetTrackWithTags(trackId, this.tagList);
                    sourceTrackList.Add(track);
                }

                if(sourceTrackList != null && sourceTrackList.Count > 0)
                {
                    this.AddTracksToPlaylist(this.currentPlaylistId, sourceTrackList, e.IntegerField1, true);
                }
            }
        }
        private void InternalDragAndDropIntoPlaylistEvent(object sender, Messenger e)
        {
            List<Model.Track> sourceTrackList = new List<Model.Track>();
            DataRow playlistRow = null;
            int playlistId = -1;

            if(e.IntegerField1 > -1)
            {
                playlistRow = this.playlistListTable.Rows[e.IntegerField1];
                if (playlistRow != null)
                {
                    playlistId = Convert.ToInt32(playlistRow["Id"]);
                }

                if (e.SelectedRows != null && e.SelectedRows.Count > 0)
                {
                    for (int i = 0; i <= e.SelectedRows.Count - 1; i++)
                    {
                        int trackId = (int)e.SelectedRows[i].Cells["Id"].Value;
                        Model.Track track = this.trackDao.GetTrackWithTags(trackId, this.tagList);
                        sourceTrackList.Add(track);
                    }

                    if (sourceTrackList != null && sourceTrackList.Count > 0)
                    {
                        this.AddTracksToPlaylist(playlistId, sourceTrackList);
                        ((PlaylistView)this.playlistView).UpdateAfterCopyTracksToPlaylist(sourceTrackList.Count(), playlistRow["Name"].ToString());
                    }
                }
            }
           
        }

        private string[] scannedFileNames;
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

                    List<Tag> tagList = tagDao.GetAllTag();

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
                            Model.Track trackFromDb = this.trackDao.GetTrackWithTagsByPath(track.Path, tagList);
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
                                    ttv.Id = this.trackDao.GetNextId(TableName.TrackTagValue.ToString());
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

                    if (this.HasVirtualDj())
                    {

                        var automaticKeyImport = this.settingDao.GetBooleanSetting(Settings.AutomaticKeyImport.ToString()).Value;
                        var automaticBpmImport = this.settingDao.GetBooleanSetting(Settings.AutomaticBpmImport.ToString()).Value;

                        var keyTagValueList = automaticKeyImport ? this.tagDao.GetTagValuesByTagId(tagList.Find(x => x.Name == "Key")?.Id ?? 0) : new List<TagValue>();
                        var bpmTagValueList = automaticBpmImport ? this.tagDao.GetTagValuesByTagId(tagList.Find(x => x.Name == "Bpm")?.Id ?? 0) : new List<TagValue>();

                        var result = VirtualDJReader.Instance.ReadKeyAndBpmFromVirtualDJDatabase(ref trackList, this.trackDao, keyTagValueList, bpmTagValueList);
                        if (!result.Success)
                        {
                            MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                }
                return trackList;
            }
           
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
        private void ExternalDragAndDropIntoTracklistEvent(object sender, Messenger e)
        {
            List<Model.Track> trackList = new List<Model.Track>();
            string[] mediaFiles;
            string[] directories;

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

                this.AddTracksToPlaylist(this.currentPlaylistId, trackList, dragIndex);
            }
        }
        private void ExternalDragAndDropIntoPlaylistEvent(object sender, Messenger e)
        {
            List<Model.Track> trackList = new List<Model.Track>();
            string[] mediaFiles;
            string[] directories;
            DataRow playlistRow = null;
            int playlistId = -1;

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

                playlistRow = this.playlistListTable.Rows[dragIndex];
                if (playlistRow != null)
                {
                    playlistId = Convert.ToInt32(playlistRow["Id"]);
                }

                this.AddTracksToPlaylist(playlistId, trackList);
            }
        }
        private void AddTracksToPlaylist(int playlistId, List<Model.Track> trackList, int dragIndex = -1, bool internalDragAndDrop = false)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
                loadingDialog.SetProcessDescription("Add tracks to playlist/tracklist...");
                loadingDialog.Refresh();

                List<Model.Track> playlistTracklist = this.trackDao.GetTracklistWithTagsByPlaylistId(playlistId, this.tagList);

                if (trackList != null && trackList.Count > 0)
                {
                    int orderInList = playlistTracklist.Count;
                    foreach (Model.Track track in trackList)
                    {
                        PlaylistContent plc = new PlaylistContent();
                        plc.Id = this.trackDao.GetNextId(TableName.PlaylistContent.ToString());
                        plc.PlaylistId = playlistId;
                        plc.TrackId = track.Id;
                        plc.OrderInList = orderInList;
                        plc.TrackIdInPlaylist = this.trackDao.GetNextSmallestTrackIdInPlaylist();

                        if (internalDragAndDrop && track.TrackIdInPlaylist == this.mediaPlayerComponent.CurrentTrackIdInPlaylist)
                        {
                            this.mediaPlayerComponent.CurrentTrackIdInPlaylist = plc.TrackIdInPlaylist;
                        }

                        track.TrackIdInPlaylist = plc.TrackIdInPlaylist;
                        this.trackDao.CreatePlaylistContent(plc);
                        orderInList++;
                    }

                    if (playlistId == this.currentPlaylistId)
                    {
                        this.LoadTrackList(trackList, dragIndex);

                        bool playTrackAfterOpenFiles = this.settingDao.GetBooleanSetting(Settings.PlayTrackAfterOpenFiles.ToString()).Value;
                        if (playTrackAfterOpenFiles)
                        {
                            if (this.mediaPlayerComponent.MediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                            {
                                this.mediaPlayerComponent.SetCurrentTrackIndex(0);
                                this.PlayTrack();
                            }
                        }
                    }
                }
            }
            
        }
        private void MovePlaylistRowEvent(object sender, Messenger e)
        {

            int sourceIndex = e.IntegerField1;
            int targetIndex = e.IntegerField2;

            if (sourceIndex == targetIndex || sourceIndex <= 0 || targetIndex <= 0)
            {
                return; // No move needed or invalid indices
            }

            // Create a list to hold the row to be moved
            List<DataRow> rowsToMove = new List<DataRow>();

            // Collect the row to be moved
            DataRow row = playlistListTable.NewRow();
            row.ItemArray = playlistListTable.Rows[sourceIndex].ItemArray;
            rowsToMove.Add(row);
            playlistListTable.Rows.RemoveAt(sourceIndex);

            // Adjust the target index if necessary
            if (targetIndex > sourceIndex)
            {
                targetIndex -= rowsToMove.Count;
            }

            // Insert the row at the target index
            foreach (DataRow r in rowsToMove)
            {
                playlistListTable.Rows.InsertAt(r, targetIndex);
                targetIndex++;
            }

            this.SavePlaylistList();
            this.ReloadPlaylist();
        }
        private void MovePlaylistEvent(object sender, Messenger e)
        {
            int oldId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);

            DataRow row = this.playlistListTable.NewRow();
            row["Id"] = -1;
            row["Name"] = this.playlistListTable.Rows[e.IntegerField1]["Name"];
            row["OrderInList"] = this.playlistListTable.Rows[e.IntegerField1]["OrderInList"];
            row["ProfileId"] = this.playlistListTable.Rows[e.IntegerField1]["ProfileId"];
            row["IsActive"] = this.playlistListTable.Rows[e.IntegerField1]["IsActive"];
            row["IsModelTrainer"] = this.playlistListTable.Rows[e.IntegerField1]["IsModelTrainer"];

            this.playlistListTable.Rows.InsertAt(row, e.IntegerField2);

            for (var i = this.playlistListTable.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(this.playlistListTable.Rows[i]["Id"]) == oldId)
                {
                    this.playlistListTable.Rows[i].Delete();
                    break;
                }
            }

            for (var i = this.playlistListTable.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(this.playlistListTable.Rows[i]["Id"]) == -1)
                {
                    this.playlistListTable.Rows[i]["Id"] = oldId;
                    break;
                }
            }

            this.SavePlaylistList();
            this.ReloadPlaylist();
        }
        internal void CallAddTrackToTrackListEvent(List<Model.Track> trackList, int dragIndex)
        {
            ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);
            this.AddTracksToPlaylist(this.currentPlaylistId, trackList, dragIndex);
            ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);
            this.UpdateCoverBrowser(dragIndex);
        }
        private void LoadTrackList(List<Model.Track> trackList, int dragIndex)
        {
            

            if (trackList != null && trackList.Count > 0)
            {
                if (!isInitializing)
                    ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);

                foreach (Model.Track track in trackList)
                {
                    String length = this.LengthToString(track.Length);

                    if (dragIndex > -1)
                    {
                        DataRow dataRow = trackListTable.NewRow();
                        dataRow["Id"] = track.Id;
                        dataRow["Album"] = track.Album;
                        dataRow["Artist"] = track.Artist;
                        dataRow["Title"] = track.Title;
                        dataRow["Year"] = track.Year;
                        dataRow["Length"] = length;
                        dataRow["IsMissing"] = track.IsMissing;
                        dataRow["Path"] = track.Path;
                        dataRow["FileName"] = track.FileName;
                        dataRow["OrderInList"] = track.OrderInList;
                        dataRow["TrackIdInPlaylist"] = track.TrackIdInPlaylist;
                        if (track.TrackTagValues != null)
                        {
                            foreach (TrackTagValue ttv in track.TrackTagValues)
                            {
                                if (ttv.HasMultipleValues)
                                {
                                    dataRow[ttv.TagName] = ttv.Value;
                                    dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                                }
                                else
                                {
                                    dataRow[ttv.TagName] = ttv.TagValueName;
                                    dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                                }
                            }
                        }
                        this.trackListTable.Rows.InsertAt(dataRow, dragIndex);
                    }
                    else
                    {
                        object[] args = null;
                        if (track.TrackTagValues != null)
                        {
                            args = new object[11 + (track.TrackTagValues.Count * 2)];
                        }
                        else
                        {
                            args = new object[11];
                        }
                        
                        args[0] = track.Id;
                        args[1] = track.Album;
                        args[2] = track.Artist;
                        args[3] = track.Title;
                        args[4] = track.Year.ToString();
                        args[5] = length;
                        args[6] = track.IsMissing;
                        args[7] = track.Path;
                        args[8] = track.FileName;
                        args[9] = track.OrderInList;
                        args[10] = track.TrackIdInPlaylist;

                        if (track.TrackTagValues != null)
                        {
                            int i = 11;
                            foreach (TrackTagValue ttv in track.TrackTagValues)
                            {
                                if (ttv.HasMultipleValues)
                                {
                                    args[i] = ttv.Value;
                                }
                                else
                                {
                                    args[i] = ttv.TagValueName;
                                }
                                i++;

                                if (ttv.HasMultipleValues)
                                {
                                    args[i] = -1;
                                }
                                else{
                                    args[i] = ttv.TagValueId;
                                }
                                i++;
                            }
                        }
                        
                        this.trackListTable.Rows.Add(args);
                    }
                }

                if (!isInitializing)
                    ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);


                this.ReloadTrackList();
                this.TrackListChanged();
                ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
            }
        }
        #endregion


        #region TRACKLIST - REMOVE TRACKS

        //In the case of tracklist remove, selection change evetn must be turn off and on again to NOT load the UpdateCoverBrowser unnecessary
        private void DeleteTracksEvent(object sender, Messenger e)
        {
            // Check if filter mode is disabled and saving is not in progress
            if (!this.isFilterModeEnabled && !isSaving)
            {
                // Check if the track list table is not null and has rows
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    // Temporarily disable tracklist selection
                    ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);

                    using (LoadingDialog loadingDialog = new LoadingDialog())
                    {
                        loadingDialog.Show();
                        loadingDialog.SetProcessDescription("Delete tracks from playlist/tracklist...");
                        loadingDialog.Refresh();

                        // Find the selected rows and mark them for deletion
                        var rowsToDelete = e.Rows.Cast<DataGridViewRow>()
                        .Where(row => row.Selected)
                        .Select(row => row.Index)
                        .ToList();

                        if (rowsToDelete != null && rowsToDelete.Count == this.trackListTable.Rows.Count)
                        {
                            // Clear the entire table if all rows are selected
                            this.trackListTable.Rows.Clear();
                        }
                        else
                        {
                            // Delete the rows in reverse order to avoid index issues
                            foreach (int rowIndex in rowsToDelete.OrderByDescending(index => index))
                            {
                                this.trackListTable.Rows[rowIndex].Delete();
                            }
                        }
                    }

                    // Adjust the index to be within the valid range
                    int newIndex = Math.Min(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.trackListTable.Rows.Count - 1);
                    newIndex = Math.Max(newIndex, 0); // Ensure the index is not negative

                    // Perform UI updates after all deletions are done
                    this.ReloadTrackList();
                    this.TrackListChanged();
                    ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
                    ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);

                    // Update the cover browser with the adjusted index
                    this.UpdateCoverBrowser(newIndex);
                }
            }
        }
        public void RemoveMissingTracks()
        {
            if (!this.isFilterModeEnabled && !isSaving)
            {
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);
                    using (LoadingDialog loadingDialog = new LoadingDialog())
                    {
                        loadingDialog.Show();
                        loadingDialog.SetProcessDescription("Remove missing tracks from playlist/tracklist...");
                        loadingDialog.Refresh();

                        for (int i = this.trackListTable.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(this.trackListTable.Rows[i]["IsMissing"]))
                            {
                                this.trackListTable.Rows[i].Delete();
                            }
                        }
                    }

                    // Adjust the index to be within the valid range
                    int newIndex = Math.Min(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.trackListTable.Rows.Count - 1);
                    newIndex = Math.Max(newIndex, 0); // Ensure the index is not negative

                    this.ReloadTrackList();
                    this.TrackListChanged();
                    ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
                    ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);
                    // Update the cover browser with the adjusted index
                    this.UpdateCoverBrowser(newIndex);
                }
            }
        }
        public void RemoveDuplicatedTracks()
        {
            if (!this.isFilterModeEnabled && !isSaving)
            {
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);
                    using (LoadingDialog loadingDialog = new LoadingDialog())
                    {
                        loadingDialog.Show();
                        loadingDialog.SetProcessDescription("Remove duplicated tracks from playlist/tracklist...");
                        loadingDialog.Refresh();

                        List<int> trackIds = new List<int>();
                        List<int> trackIdsToRemove = new List<int>();

                        int trackIdInPlaylist = 0;
                        int rowIndexToKeep = 0;

                        for (int i = this.trackListTable.Rows.Count - 1; i >= 0; i--)
                        {
                            if (!trackIds.Contains(Convert.ToInt32(this.trackListTable.Rows[i]["Id"])))
                            {
                                trackIdInPlaylist = Convert.ToInt32(this.trackListTable.Rows[i]["TrackIdInPlaylist"]);
                                if (this.mediaPlayerComponent.CurrentTrackIdInPlaylist != trackIdInPlaylist)
                                {
                                    trackIds.Add(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]));
                                }
                                else
                                {
                                    trackIdsToRemove.Add(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]));
                                    rowIndexToKeep = i;
                                }
                            }
                            else
                            {
                                trackIdsToRemove.Add(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]));
                            }
                        }
                        for (int i = this.trackListTable.Rows.Count - 1; i >= 0; i--)
                        {
                            if (trackIdsToRemove.Contains(Convert.ToInt32(this.trackListTable.Rows[i]["Id"])) && rowIndexToKeep != i)
                            {
                                trackIdsToRemove.Remove(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]));
                                this.trackListTable.Rows[i].Delete();
                            }
                        }
                    }

                    // Adjust the index to be within the valid range
                    int newIndex = Math.Min(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.trackListTable.Rows.Count - 1);
                    newIndex = Math.Max(newIndex, 0); // Ensure the index is not negative

                    this.ReloadTrackList();
                    this.TrackListChanged();
                    ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
                    ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);
                    // Update the cover browser with the adjusted index
                    this.UpdateCoverBrowser(newIndex);
                }
            }
        }
        public void Clear()
        {
            // Check if filter mode is disabled and saving is not in progress
            if (!this.isFilterModeEnabled && !isSaving)
            {
                // Temporarily disable tracklist selection
                ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);

                // Clear all rows in the track list table
                this.trackListTable.Rows.Clear();

                // Perform UI updates after clearing the track list
                this.ReloadTrackList();
                this.TrackListChanged();
                ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);

                // Re-enable tracklist selection
                ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);

                // Update the cover browser with the adjusted index
               this.UpdateCoverBrowser(0); // Clear operation, so start from the beginning
            }
        }

        #endregion


        #region PLAYER
        ///meghívja a media player függvényeit, amelyek beállítják a lejátszó STÁTUSZÁT
        ///a STÁTUSZ alapján frissíti a felületen az aktuálisan szóló vagy leállított szám kiíratását:

        ///UpdateAfterPlayTrack:             "Playing: aktuális szám"        + színez
        ///UpdateAfterPauseTrack:            "Paused: aktuálios szám"
        ///UpdateAfterStopTrack:             "Playing: -"                     + színez
        ///UpdateAfterPlayTrackAfterPause:   "Playing: aktuális szám"

        ///FONTOS: van egy timer által hívott függvény itt, ami visszafrissíti a felületen az eltelt/hátralő időt és progress bar-t
        ///FONTOS: a felületen a számcím frissítése után egyes esetekben leut a lista színezése függvény
        ///FONTOS: ha a progress bar betelik, akkor a media player STÁTUSZA alapján ugrik a következő számra, frissíti a felületet a fentiekkel
        public void SetCurrentTrackEvent(object sender, Messenger e)
        {
            this.mediaPlayerComponent.SetCurrentTrackIndex(e.IntegerField1);

            if(e.IntegerField1 > -1)
                this.UpdateCoverBrowser(e.IntegerField1);
        }
        public void PlayTrackEvent(object sender, Messenger e)
        {
            this.PlayTrack();
        }
        public void PlayTrack()
        {
            MediaPlayerUpdateState updateState = this.mediaPlayerComponent.PlayTrack();

            if (updateState == MediaPlayerUpdateState.AfterPlay)
            {
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.mediaPlayerComponent.CurrentTrackIdInPlaylist);
                }
            }
            else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
            {
                this.playlistView.UpdateAfterPlayTrackAfterPause();
            }
        }
        public void PauseTrackEvent(object sender, EventArgs e)
        {
            if (this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                MediaPlayerUpdateState updateState = this.mediaPlayerComponent.PlayTrack();

                if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                {
                    this.playlistView.UpdateAfterPlayTrackAfterPause();
                }
            }
            else
            {
                this.mediaPlayerComponent.PauseTrack();
                this.playlistView.UpdateAfterPauseTrack();
            }
           
        }
        public void StopTrackEvent(object sender, EventArgs e)
        {
            this.mediaPlayerComponent.StopTrack();
            this.playlistView.UpdateAfterStopTrack();
        }

        public void PrevTrackEvent(object sender, Messenger e)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                MediaPlayerUpdateState updateState = this.mediaPlayerComponent.PrevTrack();

                if (updateState == MediaPlayerUpdateState.AfterPlay)
                {
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.mediaPlayerComponent.CurrentTrackIdInPlaylist);
                }
                else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                {
                    this.playlistView.UpdateAfterPlayTrackAfterPause();
                }
            }

        }
        public void NextTrackEvent(object sender, Messenger e)
        {
            if (this.mediaPlayerComponent.IsShuffleEnabled)
            {
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    MediaPlayerUpdateState updateState = this.mediaPlayerComponent.RandomTrack();
                    if (updateState == MediaPlayerUpdateState.AfterPlay)
                    {
                        this.playlistView.UpdateAfterPlayTrack(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.mediaPlayerComponent.CurrentTrackIdInPlaylist);
                    }
                    else
                    {
                        this.playlistView.UpdateAfterPlayTrackAfterPause();
                    }
                }
            }
            else
            {
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    MediaPlayerUpdateState updateState = this.mediaPlayerComponent.NextTrack();

                    if (updateState == MediaPlayerUpdateState.AfterPlay)
                    {
                        this.playlistView.UpdateAfterPlayTrack(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.mediaPlayerComponent.CurrentTrackIdInPlaylist);
                    }
                    else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                    {
                        this.playlistView.UpdateAfterPlayTrackAfterPause();
                    }

                }
            }
        }
        private void MediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                playNextTrackTimer.Start();
            }
        }
        private void PlayNextTrackTimer_Tick(object sender, EventArgs e)
        {
            playNextTrackTimer.Stop();
            if (this.mediaPlayerComponent.IsShuffleEnabled)
            {
                PlayRandomTrack();
            }
            else
            {
                PlayNextTrack();
            }
        }
        public void RandomTrackEvent(object sender, EventArgs e)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                MediaPlayerUpdateState updateState = this.mediaPlayerComponent.RandomTrack();
                if (updateState == MediaPlayerUpdateState.AfterPlay)
                {
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.mediaPlayerComponent.CurrentTrackIdInPlaylist);
                }
                else
                {
                    this.playlistView.UpdateAfterPlayTrackAfterPause();
                }
            } 
        }

        private void PlayRandomTrack()
        {
            if (this.trackListTable != null && this.trackListTable.Rows.Count > 0)
            {
                MediaPlayerUpdateState updateState = this.mediaPlayerComponent.RandomTrack();
                UpdatePlaylistView(updateState);
            }
        }

        private void PlayNextTrack()
        {
            if (this.trackListTable != null && this.trackListTable.Rows.Count > 0) 
            { 
                if(this.mediaPlayerComponent.selectedRowIndex != this.trackListTable.Rows.Count-1)
                {
                    MediaPlayerUpdateState updateState = this.mediaPlayerComponent.NextTrack();
                    UpdatePlaylistView(updateState);
                }
            }
        }
        private void UpdatePlaylistView(MediaPlayerUpdateState updateState)
        {
            if (updateState == MediaPlayerUpdateState.AfterPlay)
            {
                this.playlistView.UpdateAfterPlayTrack(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.mediaPlayerComponent.CurrentTrackIdInPlaylist);
            }
            else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
            {
                this.playlistView.UpdateAfterPlayTrackAfterPause();
            }
        }

        internal void CallChangeProgressEvent(int currentPosX, int width)
        {
            this.mediaPlayerComponent.ChangeProgress(currentPosX, width);
        }

        internal void CallChangeVolumeEvent(int volume)
        {
            this.mediaPlayerComponent.ChangeVolume(volume);
            this.settingDao.SetIntegerSetting(Settings.Volume.ToString(), volume);
            this.settingDao.SetBooleanSetting(Settings.IsMuteEnabled.ToString(), false);
        }
        internal void CallChangeMuteEvent(bool isMuteEnabled)
        {
            this.settingDao.SetBooleanSetting(Settings.IsMuteEnabled.ToString(), isMuteEnabled);

            int volume = 0;
            if (isMuteEnabled)
            {
                volume = 0;
                this.mediaPlayerComponent.ChangeVolume(volume);
                this.playlistView.SetVolume(volume);
            }
            else
            {
                volume = this.settingDao.GetIntegerSetting(Settings.Volume.ToString());
                this.mediaPlayerComponent.ChangeVolume(volume);
                this.playlistView.SetVolume(volume);
            }
        }
        internal void CallChangeShuffleEvent(bool isShuffleEnabled)
        {
            this.mediaPlayerComponent.ChangeShuffle(isShuffleEnabled);
            this.settingDao.SetBooleanSetting(Settings.IsShuffleEnabled.ToString(), isShuffleEnabled);
        }
        internal void CallChangePreviewEvent(bool isPreviewEnabled)
        {
            this.mediaPlayerComponent.ChangePreview(isPreviewEnabled);
            this.settingDao.SetBooleanSetting(Settings.IsPreviewEnabled.ToString(), isPreviewEnabled);
        }

       

       
        #endregion



        #region COVER BROWSER

        private int displayedCoverNumber = 11;
        private ConcurrentQueue<ImageExtension> coverList;
        private void LoadCoversEvent(object sender, Messenger e)
        {
            this.UpdateCoverBrowser(e.IntegerField1);
        }
        private void UpdateCoverBrowser(int index)
        {
            coverList = new ConcurrentQueue<ImageExtension>();
            List<DataRow> trackList = this.isFilterModeEnabled ? this.filteredTrackListTable.Rows.Cast<DataRow>().ToList() : trackListTable.Rows.Cast<DataRow>().ToList();

            // Handle the empty track list case
            if (trackList == null || trackList.Count == 0)
            {
                this.playlistView.UpdateCoverList(this.coverList); // Clear the cover list in the view
                return;
            }

            // Adjust the index to be within the valid range
            index = Math.Min(index, trackList.Count - 1);
            index = Math.Max(index, 0); // Ensure the index is not negative

            int startIndex = Math.Max(0, index - 5);
            int finalIndex = Math.Min(trackList.Count, index + 6);

            // Adjust start and final indices based on the displayed cover number
            if (trackList.Count > displayedCoverNumber)
            {
                if (index <= 5)
                {
                    finalIndex = Math.Min(trackList.Count, displayedCoverNumber);
                }
                else if (index + 6 > trackList.Count)
                {
                    startIndex = Math.Max(0, trackList.Count - displayedCoverNumber);
                }
            }

            // Enqueue cover images for the specified range
            for (int i = startIndex; i < finalIndex; i++)
            {
                var coverImage = new ImageExtension
                {
                    FilePath = trackList[i]["Path"].ToString(),
                    TrackIdInPlaylist = Convert.ToInt32(trackList[i]["TrackIdInPlaylist"]),
                    IsMainCover = (i == index)
                };
                coverList.Enqueue(coverImage);
            }

            // Process and update cover images in parallel
            if (coverList.Count > 0)
            {
                Parallel.ForEach(coverList, preparedCoverImage =>
                {
                    try
                    {
                        using (var file = TagLib.File.Create(preparedCoverImage.FilePath))
                        {
                            if (file?.Tag.Pictures.Length > 0)
                            {
                                var pictureData = file.Tag.Pictures[0].Data.Data;
                                using (var ms = new MemoryStream(pictureData))
                                {
                                    ms.Position = 0;
                                    using (var img = Image.FromStream(ms))
                                    {
                                        preparedCoverImage.Image = img.GetThumbnailImage(75, 75, () => false, IntPtr.Zero);
                                    }
                                }
                            }
                            else
                            {
                                preparedCoverImage.Image = Properties.Resources.MissingCover;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing file {preparedCoverImage.FilePath}: {ex.Message}");
                        preparedCoverImage.Image = Properties.Resources.MissingCover;
                    }
                });

                // Update the cover list in the view
                this.playlistView.UpdateCoverList(this.coverList);
            }
        }

        #endregion


        #region PLAYLIST

        private void CreatePlaylist(object sender, EventArgs e)
        {
            PlaylistEditorView playlistEditorView = new PlaylistEditorView();
            PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(playlistEditorView, this.trackDao, this.settingDao);
            if (playlistEditorView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
            {
                this.playlistListTable.Rows.Add(presenter.newPlaylist.Id, presenter.newPlaylist.Name, presenter.newPlaylist.OrderInList, presenter.newPlaylist.ProfileId, presenter.newPlaylist.IsActive, presenter.newPlaylist.IsModelTrainer);
                this.SavePlaylistList();
                this.ReloadPlaylist();
            }
        }
        private void EditPlaylist(object sender, Messenger e)
        {
            DataRow playlistRow = this.playlistListTable.Select("Id = " + Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"])).First();
            if (playlistRow != null)
            {
                int id = (int)playlistRow["Id"];

                Playlist playlist  = this.trackDao.GetPlaylist(id);

                int playlistIndex = this.playlistListTable.Rows.IndexOf(playlistRow);

                PlaylistEditorView playlistEditorView = new PlaylistEditorView();
                PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(playlistEditorView, this.trackDao, this.settingDao, playlist);
                if (playlistEditorView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
                {
                    this.playlistListTable.Rows[playlistIndex]["Name"] = presenter.newPlaylist?.Name;
                    this.playlistListTable.Rows[playlistIndex]["IsModelTrainer"] = presenter.newPlaylist?.IsModelTrainer;

                    this.SavePlaylistList();
                    this.ReloadPlaylist();
                }
            }
        }

        private void LoadPlaylistEvent(object sender, Messenger e)
        {
                if (this.isTrackListChanged)
                {
                    this.SaveTrackListSync();
                    this.isTrackListChanged = false;
                    this.playlistView.ChangeSaveButtonColor(false);
                }

                this.currentPlaylistId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);
                this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId);

                this.ReloadPlaylist();
                this.InitializeTrackListRows(this.trackListTable, null);

                this.mediaPlayerComponent.SetWorkingTable(this.trackListTable);
        }

        /*
        private void LoadPlaylist(int playlistIndex)
        {
            if(playlistIndex != -1)
            {
                this.currentPlaylistId = Convert.ToInt32(this.playlistListTable.Rows[playlistIndex]["Id"]);
                this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId);
                
                this.trackListTable.Rows.Clear();
                this.mediaPLayerComponent.ClearPlaylist(this.trackListTable);

                //lekérjük az aktuális plalyist-et és betöltjük a számokat belőle
                Playlist playlist = this.trackDao.GetPlaylist(this.currentPlaylistId);
                if(playlist != null){
                    this.LoadPlaylist(playlist);
                }

                //ha a szám ebben a listából szól a lejátszóban, ki kellene szinezni
                if (this.mediaPLayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying ||
                    this.mediaPLayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
                {
                    int currentTrackIdInPlaylist = this.mediaPLayerComponent.CurrentTrackIdInPlaylist;
                    int trackIdInPlaylist = -1;

                    //FONTOS: a this.trackListTable feltöltődik a LoadPlaylist-ben!
                    for (int i = 0; i <= this.trackListTable.Rows.Count - 1; i++)
                    {
                        trackIdInPlaylist = Convert.ToInt32(trackListTable.Rows[i]["TrackIdInPlaylist"]);
                        if (trackIdInPlaylist == currentTrackIdInPlaylist)
                        {
                            this.playlistView.SetCurrentTrackColor(currentTrackIdInPlaylist);
                            break;
                        }
                    }
                }
                //this.trackDao.SetA{ctivePlaylist(this.currentPlaylistId);

                 for (int i = 0; i <= this.playlistListTable.Rows.Count - 1; i++)
                 {
                     if (Convert.ToInt32(this.playlistListTable.Rows[i]["Id"]) == this.currentPlaylistId)
                     {
                         this.playlistListTable.Rows[i]["IsActive"] = true;
                     }
                     else
                     {
                         this.playlistListTable.Rows[i]["IsActive"] = false;
                     }

                 }



                //this.mediaPLayerComponent.Initialize(this.trackListTable);


                // Playlist playlist = this.trackDao.GetActivePlaylist();
                //  this.LoadPlaylist(playlist);



            }
            
        }*/
        private void DeletePlaylistEvent(object sender, Messenger e)
        {
            if (e.IntegerField1 > -1)
            {
                String playlistName = this.playlistListTable.Rows[e.IntegerField1]["Name"].ToString();
                int playlistId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);

                if (playlistName.Equals("Default Playlist"))
                {
                    MessageBox.Show("Default playlist cannot be deleted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DialogResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        this.playlistListTable.Rows.RemoveAt(e.IntegerField1);
                        this.trackDao.DeletePlaylist(playlistId);

                        //ha az aktuális listát töröljük, betöltjük a megelőző listát (a legelső, utolsó listát nem lehet törölni, így az lesz az utolsó)
                        if (playlistId == this.currentPlaylistId)
                        {
                            int nextPlaylistIndex = 0;
                            for (int i = 0; i < this.playlistListTable.Rows.Count; i++)
                            {
                                if (Convert.ToInt32(this.playlistListTable.Rows[0]["Id"]) == playlistId)
                                {
                                    nextPlaylistIndex = i--;
                                    break;
                                }
                            }

                            this.currentPlaylistId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);
                            this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId);
                            this.ReloadPlaylist();

                            this.InitializeTrackListRows(this.trackListTable,null);
                            this.mediaPlayerComponent.SetWorkingTable(this.trackListTable);
                        }
                        else
                        {
                            this.ReloadPlaylist();
                        }                        
                    }
                }

            }
        }

        private void DisplayPlaylistListEvent(object sender, EventArgs e)
        {
            this.isPlaylistListDisplayed = !this.isPlaylistListDisplayed;
            this.settingDao.SetBooleanSetting(Settings.IsPlaylistListDisplayed.ToString(), this.isPlaylistListDisplayed);
            ((PlaylistView)this.playlistView).UpdateDisplayPlaylistList(this.isPlaylistListDisplayed);
        }
        

        /* private void SetPlaylistAsCurrent(int playlistId)
         {
             for(int i = 0; i<= this.playlistListTable.Rows.Count - 1; i++)
             {
                 if(Convert.ToInt32(this.playlistListTable.Rows[i]["Id"]) != playlistId)
                 {
                     this.playlistListTable.Rows[i]["IsActive"] = false;

                 }
                 else
                 {
                     this.playlistListTable.Rows[i]["IsActive"] = true;
                 }
             }
             this.trackDao.SetActivePlaylist(playlistId);
         }*/
        /*  private void SetQuickListEvent(object sender, ListEventArgs e)
          {
              int oldgroup = Convert.ToInt32(playlistListTable.Rows[e.IntegerField1]["G"]);

              for (int i = 0; i <= this.playlistListTable.Rows.Count - 1; i++)
              {
                  if (Convert.ToInt32(this.playlistListTable.Rows[i]["G"]) == e.IntegerField2)
                  {
                      this.playlistListTable.Rows[i]["G"] = 0;
                      break;
                  }
              }

              if(oldgroup == 0)
              {
                  playlistListTable.Rows[e.IntegerField1]["G"] = e.IntegerField2.ToString();
              }
              else
              {
                  if (oldgroup != e.IntegerField2)
                  {
                      playlistListTable.Rows[e.IntegerField1]["G"] = e.IntegerField2.ToString();
                  }
              }

              this.SavePlaylistList(this.playlistListTable);
              this.ReloadPlaylistDatagGridView(this.playlistListTable);
          }*/
        private void ExportToM3UEvent(object sender, Messenger e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Playlist files (*.m3u)|*.m3u";
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                String row = String.Empty;

                using (StreamWriter myStream = new StreamWriter(sfd.FileName))
                {
                    int playlistId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);
                    List<Model.Track> trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(playlistId, this.tagList);
                    if (trackList != null && trackList.Count > 0)
                    {
                        foreach (Model.Track track in trackList)
                        {
                            row = "#EXTVDJ:";
                            row += track.Length.ToString() + "," ;
                            if (!string.IsNullOrEmpty(track.Title))
                            {
                                row += track.Artist + " - " + track.Title;
                            }
                            else
                            {
                                row += track.Artist;
                            }

                            myStream.WriteLine(row);
                            myStream.WriteLine(track.Path);
                        }
                        myStream.Close();
                    }
                }
            }
        }
        private void ExportToTXTEvent(object sender, Messenger e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text files (*.txt)|*.txt";
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                String row = String.Empty;
                int index = 1;

                using (StreamWriter myStream = new StreamWriter(sfd.FileName))
                {
                    int playlistId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);
                    List<Model.Track> trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(playlistId, this.tagList);
                    if (trackList != null && trackList.Count > 0)
                    {
                        foreach (Model.Track track in trackList)
                        {
                            row = index.ToString() + ". ";
                            
                            if (!string.IsNullOrEmpty(track.Title))
                            {
                                row += track.Artist + " - " + track.Title;
                            }
                            else
                            {
                                row += track.Artist;
                            }
                            row += " (" + this.LengthToString(track.Length) + ")";

                            myStream.WriteLine(row);

                            index++;
                        }
                        myStream.Close();
                    }
                }
            }
        }
        private void ExportToDirectoryEvent(object sender, Messenger e)
        {
            ExportToDirectoryView exportToDirectoryView = new ExportToDirectoryView();

            int playlistId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);
            List<Model.Track> trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(playlistId, this.tagList);
            if (trackList != null && trackList.Count > 0)
            {
                ExportToDirectoryPresenter presenter = new ExportToDirectoryPresenter(exportToDirectoryView, trackList, this.tagDao, this.settingDao);
                presenter.Initialize();


                if (exportToDirectoryView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
                {

                }
            }

                
        }
        #endregion

        private void ShowColumnVisibilityEditorEvent(object sender, EventArgs e)
        {
            ColumnVisibilityEditorView columnVisibilityEditorView = new ColumnVisibilityEditorView();
            ColumnVisibilityEditorPresenter presenter = new ColumnVisibilityEditorPresenter(columnVisibilityEditorView,this.trackDao, this.settingDao);
 
            if (columnVisibilityEditorView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
            {
                this.trackColumnVisibilityArray = presenter.TrackPropertyList.Select(x => x.IsEnabled).ToArray();

                int i = 0;
                foreach(TrackProperty tp in presenter.TrackPropertyList)
                {
                    tp.SortingId = i;
                    this.settingDao.UpdateTrackProperty(tp);
                    i++;
                }

                this.ReloadTrackList();
            }
        }
        private void ScanKeyAndBpmEvent(object sender, EventArgs e)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
                loadingDialog.SetProcessDescription("Scan key and bpm from VirtualDj databases..."); 
                loadingDialog.Refresh();

                try
                {
                    bool automaticKeyImport = this.settingDao.GetBooleanSetting(Settings.AutomaticKeyImport.ToString()).Value;
                    bool automaticBpmImport = this.settingDao.GetBooleanSetting(Settings.AutomaticBpmImport.ToString()).Value;

                    List<TagValue> keyTagValueList = new List<TagValue>();
                    List<TagValue> bpmTagValueList = new List<TagValue>();

                    int keyTagId = -1;
                    int bpmTagId = -1;

                    if (automaticKeyImport)
                    {
                        Tag tag = this.tagList.Find(x => x.Name == "Key");
                        if (tag != null)
                        {
                            keyTagId = tag.Id;
                            keyTagValueList = this.tagDao.GetTagValuesByTagId(tag.Id);
                        }
                    }
                    if (automaticBpmImport)
                    {
                        Tag tag = this.tagList.Find(x => x.Name == "Bpm");
                        if (tag != null)
                        {
                            bpmTagId = tag.Id;
                            bpmTagValueList = this.tagDao.GetTagValuesByTagId(tag.Id);
                        }
                    }

                    List<Model.Track> trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(this.currentPlaylistId, this.tagList);
                    if (trackList != null && trackList.Count > 0)
                    {
                        foreach (Model.Track track in trackList)
                        {
                            if (track.TrackTagValues != null && track.TrackTagValues.Count > 0)
                            {
                                TrackTagValue ttv = track.TrackTagValues.Find(x => x.TagId == keyTagId);
                                if (ttv != null)
                                {
                                    if (automaticKeyImport)
                                    {
                                        track.IsNew = true;
                                    }
                                }
                                ttv = track.TrackTagValues.Find(x => x.TagId == bpmTagId);
                                if (ttv != null)
                                {
                                    if (automaticBpmImport)
                                    {
                                        track.IsNew = true;
                                    }
                                }
                            }

                        }

                        VirtualDJReader.Instance.ReadKeyAndBpmFromVirtualDJDatabase(ref trackList, this.trackDao, keyTagValueList, bpmTagValueList);

                        TagValue tv = null;
                        String tagValueName = String.Empty;
                        String keyCode = String.Empty;
                        String[] parts = null;

                        foreach (Track track in trackList)
                        {
                            if (track.Path.EndsWith(".mp3"))
                            {
                                using (var file = TagLib.File.Create(track.Path))
                                {
                                    track.Comment = file.Tag.Comment;
                                }
                            }
                            else if (track.Path.EndsWith(".flac"))
                            {
                                using (var file = new FlacFile(track.Path))
                                {
                                    var vorbisComment = file.VorbisComment;
                                    if (vorbisComment != null)
                                    {
                                        var comment = vorbisComment["COMMENT"];
                                        if (comment != null && comment.Count > 0)
                                        {
                                            track.Comment = comment[0];
                                        }
                                    }
                                }
                            }

                            this.trackDao.UpdateTrack(track);

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

                        this.InitializeTrackListRows(this.trackListTable, trackList);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void DisplayTagComponentEvent(object sender, EventArgs e)
        {
            this.isTagComponentDisplayed = !this.isTagComponentDisplayed;
            this.settingDao.SetBooleanSetting(Settings.IsTagEditorComponentDisplayed.ToString(), this.isTagComponentDisplayed);
            ((PlaylistView)this.playlistView).UpdateDisplayTagComponent(this.isTagComponentDisplayed);
        }

        private bool isOnlyPlayingRowModeEnabled { get; set; }
        private bool isFilterModeEnabled { get; set; }

        private void EnableFilterModeEvent(object sender, EventArgs e)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
                loadingDialog.SetProcessDescription("Preparing filter mode..."); 
                loadingDialog.Refresh();

                if (!this.isFilterModeEnabled)
                {
                    this.isTrackListChanged = true;
                    this.SaveTrackList();
                }

                this.isFilterModeEnabled = true;

                DataView dv = this.trackListTable.DefaultView;
                dv.RowFilter = "";
                DataTable filteredDT = dv.ToTable();
                this.filteredTrackListTable = filteredDT;

                ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);

                this.trackListBindingSource.DataSource = this.filteredTrackListTable;
                this.InitializeTrackList(this.filteredTrackListTable, this.mediaPlayerComponent.CurrentTrackIdInPlaylist);

                ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);

                this.isFilterModeEnabled = true;
                this.mediaPlayerComponent.SetWorkingTable(this.filteredTrackListTable);
            }
        }
        private void EnableSetterModeEvent(object sender, EventArgs e)
        {
            if (this.isFilterModeEnabled)
            {
                this.tagValueFilterList = new List<TagValueFilter>();

                DataView dv = this.trackListTable.DefaultView;
                dv.RowFilter = "";
                DataTable filteredDT = dv.ToTable();
                this.trackListTable = filteredDT;

                ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);

                this.trackListBindingSource.DataSource = this.trackListTable;
                this.InitializeTrackList(this.trackListTable, this.mediaPlayerComponent.CurrentTrackIdInPlaylist);

                ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);
            }

            this.isFilterModeEnabled = false;
            this.mediaPlayerComponent.SetWorkingTable(this.trackListTable);
        }
        private void ChangeOnlyPlayingRowModeEnabled(object sender, Messenger e)
        {
            this.isOnlyPlayingRowModeEnabled = e.BooleanField1;
            this.settingDao.SetBooleanSetting(Settings.IsOnlyPlayingRowModeEnabled.ToString(), this.isOnlyPlayingRowModeEnabled);
        }

        private void SetTagValue(String tagName,String tagValueName,String tagValueValue,int tagValueId, DataGridViewRowCollection rows)
        {
            if (this.tagList != null && this.tagList.Count > 0)
            {
                Tag currentTag = this.tagList.Find(x => x.Name == tagName);
                if (currentTag != null)
                {
                    List<TagValue> tagValueList = this.tagDao.GetTagValuesByTagId(currentTag.Id);
                    if (tagValueList != null && tagValueList.Count > 0)
                    {
                        if (currentTag.HasMultipleValues)
                        {
                            TagValue tv = tagValueList[0];
                            if (tv != null)
                            {
                                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                                {
                                    if (!this.isOnlyPlayingRowModeEnabled)
                                    {
                                        for (int i = rows.Count - 1; i >= 0; i--)
                                        {
                                            if (rows[i].Selected)
                                            {
                                                Model.Track track = this.trackDao.GetTrackWithTags(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                                if (track != null)
                                                {
                                                    foreach (TrackTagValue ttv in track.TrackTagValues)
                                                    {
                                                        if (ttv.TagId == currentTag.Id)
                                                        {
                                                            ttv.TagValueId = tv.Id;
                                                            ttv.TagValueName = tv.Name;
                                                            ttv.HasMultipleValues = currentTag.HasMultipleValues;
                                                            ttv.Value = tagValueValue;
                                                            this.trackDao.UpdateTrackTagValue(ttv);
                                                            break;
                                                        }
                                                    }
                                                }
                                                this.trackListTable.Rows[i][currentTag.Name] = tagValueValue;
                                                this.trackListTable.Rows[i][currentTag.Name + "TagValueId"] = tagValueId;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int currentTrackIdInPlaylist = this.mediaPlayerComponent.CurrentTrackIdInPlaylist;
                                        for (int i = rows.Count - 1; i >= 0; i--)
                                        {
                                            int trackIdInPlaylist = Convert.ToInt32(this.trackListTable.Rows[i]["TrackIdInPlaylist"]);
                                            if (trackIdInPlaylist == currentTrackIdInPlaylist)
                                            {
                                                Model.Track track = this.trackDao.GetTrackWithTags(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                                if (track != null)
                                                {
                                                    foreach (TrackTagValue ttv in track.TrackTagValues)
                                                    {
                                                        if (ttv.TagId == currentTag.Id)
                                                        {
                                                            ttv.TagValueId = tv.Id;
                                                            ttv.TagValueName = tv.Name;
                                                            ttv.HasMultipleValues = currentTag.HasMultipleValues;
                                                            ttv.Value = tagValueValue;
                                                            this.trackDao.UpdateTrackTagValue(ttv);
                                                            break;
                                                        }
                                                    }
                                                }
                                                this.trackListTable.Rows[i][currentTag.Name] = tagValueValue;
                                                this.trackListTable.Rows[i][currentTag.Name + "TagValueId"] = tagValueId;
                                                break;
                                            }
                                        }
                                    }


                                    this.ReloadTrackList();
                                    this.TrackListChanged();
                                }
                            }
                        }
                        else
                        {
                            TagValue tv = tagValueList.Find(x => x.Name == tagValueName);
                            if (tv != null)
                            {
                                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                                {
                                    if (!this.isOnlyPlayingRowModeEnabled)
                                    {
                                        for (int i = rows.Count - 1; i >= 0; i--)
                                        {
                                            if (rows[i].Selected)
                                            {
                                                Model.Track track = this.trackDao.GetTrackWithTags(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                                if (track != null)
                                                {
                                                    foreach (TrackTagValue ttv in track.TrackTagValues)
                                                    {
                                                        if (ttv.TagId == currentTag.Id)
                                                        {
                                                            ttv.TagValueId = tv.Id;
                                                            ttv.TagValueName = tv.Name;
                                                            this.trackDao.UpdateTrackTagValue(ttv);
                                                            break;
                                                        }
                                                    }
                                                }
                                                this.trackListTable.Rows[i][currentTag.Name] = tv.Name;
                                                this.trackListTable.Rows[i][currentTag.Name + "TagValueId"] = tagValueId;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int currentTrackIdInPlaylist = this.mediaPlayerComponent.CurrentTrackIdInPlaylist;
                                        for (int i = rows.Count - 1; i >= 0; i--)
                                        {
                                            int trackIdInPlaylist = Convert.ToInt32(this.trackListTable.Rows[i]["TrackIdInPlaylist"]);
                                            if (trackIdInPlaylist == currentTrackIdInPlaylist)
                                            {
                                                Model.Track track = this.trackDao.GetTrackWithTags(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                                if (track != null)
                                                {
                                                    foreach (TrackTagValue ttv in track.TrackTagValues)
                                                    {
                                                        if (ttv.TagId == currentTag.Id)
                                                        {
                                                            ttv.TagValueId = tv.Id;
                                                            ttv.TagValueName = tv.Name;
                                                            this.trackDao.UpdateTrackTagValue(ttv);
                                                            break;
                                                        }
                                                    }
                                                }
                                                this.trackListTable.Rows[i][currentTag.Name] = tv.Name;
                                                this.trackListTable.Rows[i][currentTag.Name + "TagValueId"] = tagValueId;
                                                break;
                                            }
                                        }
                                    }


                                    this.ReloadTrackList();
                                    this.TrackListChanged();

                                }
                            }

                        }
                    }


                }
            }
        }

        private List<TagValueFilter> tagValueFilterList { get; set; }
        private void FilterByTagValue(String tagName, String tagValueName, String tagValueValue, int tagValueId)
        {
            if(this.tagValueFilterList == null || this.tagValueFilterList.Count == 0)
            {
                this.tagValueFilterList = new List<TagValueFilter>();
                this.tagValueFilterList.Add(new TagValueFilter()
                {
                    TagName= tagName,
                    TagValueName = tagValueName,
                    TagValueId = tagValueId,
                    TagValueValue= tagValueValue
                });
            }
            else
            {
                if(!this.tagValueFilterList.Exists(x => x.TagName == tagName))
                {
                    this.tagValueFilterList.Add(new TagValueFilter()
                    {
                        TagName = tagName,
                        TagValueName = tagValueName,
                        TagValueId = tagValueId,
                        TagValueValue = tagValueValue
                    });
                }
                else
                {
                    if (tagValueId == -1)
                    {
                        for (int i = 0; i < this.tagValueFilterList.Count; i++)
                        {
                            if (this.tagValueFilterList[i].TagName == tagName)
                            {
                                this.tagValueFilterList.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        bool removeHappened = false;
                        for (int i = 0; i < this.tagValueFilterList.Count; i++)
                        {
                            if (this.tagValueFilterList[i].TagName == tagName && this.tagValueFilterList[i].TagValueId == tagValueId)
                            {
                                this.tagValueFilterList.RemoveAt(i);
                                removeHappened = true;
                            }
                        }
                        if (!removeHappened)
                        {
                            for (int i = 0; i < this.tagValueFilterList.Count; i++)
                            {
                                if (this.tagValueFilterList[i].TagName == tagName)
                                {
                                    this.tagValueFilterList.Insert(i, new TagValueFilter()
                                    {
                                        TagName = tagName,
                                        TagValueName = tagValueName,
                                        TagValueId = tagValueId,
                                        TagValueValue = tagValueValue
                                    });
                                    break;
                                }
                            }

                        }
                    }
                    
                }               
            }
           ((PlaylistView)this.playlistView).SetTagValueFilter(this.tagValueFilterList);

        }

        

        private void SetTagValueEvent(object sender, Messenger e)
        {
            if (!isSaving) { 
                if (!this.isFilterModeEnabled)
                {
                    this.SetTagValue(e.StringField1,e.StringField2,e.StringField3,e.IntegerField1, e.Rows);
                }
                else
                {
                    this.FilterByTagValue(e.StringField1, e.StringField2, e.StringField3, e.IntegerField1);
                }
            }
        }

        private void ClearTagValueEvent(object sender, Messenger e)
        {
            if (!isSaving)
            {
                if (!this.isFilterModeEnabled)
                {
                    if (this.tagList != null && this.tagList.Count > 0)
                    {
                        Tag currentTag = this.tagList.Find(x => x.Name == e.StringField1);
                        if (currentTag != null)
                        {
                            List<TagValue> tagValueList = this.tagDao.GetTagValuesByTagId(currentTag.Id);
                            if (tagValueList != null && tagValueList.Count > 0)
                            {
                                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                                {
                                    if (!this.isOnlyPlayingRowModeEnabled)
                                    {
                                        for (int i = e.Rows.Count - 1; i >= 0; i--)
                                        {
                                            if (e.Rows[i].Selected)
                                            {
                                                Model.Track track = this.trackDao.GetTrackWithTags(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                                if (track != null)
                                                {
                                                    foreach (TrackTagValue ttv in track.TrackTagValues)
                                                    {
                                                        if (ttv.TagId == currentTag.Id)
                                                        {

                                                            ttv.TagValueId = -1;
                                                            ttv.TagValueName = String.Empty;


                                                            this.trackDao.UpdateTrackTagValue(ttv);
                                                            break;
                                                        }
                                                    }
                                                }
                                                this.trackListTable.Rows[i][currentTag.Name] = "";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int currentTrackIdInPlaylist = this.mediaPlayerComponent.CurrentTrackIdInPlaylist;
                                        for (int i = e.Rows.Count - 1; i >= 0; i--)
                                        {
                                            int trackIdInPlaylist = Convert.ToInt32(this.trackListTable.Rows[i]["TrackIdInPlaylist"]);
                                            if (trackIdInPlaylist == currentTrackIdInPlaylist)
                                            {
                                                Model.Track track = this.trackDao.GetTrackWithTags(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                                if (track != null)
                                                {
                                                    foreach (TrackTagValue ttv in track.TrackTagValues)
                                                    {
                                                        if (ttv.TagId == currentTag.Id)
                                                        {

                                                            ttv.TagValueId = -1;
                                                            ttv.TagValueName = String.Empty;


                                                            this.trackDao.UpdateTrackTagValue(ttv);
                                                            break;
                                                        }
                                                    }
                                                }
                                                this.trackListTable.Rows[i][currentTag.Name] = "";
                                            }
                                        }
                                    }

                                    this.ReloadTrackList();
                                    this.TrackListChanged();


                                }

                            }
                        }
                    }
                }
                else
                {
                    for(int i = this.tagValueFilterList.Count - 1; i >=0; i--)
                    {
                        if (this.tagValueFilterList[i].TagName == e.StringField1)
                        {
                            this.tagValueFilterList.RemoveAt(i);
                        }
                    }

                    ((PlaylistView)this.playlistView).SetTagValueFilter(this.tagValueFilterList);
                }
            }
        }
        private void RemoveTagValueFilter(object sender, EventArgs e)
        {
            this.tagValueFilterList = new List<TagValueFilter>();

            ((PlaylistView)this.playlistView).SetTagValueFilter(this.tagValueFilterList);
        }

        private void ChangeFilterParameters(object sender, Messenger e)
        {
            DataView dv = this.trackListTable.DefaultView;
            String filterText = e.StringField1;
            String filterQuery = String.Empty;
            String filterQuery2 = String.Empty;
            List<String> filteringColumnNames = new List<String>();

            if (!String.IsNullOrEmpty(filterText) || this.isFilterModeEnabled)
            {
                if (!String.IsNullOrEmpty(filterText))
                {
                    filteringColumnNames.Add("Artist");
                    filteringColumnNames.Add("Title");
                    filteringColumnNames.Add("Path");

                    if (filteringColumnNames != null && filteringColumnNames.Count > 0)
                    {
                        foreach (String col in filteringColumnNames)
                        {
                            if (String.IsNullOrEmpty(filterQuery))
                            {
                                filterQuery = col + " LIKE '%" + filterText + "%' ";
                            }
                            else
                            {
                                filterQuery = filterQuery + " OR " + col + " LIKE '%" + filterText + "%' ";
                            }
                        }
                        filterQuery = "(" + filterQuery + ")";
                    }
                }

                List<String> processedTags = new List<String>();

                if (this.tagValueFilterList != null && this.tagValueFilterList.Count > 0)
                {
                    String lastTagName = String.Empty;

                    for (int i = 0; i < this.tagValueFilterList.Count; i++)
                    {
                        

                        if (String.IsNullOrEmpty(filterQuery2))
                        {
                            if (this.tagValueFilterList[i].TagValueValue != "-1")
                            {
                                lastTagName = this.tagValueFilterList[i].TagName;
                                filterQuery2 += "(" + lastTagName + " = " + this.tagValueFilterList[i].TagValueValue +" ";
                            }
                            else
                            {
                                lastTagName = this.tagValueFilterList[i].TagName;
                                filterQuery2 += "(" + lastTagName + "TagValueId=" + this.tagValueFilterList[i].TagValueId;
                            }
                            
                        }
                        else
                        {
                            if (this.tagValueFilterList[i].TagValueValue != "-1")
                            {
                                if (lastTagName != this.tagValueFilterList[i].TagName)
                                {
                                    lastTagName = this.tagValueFilterList[i].TagName;
                                    filterQuery2 += ") AND (";
                                }
                                filterQuery2 += lastTagName + " = " + this.tagValueFilterList[i].TagValueValue + " ";
                            }
                            else
                            {
                                if (lastTagName != this.tagValueFilterList[i].TagName)
                                {
                                    lastTagName = this.tagValueFilterList[i].TagName;
                                    filterQuery2 += ") AND (";
                                }
                                else
                                {
                                    filterQuery2 += " OR ";
                                }
                                filterQuery2 += lastTagName + "TagValueId=" + this.tagValueFilterList[i].TagValueId;
                            }
                                
                        }

                    }
                    filterQuery2 = "(" + filterQuery2 + "))";
                }

                if (!String.IsNullOrEmpty(filterQuery) && !String.IsNullOrEmpty(filterQuery2))
                {
                    dv.RowFilter = filterQuery + " AND " + filterQuery2;
                }
                else if (!String.IsNullOrEmpty(filterQuery) && String.IsNullOrEmpty(filterQuery2))
                {
                    dv.RowFilter = filterQuery;
                }
                else if (String.IsNullOrEmpty(filterQuery) && !String.IsNullOrEmpty(filterQuery2))
                {
                    dv.RowFilter = filterQuery2;
                }
                else
                {
                    dv.RowFilter = "";
                }

                DataTable filteredDT = dv.ToTable();
                this.filteredTrackListTable = filteredDT;

                ((PlaylistView)this.playlistView).ToggleTracklistSelection(false);

                this.trackListBindingSource.DataSource = this.filteredTrackListTable;
                this.InitializeTrackList(this.filteredTrackListTable, this.mediaPlayerComponent.CurrentTrackIdInPlaylist);

                ((PlaylistView)this.playlistView).ToggleTracklistSelection(true);
            }

            this.mediaPlayerComponent.SetWorkingTable(this.filteredTrackListTable);
        }


     //   KeyTrainingDataGenerator keyModelTrainer = new KeyTrainingDataGenerator();

       /* private void AddToKeyDetectorEvent(object sender, ListEventArgs e)
        {
            this.keyModelTrainer = KeyTrainingDataGenerator.GetInstance();

            List<Track> tracks = new List<Track>();

            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                for (int i = e.Rows.Count - 1; i >= 0; i--)
                {
                    if (e.Rows[i].Selected)
                    {
                        int trackId = (int)e.Rows[i].Cells["Id"].Value;
                        Model.Track track = this.trackDao.GetTrackWithTags(trackId, this.tagList);
                        tracks.Add(track);
                    }
                }

                if (tracks != null && tracks.Count > 0)
                {
                    ConcurrentBag<Track> concurrentTracks = new ConcurrentBag<Track>(tracks);

                    int sum = 0;
                    int totalTracks = tracks.Count;

                    Parallel.ForEach(concurrentTracks, track =>
                    {
                        if (track.TrackTagValues.Exists(x => x.TagName == "Key"))
                        {
                            String trainingKey = track.TrackTagValues.Find(x => x.TagName == "Key").TagValueName;
                            if (!String.IsNullOrEmpty(trainingKey))
                            {
                                this.keyModelTrainer.AddTrack(track.Path, trainingKey);
                                Interlocked.Increment(ref sum);

                                Console.WriteLine($"Add to model: {sum}/{totalTracks} ready");
                            }
                        }
                    });

                     ((PlaylistView)this.playlistView).UpdateLog("ADDED TO MODEL");

                    if (this.keyModelTrainer.tracks != null)
                        ((PlaylistView)this.playlistView).UpdateTrackCountInModel(this.keyModelTrainer.tracks.Count);
                }
            }
        }*/

        private void AddToKeyDetectorEvent(object sender, Messenger e)
        {
            //this.keyModelTrainer = KeyTrainingDataGenerator.GetInstance();

           /* List<Track> tracks = new List<Track>();

            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                for (int i = e.Rows.Count - 1; i >= 0; i--)
                {
                    if (e.Rows[i].Selected)
                    {
                        int trackId = (int)e.Rows[i].Cells["Id"].Value;
                        Model.Track track = this.trackDao.GetTrackWithTags(trackId, this.tagList);
                        tracks.Add(track);
                    }
                }

                if (tracks != null && tracks.Count > 0)
                {
                    int batchSize = 20; // Adjust the batch size as needed
                    int totalTracks = tracks.Count;
                    int sum = 0;

                    for (int i = 0; i < totalTracks; i += batchSize)
                    {
                        var batch = tracks.Skip(i).Take(batchSize).ToList();
                        ConcurrentBag<Track> concurrentTracks = new ConcurrentBag<Track>(batch);

                        Parallel.ForEach(concurrentTracks, track =>
                        {
                            if (track.TrackTagValues.Exists(x => x.TagName == "Key"))
                            {
                                String trainingKey = track.TrackTagValues.Find(x => x.TagName == "Key").TagValueName;
                                if (!String.IsNullOrEmpty(trainingKey))
                                {
                                    this.keyModelTrainer.AddTrack(track.Path, trainingKey);
                                    Interlocked.Increment(ref sum);

                                    Console.WriteLine($"Add to model: {sum}/{totalTracks} ready");
                                }
                            }
                        });

                        // Write the current batch to the CSV file
                        this.keyModelTrainer.WriteToCsv("keyTrainingData.csv");

                        ((PlaylistView)this.playlistView).UpdateLog($"Batch {i / batchSize + 1} added to model");

                        if (this.keyModelTrainer.tracks != null)
                            ((PlaylistView)this.playlistView).UpdateTrackCountInModel(this.keyModelTrainer.tracks.Count);
                    }
                }
            }*/
        }

        private void CreateModelEvent(object sender, EventArgs e)
        {
           // this.keyModelTrainer.GenerateCsv("keyTrainingData.csv");
           // ((PlaylistView)this.playlistView).UpdateTrackCountInModel(0);
           // ((PlaylistView)this.playlistView).UpdateLog("MODEL CREATED");
        }

        //KeyDetector detector = new KeyDetector();

        private void TrainKeyDetectorEvent(object sender, EventArgs e)
        {
           // ((PlaylistView)this.playlistView).UpdateLog("MODEL TRAINING");
           // KeyDetector.GetInstance().TrainKeyDetector("keyTrainingData.csv");
           // ((PlaylistView)this.playlistView).UpdateLog("MODEL READY");
        }
        private void DetectKeyEvent(object sender, Messenger e)
        {



           /* List<Track> tracks = new List<Track>();

            Tag currentTag = this.tagList.Find(x => x.Name == "Key");
            List<TagValue> tagValueList = this.tagDao.GetTagValuesByTagId(currentTag.Id);
            Dictionary<int, String> idAndKey = new Dictionary<int, String>();

            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                for (int i = e.Rows.Count - 1; i >= 0; i--)
                {
                    if (e.Rows[i].Selected)
                    {
                        int trackId = (int)e.Rows[i].Cells["Id"].Value;
                        Model.Track track = this.trackDao.GetTrackWithTags(trackId, this.tagList);
                        tracks.Add(track);
                    }
                }

                if (tracks != null && tracks.Count > 0)
                {
                    ConcurrentBag<Track> concurrentTracks = new ConcurrentBag<Track>(tracks);

                    ((PlaylistView)this.playlistView).UpdateLog("KEY DETECTING");
                    Parallel.ForEach(concurrentTracks, track =>
                    {
                        String predictedKey = KeyDetector.GetInstance().DetectKey(track.Path);
                        if (predictedKey != "Unknown")
                            idAndKey.Add(track.Id, predictedKey);
                    });

                    for (int i = e.Rows.Count - 1; i >= 0; i--)
                    {
                        if (e.Rows[i].Selected)
                        {
                            int trackId = (int)e.Rows[i].Cells["Id"].Value;
                            if (idAndKey.ContainsKey(trackId))
                            {

                                Track track = tracks.Find(x => x.Id == trackId);
                                if (track != null)
                                {
                                    TagValue tv = tagValueList.Find(x => x.Name.Equals(idAndKey[trackId]));
                                    if (tv != null)
                                    {
                                        foreach (TrackTagValue ttv in track.TrackTagValues)
                                        {
                                            if (ttv.TagId == currentTag.Id)
                                            {
                                                ttv.TagValueId = tv.Id;
                                                ttv.TagValueName = tv.Name;
                                                this.trackDao.UpdateTrackTagValue(ttv);
                                                break;
                                            }
                                        }

                                    }
                                    this.trackListTable.Rows[i][currentTag.Name] = tv.Name;
                                    this.trackListTable.Rows[i][currentTag.Name + "TagValueId"] = tv.Id;
                                }

                                
                            }
                        }
                    }

                    ((PlaylistView)this.playlistView).UpdateLog("KEY DETECTION READY");

                }

            }*/

        }
    }
}
