using FlacLibSharp;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.IViews;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Type = System.Type;

namespace MitoPlayer_2024.Presenters
{

    public class SelectorPresenter
    {
        private ISelectorView selectorView { get; set; }
        private ITrackDao trackDao { get; set; }
        private ITagDao tagDao { get; set; }
        private ISettingDao settingDao { get; set; }

        private bool isTrackListInitializing = true;
        private bool isSelectorTrackListInitializing = true;

        public SelectorPresenter(ISelectorView view, ITrackDao trackDao, ITagDao tagValueDao, ISettingDao settingDao)
        {
            //INITIALIZE
            this.selectorView = view;
            this.trackDao = trackDao;
            this.tagDao = tagValueDao;
            this.settingDao = settingDao;

            //MEDIA PLAYER
            this.selectorView.SetCurrentTrackEvent += SetCurrentTrackEvent;
            this.selectorView.PlayTrackEvent += PlayTrackEvent;
            this.selectorView.PauseTrackEvent += PauseTrackEvent;
            this.selectorView.StopTrackEvent += StopTrackEvent;

            //TRACKLIST
           // this.selectorView.SetTrackListToActive += SelectorView_SetTrackListToActive;
            this.selectorView.OrderByColumnEvent += OrderByColumnEvent;
            this.selectorView.DeleteTracksEvent += DeleteTracksEvent;
            this.selectorView.InternalDragAndDropIntoTracklistEvent += InternalDragAndDropIntoTracklistEvent;
            this.selectorView.ExternalDragAndDropIntoTracklistEvent += ExternalDragAndDropIntoTracklistEvent;
            this.selectorView.MoveTracklistRowsEvent += MoveTracklistRowsEvent;
            this.selectorView.SaveTrackListEvent += SaveTrackListEvent;

            //SELECTOR TRACKLIST
           // this.selectorView.SetSelectorToActive += SelectorView_SetSelectorToActive;
            this.selectorView.OrderSelectorByColumnEvent += OrderSelectorByColumnEvent;
            this.selectorView.InternalDragAndDropIntoSelectorTracklistEvent += InternalDragAndDropIntoSelectorTracklistEvent;
            this.selectorView.ExternalDragAndDropIntoSelectorTracklistEvent += ExternalDragAndDropIntoSelectorTracklistEvent;
            this.selectorView.MoveSelectorTracklistRowsEvent += MoveSelectorTracklistRowsEvent;
            this.selectorView.ChangePlaylistSource += SelectorView_ChangePlaylistSource;
            this.selectorView.ChangeBestFit += SelectorView_ChangeBestFit;
            this.selectorView.ChangeResultSize += SelectorView_ChangeResultSize;
            this.selectorView.ChangeTrackMoveMode += SelectorView_ChangeTrackMoveMode;
            this.selectorView.SubtractTracksFromPlaylist += SelectorView_SubtractTracksFromPlaylist; ;

            //PLAYLIST
            this.selectorView.CreatePlaylist += CreatePlaylist;
            this.selectorView.EditPlaylist += EditPlaylist;
            this.selectorView.LoadPlaylistEvent += LoadPlaylistEvent;
            this.selectorView.LoadPlaylistIntoTracklistEvent += SelectorView_LoadPlaylistIntoTracklistEvent;
            this.selectorView.LoadPlaylistIntoSelectorEvent += SelectorView_LoadPlaylistIntoSelectorEvent;
            this.selectorView.MovePlaylistEvent += MovePlaylistEvent;
            this.selectorView.DeletePlaylistEvent += DeletePlaylistEvent;
            this.selectorView.ExportToM3UEvent += ExportToM3UEvent;
            this.selectorView.ExportToTXTEvent += ExportToTXTEvent;
            this.selectorView.ExportToDirectoryEvent += ExportToDirectoryEvent;
            this.selectorView.MovePlaylistRowEvent += MovePlaylistRowEvent;
            this.selectorView.DisplayPlaylistListEvent += DisplayPlaylistListEvent;
            this.selectorView.InternalDragAndDropIntoPlaylistEvent += InternalDragAndDropIntoPlaylistEvent;
            this.selectorView.ExternalDragAndDropIntoPlaylistEvent += ExternalDragAndDropIntoPlaylistEvent;

            //TAG EDITOR
            this.selectorView.SetTagValueEvent += SetTagValueEvent;
            this.selectorView.ClearTagValueEvent += ClearTagValueEvent;
            this.selectorView.EnableFilterModeEvent += EnableFilterModeEvent;
            this.selectorView.ChangeFilterParametersEvent += ChangeFilterParameters;
            this.selectorView.RemoveTagValueFilter += RemoveTagValueFilter;

        }

       


        #region INITIALIZE
        public void Initialize(MediaPlayerComponent mediaPlayer)
        {
            isTrackListInitializing = true;
            isSelectorTrackListInitializing = true;

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

                    this.InitializeActiveTracklist();

                    this.InitializePlaylistListColumns();
                    this.InitializePlaylistListRows();
                    this.InitializePlaylistList();

                    this.InitializeShortTrackColouring();
                   

                    loadingDialog.SetProcessDescription("Initialize tracklist grid structure...");
                    loadingDialog.Refresh();

                    this.InitializeTracklistColumns();
                    this.InitializeTrackListRows(this.trackListTable, null);
                    this.InitializeTrackList(this.trackListTable);

                    loadingDialog.SetProcessDescription("Initialize selector tracklist grid structure...");
                    loadingDialog.Refresh();

                    this.InitializeSelectorTracklistColumns();
                    this.InitializeSelectorTrackListRows(this.selectorTrackListTable, null);
                    this.InitializeSelectorTrackList(this.selectorTrackListTable);

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

                    this.InitializeTimers();

                    this.InitializeSelectorOptions();

                    isPlaylistDisplayed = true;
                    //currentPlaylistId = 1;
                    //currentSelectorPlaylistId = 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            isTrackListInitializing = false;
            isSelectorTrackListInitializing = false;

        }
        #endregion

        #region INITALIZE - TAGS AND TAGVALUES
        private List<Tag> tagList { get; set; }
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        private void InitializeTagsAndTagValues()
        {
            this.tagValueDictionary = new Dictionary<String, Dictionary<String, Color>>();

            List<Tag> tagList = this.tagDao.GetAllTag().Value;
            if (tagList != null && tagList.Count > 0)
            {
                this.tagList = tagList;

                List<TagValue> tagValueList = new List<TagValue>();

                foreach (Tag tag in this.tagList)
                {
                    Dictionary<String, Color> tvDic = new Dictionary<String, Color>();

                    tagValueList = this.tagDao.GetTagValuesByTagId(tag.Id).Value;
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
            this.selectorView.InitializeTagsAndTagValues(this.tagList, this.tagValueDictionary);
        }
        #endregion

        #region INITIALIZE - PLAYLIST
        private BindingSource playlistListBindingSource { get; set; }
        private DataTable playlistListTable { get; set; }
        private bool[] playlistListColumnVisibilityArray { get; set; }
        private int currentPlaylistId { get; set; }
        private int currentSelectorPlaylistId { get; set; }
        /// PLAYLIST GRID
        /// Retrieve the columns (track properties)
        /// Add the columns to the playlist DataTable
        /// Retrieve the visibility of the columns into a boolean array
        /// Get the current playlist ID from the settings
        /// Check if the current playlist ID is present in the current list
        /// Bind the playlist, passing the visibility array and the current playlist
        private void InitializePlaylistListColumns()
        {
            this.currentPlaylistId = 1;
            this.currentSelectorPlaylistId = 1;
            this.playlistListBindingSource = new BindingSource();
            this.playlistListTable = new DataTable();

            List<TrackProperty> tpList = new List<TrackProperty>();
            tpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.PlaylistColumns.ToString()).Value;
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

            List<Playlist> playlistList = this.trackDao.GetAllPlaylist().Value;
            if (playlistList != null && playlistList.Count > 0)
            {
                foreach (Playlist playlist in playlistList)
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
            this.currentPlaylistId = this.settingDao.GetIntegerSetting(Settings.CurrentPlaylistIdInSelector.ToString()).Value;
            this.currentSelectorPlaylistId = this.settingDao.GetIntegerSetting(Settings.CurrentSelectorPlaylistId.ToString()).Value;

            Playlist trackList = this.trackDao.GetPlaylist(this.currentPlaylistId).Value;
            Playlist selector = this.trackDao.GetPlaylist(this.currentSelectorPlaylistId).Value;

            DataTableModel model = new DataTableModel()
            {
                BindingSource = this.playlistListBindingSource,
                ColumnVisibilityArray = this.playlistListColumnVisibilityArray,
                CurrentObjectId = this.isTrackListActive ? this.currentPlaylistId : this.currentSelectorPlaylistId,
                CurrentTracklistName = trackList?.Name,
                CurrentSelectorName = selector?.Name
            };

            this.selectorView.InitializePlaylistList(model);
        }
        private void ReloadPlaylist()
        {
            this.InitializePlaylistList();
        }
        #endregion

        #region INITIALIZE - ACTIVE TRACKLIST
        private bool isTrackListActive{ get; set; }
        private void InitializeActiveTracklist()
        {
            ResultOrError<bool?> isActive = this.settingDao.GetBooleanSetting(Settings.IsTrackListActive.ToString());
            if (isActive.Value == null)
            {
                this.isTrackListActive = true;
                this.settingDao.SetBooleanSetting(Settings.IsTrackListActive.ToString(), this.isTrackListActive);
            }
            else
            {
                this.isTrackListActive = isActive.Value.Value;
            }
            ((SelectorView)this.selectorView).InitializeTracklistActiveButton(this.isTrackListActive);
        }
       /* private void SelectorView_SetTrackListToActive(object sender, Messenger e)
        {
            this.isTrackListActive = true;
            this.settingDao.SetBooleanSetting(Settings.IsTrackListActive.ToString(), this.isTrackListActive);

            LoadPlaylistEvent(sender, e);
        }
        private void SelectorView_SetSelectorToActive(object sender, Messenger e)
        {
            this.isTrackListActive = false;
            this.settingDao.SetBooleanSetting(Settings.IsTrackListActive.ToString(), this.isTrackListActive);

            if (isPlaylistDisplayed)
            {
                LoadPlaylistEvent(sender, e);
            }
            else
            {
                LoadTracksFromDatabase();
            }
            
        }*/
        #endregion

        #region INITALIZE - TRACKLIST
        private BindingSource trackListBindingSource { get; set; }
        private DataTable trackListTable { get; set; }
        private List<Model.Track> tracklist { get; set; }
        private Dictionary<string, int> columnOrderStates { get; set; }
        private int[] trackColumnDisplayIndexArray { get; set; }
        private bool[] trackColumnVisibilityArray { get; set; }
        /// TRACKLIST GRID
        /// Retrieve the columns (track properties)
        /// Retrieve the sorting IDs into an integer array
        /// Retrieve the column order into a string-int dictionary
        /// Add the columns to the tracklist DataTable
        /// Retrieve the visibility of the columns into a boolean array
        /// Get the current track ID in the playlist from the settings
        /// Check if the current track ID in the playlist is present in the current list
        /// Bind the tracklist, passing the visibility array, the order dictionary, and the current track ID in the playlist
        private void InitializeTracklistColumns()
        {
            this.trackListBindingSource = new BindingSource();
            this.trackListTable = new DataTable();
            this.columnOrderStates = new Dictionary<string, int>();

            List<TrackProperty> tpList = new List<TrackProperty>();
            tpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString()).Value;
            if (tpList != null && tpList.Count > 0)
            {
                this.trackColumnDisplayIndexArray = tpList.Select(x => x.SortingId).ToArray();

                tpList = tpList.OrderBy(x => x.SortingId).ToList();
                for (int i = 0; i <= tpList.Count - 1; i++)
                {
                    this.columnOrderStates.Add(tpList[i].Name, -1);
                    this.trackListTable.Columns.Add(tpList[i].Name, Type.GetType(tpList[i].Type));
                }

                this.trackColumnVisibilityArray = tpList.Select(x => x.IsEnabled).ToArray();
            }
        }
        private void InitializeTrackListRows(DataTable tracklistTable, List<Model.Track> tList)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
                loadingDialog.SetProcessDescription("Reload tracklist...");
                loadingDialog.Refresh();


                tracklistTable.Clear();

                if (tList == null)
                {
                    List<Playlist> plsList = this.trackDao.GetAllPlaylist().Value;
                    Playlist actualPlaylist = null;
                    if (plsList != null && plsList.Count > 0)
                    {
                        actualPlaylist = plsList.Find(x => x.Id == this.currentPlaylistId);
                    }
                    if(actualPlaylist != null)
                    {
                        tList = this.trackDao.GetTracklistWithTagsByPlaylistId(actualPlaylist.Id, this.tagList).Value;
                        //tList = tList.Take(resultSize).ToList();

                        this.tracklist = tList;
                    }
                    
                }

                if (tList != null && tList.Count > 0)
                {
                    try
                    {

                        foreach (Model.Track track in tList)
                        {
                            if (track.IsMissing && File.Exists(track.Path))
                            {
                                track.IsMissing = false;
                                this.trackDao.UpdateTrack(track);
                            }
                            if (!track.IsMissing && !File.Exists(track.Path))
                            {
                                track.IsMissing = true;
                                this.trackDao.UpdateTrack(track);
                            }

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

                ((SelectorView)this.selectorView).UpdateTrackCountAndLength(this.currentPlaylistId, true);
            }
        }

        private void UpdateTrackCountAndLength()
        {
            if (this.isTrackListActive)
            {
                ((SelectorView)this.selectorView).UpdateTrackCountAndLength(this.currentPlaylistId, true);
            }
            else
            {
                ((SelectorView)this.selectorView).UpdateTrackCountAndLength(this.currentSelectorPlaylistId, false);
            }
               
        }

        private void InitializeTrackList(DataTable trackListTable, int currentTrackIdInPlaylist = -1)
        {
            if (!isTrackListInitializing)
                ((SelectorView)this.selectorView).ToggleTracklistSelection(false);

            this.trackListBindingSource.DataSource = trackListTable;

            DataTableModel model = new DataTableModel()
            {
                BindingSource = this.trackListBindingSource,
                ColumnVisibilityArray = this.trackColumnVisibilityArray,
                ColumnDisplayIndexArray = this.trackColumnDisplayIndexArray,
                CurrentTrackIdInPlaylist = currentTrackIdInPlaylist
            };

            this.selectorView.InitializeTrackList(model);

            if (!isTrackListInitializing)
                ((SelectorView)this.selectorView).ToggleTracklistSelection(true);

            ((SelectorView)this.selectorView).UpdateTrackCountAndLength(this.currentPlaylistId, true);

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

                    /// Start a number, load another list, and then reload the original one, 
                    /// we should mark the number
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
            this.InitializeTrackList(this.trackListTable, currentTrackIdInPlaylist);
        }
        private void InitializeShortTrackColouring()
        {
            bool isColouringEnabled = this.settingDao.GetBooleanSetting(Settings.IsShortTrackColouringEnabled.ToString()).Value.Value;
            TimeSpan timeSpan = new TimeSpan();

            if (isColouringEnabled)
            {
                decimal shortTrackColouringThreshold = this.settingDao.GetDecimalSetting(Settings.ShortTrackColouringThreshold.ToString()).Value;
                timeSpan = TimeSpan.FromMinutes((double)shortTrackColouringThreshold);
            }

           ((SelectorView)this.selectorView).InitializeShortTrackColouring(isColouringEnabled, timeSpan);
            ((SelectorView)this.selectorView).InitializeShortTrackColouringInSelector(isColouringEnabled, timeSpan);
        }
        #endregion

        #region INITALIZE - SELECTOR TRACKLIST
        private BindingSource selectorTrackListBindingSource { get; set; }
        private DataTable selectorTrackListTable { get; set; }
        private List<Model.Track> selectorTracklist { get; set; }
        private DataTable filteredSelectorTrackListTable { get; set; }
        private Dictionary<string, int> selectorColumnOrderStates { get; set; }
        private int[] selectorTrackColumnDisplayIndexArray { get; set; }
        private bool[] selectorTrackColumnVisibilityArray { get; set; }
        private void InitializeSelectorTracklistColumns()
        {
            this.selectorTrackListBindingSource = new BindingSource();
            this.selectorTrackListTable = new DataTable();
            this.filteredSelectorTrackListTable = new DataTable();
            this.selectorColumnOrderStates = new Dictionary<string, int>();

            List<TrackProperty> tpList = new List<TrackProperty>();
            tpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString()).Value;
            if (tpList != null && tpList.Count > 0)
            {
                this.selectorTrackColumnDisplayIndexArray = tpList.Select(x => x.SortingId).ToArray();

                tpList = tpList.OrderBy(x => x.SortingId).ToList();
                for (int i = 0; i <= tpList.Count - 1; i++)
                {
                    this.selectorColumnOrderStates.Add(tpList[i].Name, -1);
                    this.selectorTrackListTable.Columns.Add(tpList[i].Name, Type.GetType(tpList[i].Type));
                    this.filteredSelectorTrackListTable.Columns.Add(tpList[i].Name, Type.GetType(tpList[i].Type));
                }

                this.selectorTrackColumnVisibilityArray = tpList.Select(x => x.IsEnabled).ToArray();
            }
        }
        private void InitializeSelectorTrackListRows(DataTable tracklistTable, List<Model.Track> tList)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
                loadingDialog.SetProcessDescription("Reload selector tracklist...");
                loadingDialog.Refresh();

                tracklistTable.Clear();

                if (tList == null)
                {
                    List<Playlist> plsList = this.trackDao.GetAllPlaylist().Value;
                    Playlist actualPlaylist = null;
                    if (plsList != null && plsList.Count > 0)
                    {
                        actualPlaylist = plsList.Find(x => x.Id == this.currentSelectorPlaylistId);
                    }
                    if(actualPlaylist!= null)
                    {
                        tList = this.trackDao.GetTracklistWithTagsByPlaylistId(actualPlaylist.Id, this.tagList).Value;
                        //tList = tList.Take(resultSize).ToList();
                        this.selectorTracklist = tList;
                    }
                }

                if (tList != null && tList.Count > 0)
                {
                    try
                    {

                        foreach (Model.Track track in tList)
                        {
                            if (track.IsMissing && File.Exists(track.Path))
                            {
                                track.IsMissing = false;
                                this.trackDao.UpdateTrack(track);
                            }
                            if (!track.IsMissing && !File.Exists(track.Path))
                            {
                                track.IsMissing = true;
                                this.trackDao.UpdateTrack(track);
                            }

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

                this.UpdateTrackCountAndLength();
            }
        }
        private void InitializeSelectorTrackList(DataTable trackListTable, int currentTrackIdInPlaylist = -1)
        {
            if (!isSelectorTrackListInitializing)
                ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(false);

            this.selectorTrackListBindingSource.DataSource = trackListTable;

            DataTableModel model = new DataTableModel()
            {
                BindingSource = this.selectorTrackListBindingSource,
                ColumnVisibilityArray = this.selectorTrackColumnVisibilityArray,
                ColumnDisplayIndexArray = this.selectorTrackColumnDisplayIndexArray,
                CurrentTrackIdInPlaylist = currentTrackIdInPlaylist
            };

            this.selectorView.InitializeSelectorTrackList(model);

            if (!isSelectorTrackListInitializing)
                ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(true);

            this.UpdateTrackCountAndLength();
        }
        private void ReloadSelectorTrackList()
        {
            int currentTrackIdInPlaylist = -1;

            
            if(this.tagValueFilterList != null && this.tagValueFilterList.Count > 0)
            {
                if (this.mediaPlayerComponent != null)
                {
                    if (this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying ||
                       this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
                    {
                        currentTrackIdInPlaylist = this.mediaPlayerComponent.CurrentTrackIdInPlaylist;

                        /// Start a number, load another list, and then reload the original one, 
                        /// we should mark the number
                        int trackIdInPlaylist = -1;
                        for (int i = 0; i <= this.filteredSelectorTrackListTable.Rows.Count - 1; i++)
                        {
                            trackIdInPlaylist = Convert.ToInt32(this.filteredSelectorTrackListTable.Rows[i]["TrackIdInPlaylist"]);
                            if (trackIdInPlaylist == currentTrackIdInPlaylist)
                            {
                                currentTrackIdInPlaylist = this.mediaPlayerComponent.CurrentTrackIdInPlaylist;
                                break;
                            }
                        }
                    }
                }

                this.InitializeSelectorTrackList(this.filteredSelectorTrackListTable, currentTrackIdInPlaylist);
            }
            else
            {
                if (this.mediaPlayerComponent != null)
                {
                    if (this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying ||
                       this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
                    {
                        currentTrackIdInPlaylist = this.mediaPlayerComponent.CurrentTrackIdInPlaylist;

                        /// Start a number, load another list, and then reload the original one, 
                        /// we should mark the number
                        int trackIdInPlaylist = -1;
                        for (int i = 0; i <= this.selectorTrackListTable.Rows.Count - 1; i++)
                        {
                            trackIdInPlaylist = Convert.ToInt32(this.selectorTrackListTable.Rows[i]["TrackIdInPlaylist"]);
                            if (trackIdInPlaylist == currentTrackIdInPlaylist)
                            {
                                currentTrackIdInPlaylist = this.mediaPlayerComponent.CurrentTrackIdInPlaylist;
                                break;
                            }
                        }
                    }
                }

                this.InitializeSelectorTrackList(this.selectorTrackListTable, currentTrackIdInPlaylist);
            }
            
        }

        private bool isPlaylistDisplayed = true;
        private void SelectorView_ChangePlaylistSource(object sender, Messenger e)
        {
            isPlaylistDisplayed = e.BooleanField1;

            if (e.BooleanField1)
            {
                this.isTrackListActive = false;
                this.settingDao.SetBooleanSetting(Settings.IsTrackListActive.ToString(), this.isTrackListActive);

                //LoadPlaylistEvent(sender, e);
                SelectorView_LoadPlaylistIntoSelectorEvent(sender, e);
            }
            else
            {
                LoadTracksFromDatabase();
            }
        }
        private void SelectorView_ChangeBestFit(object sender, Messenger e)
        {
            isMoveToTracklist = e.BooleanField1;
        }
        private void LoadTracksFromDatabase()
        {
            List<Track> trackList = trackDao.GetTracklistWithTagsFromDatabaseByParameters(this.textFilter, this.tagValueFilterList, this.tagList, this.resultSize).Value;
            if(trackList != null && trackList.Count > 0)
            {
                LoadSelectorTrackList(trackList);
            }
        }
        private int resultSize = 50;
        private void SelectorView_ChangeResultSize(object sender, Messenger e)
        {
            resultSize = e.IntegerField1;

            e.IntegerField1 = this.currentSelectorPlaylistId;

            LoadTracksFromDatabase();

           /* if (isPlaylistDisplayed)
            {
                LoadPlaylistEvent(sender, e);
            }
            else
            {
                LoadTracksFromDatabase();
            }*/
        }
        private bool isMoveToTracklist = false;
        private void SelectorView_ChangeTrackMoveMode(object sender, Messenger e)
        {
            isMoveToTracklist = e.BooleanField1;
        }
        private void SelectorView_SubtractTracksFromPlaylist(object sender, EventArgs e)
        {
            foreach(DataRow row in this.trackListTable.Rows)
            {
                int trackId = Convert.ToInt32(row["ID"]);
                this.trackDao.DeletePlaylistContentByPlaylistIdAndTrackId(this.currentSelectorPlaylistId, trackId);
            }

            SelectorView_LoadPlaylistIntoSelectorEvent(sender, new Messenger { IntegerField1 = this.currentSelectorPlaylistId });

        }
        private void LoadSelectorTrackList(List<Track> trackList)
        {
            DataTable dataTable = new DataTable();
            if(tagValueFilterList != null&& tagValueFilterList.Count > 0)
            {
                dataTable = filteredSelectorTrackListTable;
            }
            else
            {
                dataTable = selectorTrackListTable;
            }

            if (trackList != null && trackList.Count > 0)
            {
                if (!isSelectorTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(false);

                dataTable.Clear();

                int i = 0;

                foreach (Track track in trackList)
                {
                    String length = this.LengthToString(track.Length);

                    DataRow dataRow = dataTable.NewRow();
                    dataRow["Id"] = track.Id;
                    dataRow["Album"] = track.Album;
                    dataRow["Artist"] = track.Artist;
                    dataRow["Title"] = track.Title;
                    dataRow["Year"] = track.Year;
                    dataRow["Length"] = length;
                    dataRow["IsMissing"] = track.IsMissing;
                    dataRow["Path"] = track.Path;
                    dataRow["FileName"] = track.FileName;
                    dataRow["OrderInList"] = i++;
                    dataRow["TrackIdInPlaylist"] = this.trackDao.GetNextSmallestTrackIdInPlaylist().Value;
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
                    dataTable.Rows.InsertAt(dataRow, 0);
                }

                if (!isSelectorTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(true);


                this.ReloadSelectorTrackList();
                this.SelectorTrackListChanged();

                this.UpdateTrackCountAndLength();
            }
        }
        #endregion

        #region INITIALIZE - TAG COMPONENT AND PLAYLIST VISIBILITY
        private void InitializeTagComponent()
        {
            List<List<TagValue>> tagValueListContainer = new List<List<TagValue>>();
            foreach (Tag tag in this.tagList)
            {
                List<TagValue> tagValues = new List<TagValue>();
                tagValues = this.tagDao.GetTagValuesByTagId(tag.Id).Value;
                tagValueListContainer.Add(tagValues);
            }

            ((SelectorView)this.selectorView).InitializeTagComponent(
                this.tagList,
                tagValueListContainer);

            ((SelectorView)this.selectorView).InitializeDisplayTagComponent();
        }
        private bool isPlaylistListDisplayed { get; set; }
        private void InitializedPlaylistList()
        {
            this.isPlaylistListDisplayed = this.settingDao.GetBooleanSetting(Settings.IsPlaylistListDisplayed.ToString()).Value.Value;
            ((SelectorView)this.selectorView).InitializeDisplayPlaylistList(this.isPlaylistListDisplayed);
        }
        #endregion

        #region INITIALIZE - OTHER
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
            int volume = this.settingDao.GetIntegerSetting(Settings.Volume.ToString()).Value;
            bool isMuted = this.settingDao.GetBooleanSetting(Settings.IsMuteEnabled.ToString()).Value.Value;

            if (volume == -1)
                volume = 50;

            if (isMuted)
                volume = 0;

            this.selectorView.SetVolume(volume);
            this.selectorView.SetMuted(isMuted);

            this.mediaPlayerComponent.MediaPlayer.settings.volume = volume;
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
        private void InitializeSelectorOptions()
        {

        }
        #endregion

        #region INITIALIZE TIMERS
        private System.Windows.Forms.Timer playNextTrackTimer;
        private void InitializeTimers()
        {
            playNextTrackTimer = new System.Windows.Forms.Timer();
            playNextTrackTimer.Interval = 100; // 100 milliseconds delay
            playNextTrackTimer.Tick += PlayNextTrackTimer_Tick;
        }
        #endregion

        #region DATATABLE SAVES - PLAYLIST
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
                    this.selectorView.ChangeSaveStatus(false);
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
        #endregion

        #region DATATABLE SAVES - TRACKLIST
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

            bool isLogMessageDisplayed = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value.Value;
            decimal logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString()).Value;
            if (isLogMessageDisplayed)
            {
                String message = "";
                if (!String.IsNullOrEmpty(errorMessage))
                {
                    message = errorMessage;
                }
                else
                {
                    message = "Playlist saved successfully!";
                }
                this.selectorView.DisplayLog(message, logMessageDisplayTime);
            }

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

            bool isLogMessageDisplayed = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value.Value;
            decimal logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString()).Value;
            if (isLogMessageDisplayed)
            {
                String message = "";
                if (String.IsNullOrEmpty(errorMessage))
                {
                    message = errorMessage;
                }
                else
                {
                    message = "Playlist saved successfully!";
                }
                this.selectorView.DisplayLog(message, logMessageDisplayTime);
            }
        }
        private bool isSaving = false;
        private bool isTrackListChanged = false;
        private void TrackListChanged()
        {
            this.isTrackListChanged = true;

            this.selectorView.ChangeSaveButtonColor(true);
            this.mediaPlayerComponent.SetWorkingTable(this.trackListTable);
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
                    readyToSave = true;

                    if (readyToSave)
                    {
                        isSaving = true;
                        this.selectorView.ChangeSaveStatus(true);

                        try
                        {
                            await SaveTrackListAsync();
                        }
                        finally
                        {
                            isSaving = false;
                            this.selectorView.ChangeSaveButtonColor(false);
                            this.selectorView.ChangeSaveStatus(false);
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

        #region DATATABLE SAVES - SELECTOR
        private void SelectorTrackListChanged()
        {
            if(this.tagValueFilterList != null && this.tagValueFilterList.Count > 0)
            {
                this.mediaPlayerComponent.SetWorkingTable(this.filteredSelectorTrackListTable);
            }
            else
            {
                this.mediaPlayerComponent.SetWorkingTable(this.selectorTrackListTable);
            }
        }
        #endregion

        #region SELECTOR TRACKLIST - ORDER
        public void OrderSelectorByColumnEvent(object sender, Messenger e)
        {
            this.OrderSelectorByColumn(e.StringField1);
        }
        private void OrderSelectorByColumn(String columnName)
        {
            if (this.selectorTrackListTable != null && this.selectorTrackListTable.Rows != null && this.selectorTrackListTable.Rows.Count > 0 && !isSaving)
            {
                DataView dv = new DataView();

                DataTable sourceTable = new DataTable();
                
                if(this.tagValueFilterList != null && this.tagValueFilterList.Count > 0)
                {
                    sourceTable = this.filteredSelectorTrackListTable;
                }
                else
                {
                    sourceTable = this.selectorTrackListTable;
                }

                dv = sourceTable.DefaultView;
                dv.Sort = columnName;
                DataTable sortedDT = dv.ToTable();

                if (this.selectorColumnOrderStates[columnName] == -1)
                {
                    this.selectorColumnOrderStates[columnName] = 0;
                    sourceTable = sortedDT;
                }
                else if (this.selectorColumnOrderStates[columnName] == 0)
                {
                    this.selectorColumnOrderStates[columnName] = 1;

                    DataTable reversedDt = new DataTable();
                    reversedDt = sourceTable.Clone();
                    for (var row = sourceTable.Rows.Count - 1; row >= 0; row--)
                        reversedDt.ImportRow(sourceTable.Rows[row]);

                    sourceTable = reversedDt;
                }
                else if (this.selectorColumnOrderStates[columnName] == 1)
                {
                    this.selectorColumnOrderStates[columnName] = 0;
                    sourceTable = sortedDT;
                }

                if (this.tagValueFilterList != null && this.tagValueFilterList.Count > 0)
                {
                    this.filteredSelectorTrackListTable = sourceTable;
                }
                else
                {
                    this.selectorTrackListTable = sourceTable;
                }

                this.ReloadSelectorTrackList();
                this.SelectorTrackListChanged();
            }
        }
        #endregion

        #region TRACKLIST - ORDER
        public void OrderByColumnEvent(object sender, Messenger e)
        {
            this.OrderByColumn(e.StringField1);
        }
        private void OrderByColumn(String columnName)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0 && !isSaving)
            {
                DataView dv = new DataView();
                dv = this.trackListTable.DefaultView;
                dv.Sort = columnName;
                DataTable sortedDT = dv.ToTable();

                if (this.columnOrderStates[columnName] == -1)
                {
                    this.columnOrderStates[columnName] = 0;
                    this.trackListTable = sortedDT;
                }
                else if (this.columnOrderStates[columnName] == 0)
                {
                    this.columnOrderStates[columnName] = 1;

                    DataTable reversedDt = new DataTable();
                    reversedDt = this.trackListTable.Clone();
                    for (var row = this.trackListTable.Rows.Count - 1; row >= 0; row--)
                        reversedDt.ImportRow(this.trackListTable.Rows[row]);

                    this.trackListTable = reversedDt;
                }
                else if (this.columnOrderStates[columnName] == 1)
                {
                    this.columnOrderStates[columnName] = 0;
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
            List<int> sourceIndices = e.SelectedIndices.OrderBy(i => i).ToList(); // Rendezés növekvő sorrendbe
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

            rowsToMove.Reverse();
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
                for (int i = 0; i <= e.SelectedRows.Count - 1; i++)
                {
                    int trackId = (int)e.SelectedRows[i].Cells["Id"].Value;
                    Model.Track track = this.trackDao.GetTrackWithTags(trackId, this.tagList).Value;
                    sourceTrackList.Add(track);
                }

                if (sourceTrackList != null && sourceTrackList.Count > 0)
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

            if (e.IntegerField1 > -1)
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
                        Model.Track track = this.trackDao.GetTrackWithTags(trackId, this.tagList).Value;
                        sourceTrackList.Add(track);
                    }

                    if (sourceTrackList != null && sourceTrackList.Count > 0)
                    {
                        this.AddTracksToPlaylist(playlistId, sourceTrackList);

                        bool isLogMessageDisplayed = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value.Value;
                        decimal logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString()).Value;
                        if (isLogMessageDisplayed)
                        {
                            String message = sourceTrackList.Count() + " track";
                            if (sourceTrackList.Count() > 1)
                            {
                                message += "s";
                            }
                            message += " copied to playlist [" + playlistRow["Name"].ToString() + "]";
                            this.selectorView.DisplayLog(message, logMessageDisplayTime);
                        }

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

                    this.ImportKeysAndBpmsFromExternalSource(ref trackList, tagList);


                }
                return trackList;
            }

        }
        private void ImportKeysAndBpmsFromExternalSource(ref List<Model.Track> trackList, List<Tag> tagList)
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

                bool isLogMessageDisplayed = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value.Value;
                decimal logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString()).Value;
                if (isLogMessageDisplayed)
                {
                    if (this.currentPlaylistId != -1)
                    {
                        Playlist playlist = this.trackDao.GetPlaylist(this.currentPlaylistId).Value;
                        String playlistName = String.Empty;
                        if (playlist != null)
                        {
                            playlistName = playlist.Name;
                        }

                        String message = trackList.Count() + " track";
                        if (trackList.Count() > 1)
                        {
                            message += "s";
                        }
                        message += " added to playlist [" + playlistName + "]";
                        this.selectorView.DisplayLog(message, logMessageDisplayTime);
                    }
                }
            }
            if (isMoveToTracklist)
            {
                DataTable table = new DataTable();

                if(this.tagValueFilterList != null && this.tagValueFilterList.Count > 0)
                {
                    table = this.filteredSelectorTrackListTable;
                }
                else
                {
                    table = this.selectorTrackListTable;
                }

                if(table.Rows != null && table.Rows.Count > 0)
                {
                    foreach(String pathToDelete in e.DragAndDropFilePathArray)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            String path = row["Path"].ToString();
                            int trackIdInPlaylist = Convert.ToInt32(row["TrackIdInPlaylist"]);

                            if (!String.IsNullOrEmpty(path) && path == pathToDelete)
                            {
                                this.trackDao.DeletePlaylistContentByPlaylistIdAndTrackInPlaylist(this.currentSelectorPlaylistId, trackIdInPlaylist);
                                break;
                            }
                        }
                    }

                    this.InitializeSelectorTrackListRows(this.selectorTrackListTable, null);

                    DataView dv = this.selectorTrackListTable.DefaultView;
                    dv.RowFilter = "";
                    DataTable filteredDT = dv.ToTable();
                    this.filteredSelectorTrackListTable = filteredDT;

                    if (!isSelectorTrackListInitializing)
                        ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(false);

                    this.selectorTrackListBindingSource.DataSource = this.filteredSelectorTrackListTable;
                    this.InitializeSelectorTrackList(this.filteredSelectorTrackListTable, this.mediaPlayerComponent.CurrentTrackIdInPlaylist);

                    if (!isSelectorTrackListInitializing)
                        ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(true);

                    ((SelectorView)this.selectorView).SetTagValueFilter(this.tagValueFilterList);





                   /* ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(false);

                    this.ReloadSelectorTrackList();
                    this.SelectorTrackListChanged();

                    this.UpdateTrackCountAndLength();

                    ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(true);*/
                }
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

                bool isLogMessageDisplayed = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value.Value;
                decimal logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString()).Value;
                if (isLogMessageDisplayed)
                {
                    String message = trackList.Count() + " track";
                    if (trackList.Count() > 1)
                    {
                        message += "s";
                    }
                    message += " added to playlist [" + playlistRow["Name"].ToString() + "]";
                    this.selectorView.DisplayLog(message, logMessageDisplayTime);
                }
            }
        }
        private void AddTracksToPlaylist(int playlistId, List<Model.Track> trackList, int dragIndex = -1, bool internalDragAndDrop = false)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
                loadingDialog.SetProcessDescription("Add tracks to playlist/tracklist...");
                loadingDialog.Refresh();

                List<Model.Track> playlistTracklist = this.trackDao.GetTracklistWithTagsByPlaylistId(playlistId, this.tagList).Value;

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
                        plc.TrackIdInPlaylist = this.trackDao.GetNextSmallestTrackIdInPlaylist().Value;

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
        private void LoadTrackList(List<Model.Track> trackList, int dragIndex)
        {
            if (trackList != null && trackList.Count > 0)
            {
                if (!isTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleTracklistSelection(false);

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
                                else {
                                    args[i] = ttv.TagValueId;
                                }
                                i++;
                            }
                        }

                        this.trackListTable.Rows.Add(args);
                    }
                }

                if (!isTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleTracklistSelection(true);


                this.ReloadTrackList();
                this.TrackListChanged();

                this.UpdateTrackCountAndLength();
            }
        }
        #endregion

        #region SELECTOR - DRAG AND DROP
        private void MoveSelectorTracklistRowsEvent(object sender, Messenger e)
        {
            List<int> sourceIndices = e.SelectedIndices.OrderBy(i => i).ToList(); // Rendezés növekvő sorrendbe
            int targetIndex = e.IntegerField1;

            // Create a list to hold the rows to be moved
            List<DataRow> rowsToMove = new List<DataRow>();

            if(tagValueFilterList!= null  && tagValueFilterList.Count > 0)
            {
                foreach (int sourceIndex in sourceIndices.OrderByDescending(i => i))
                {
                    DataRow row = filteredSelectorTrackListTable.NewRow();
                    row.ItemArray = filteredSelectorTrackListTable.Rows[sourceIndex].ItemArray;
                    rowsToMove.Add(row);
                    filteredSelectorTrackListTable.Rows.RemoveAt(sourceIndex);
                }
            }
            else
            {
                foreach (int sourceIndex in sourceIndices.OrderByDescending(i => i))
                {
                    DataRow row = selectorTrackListTable.NewRow();
                    row.ItemArray = selectorTrackListTable.Rows[sourceIndex].ItemArray;
                    rowsToMove.Add(row);
                    selectorTrackListTable.Rows.RemoveAt(sourceIndex);
                }
            }
            

            // Adjust the target index if necessary
            if (targetIndex > sourceIndices.Min())
            {
                targetIndex -= rowsToMove.Count;
            }

            // Insert the rows at the target index
            if (tagValueFilterList != null && tagValueFilterList.Count > 0)
            {
                foreach (DataRow row in rowsToMove)
                {
                    filteredSelectorTrackListTable.Rows.InsertAt(row, targetIndex);
                    targetIndex++;
                }
            }
            else
            {
                foreach (DataRow row in rowsToMove)
                {
                    selectorTrackListTable.Rows.InsertAt(row, targetIndex);
                    targetIndex++;
                }
            }
            

            this.ReloadSelectorTrackList();
            this.SelectorTrackListChanged();
        }
        private void InternalDragAndDropIntoSelectorTracklistEvent(object sender, Messenger e)
        {
            List<Model.Track> sourceTrackList = new List<Model.Track>();
            if (e.SelectedRows != null && e.SelectedRows.Count > 0)
            {
                for (int i = 0; i <= e.SelectedRows.Count - 1; i++)
                {
                    int trackId = (int)e.SelectedRows[i].Cells["Id"].Value;
                    Model.Track track = this.trackDao.GetTrackWithTags(trackId, this.tagList).Value;
                    sourceTrackList.Add(track);
                }

                if (sourceTrackList != null && sourceTrackList.Count > 0)
                {
                    this.AddTracksToPlaylist(this.currentSelectorPlaylistId, sourceTrackList, e.IntegerField1, true);
                }
            }
        }
        private void ExternalDragAndDropIntoSelectorTracklistEvent(object sender, Messenger e)
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

                bool isLogMessageDisplayed = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value.Value;
                decimal logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString()).Value;
                if (isLogMessageDisplayed)
                {
                    if (this.currentPlaylistId != -1)
                    {
                        Playlist playlist = this.trackDao.GetPlaylist(this.currentPlaylistId).Value;
                        String playlistName = String.Empty;
                        if (playlist != null)
                        {
                            playlistName = playlist.Name;
                        }

                        String message = trackList.Count() + " track";
                        if (trackList.Count() > 1)
                        {
                            message += "s";
                        }
                        message += " added to playlist [" + playlistName + "]";
                        this.selectorView.DisplayLog(message, logMessageDisplayTime);
                    }
                }
            }
        }

        #endregion

        #region TRACKLIST - REMOVE TRACKS
        //In the case of tracklist remove, selection change evetn must be turn off and on again to NOT load the UpdateCoverBrowser unnecessary
        private void DeleteTracksEvent(object sender, Messenger e)
        {
            // Check if filter mode is disabled and saving is not in progress
            if (!isSaving)
            {
                // Check if the track list table is not null and has rows
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    // Temporarily disable tracklist selection
                    ((SelectorView)this.selectorView).ToggleTracklistSelection(false);

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

                    this.UpdateTrackCountAndLength();

                    ((SelectorView)this.selectorView).ToggleTracklistSelection(true);
                }
            }
        }
        public void RemoveMissingTracks()
        {
            if (!isSaving)
            {
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    ((SelectorView)this.selectorView).ToggleTracklistSelection(false);
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

                    this.UpdateTrackCountAndLength();

                    ((SelectorView)this.selectorView).ToggleTracklistSelection(true);
                }
            }
        }
        public void RemoveDuplicatedTracks()
        {
            if (!isSaving)
            {
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    ((SelectorView)this.selectorView).ToggleTracklistSelection(false);
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

                    this.UpdateTrackCountAndLength();

                    ((SelectorView)this.selectorView).ToggleTracklistSelection(true);
                }
            }
        }
        public void Clear()
        {
            // Check if filter mode is disabled and saving is not in progress
            if (!isSaving)
            {
                // Temporarily disable tracklist selection
                ((SelectorView)this.selectorView).ToggleTracklistSelection(false);

                // Clear all rows in the track list table
                this.trackListTable.Rows.Clear();

                // Perform UI updates after clearing the track list
                this.ReloadTrackList();
                this.TrackListChanged();

                this.UpdateTrackCountAndLength();

                // Re-enable tracklist selection
                ((SelectorView)this.selectorView).ToggleTracklistSelection(true);
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
        }
        public void PlayTrackEvent(object sender, Messenger e)
        {
            this.PlayTrack(e.StringField1, e.IntegerField1);
        }
        public void PlayTrack(String source, int index)
        {

            if(index > -1)
            {
                this.mediaPlayerComponent.SetCurrentTrackIndex(index);
            }


            DataTable sourceTable = new DataTable();
            SourceTable sourceTableForGridUpdate = new SourceTable();

            if (source == TableSourceForMediaPlayer.MainButton.ToString())
            {
                if (this.isTrackListActive)
                {
                    sourceTable= this.trackListTable;
                    sourceTableForGridUpdate = SourceTable.Tracklist;
                }
                else
                {
                    if (this.tagValueFilterList != null && this.tagValueFilterList.Count > 0)
                    {
                        sourceTable = this.filteredSelectorTrackListTable;
                        sourceTableForGridUpdate = SourceTable.Selector;
                    }
                    else
                    {
                        sourceTable = this.selectorTrackListTable;
                        sourceTableForGridUpdate = SourceTable.Selector;
                    }
                }
            }
            else if(source == TableSourceForMediaPlayer.TracklistKeyDown.ToString() ||
                source == TableSourceForMediaPlayer.TracklistDoubleClick.ToString())
            {
                sourceTable = this.trackListTable;
                sourceTableForGridUpdate = SourceTable.Tracklist;
            }
            else if (source == TableSourceForMediaPlayer.SelectorButton.ToString() ||
                source == TableSourceForMediaPlayer.SelectorDoubleClick.ToString())
            {
                if (this.tagValueFilterList != null && this.tagValueFilterList.Count > 0)
                {
                    sourceTable = this.filteredSelectorTrackListTable;
                    sourceTableForGridUpdate = SourceTable.Selector;
                }
                else
                {
                    sourceTable = this.selectorTrackListTable;
                    sourceTableForGridUpdate = SourceTable.Selector;
                }
            }

            this.mediaPlayerComponent.SetWorkingTable(sourceTable, true);

            MediaPlayerUpdateState updateState = this.mediaPlayerComponent.PlayTrack();

            if (updateState == MediaPlayerUpdateState.AfterPlay)
            {
                if (sourceTable != null && sourceTable.Rows != null && sourceTable.Rows.Count > 0)
                {
                    this.selectorView.UpdateAfterPlayTrack(this.mediaPlayerComponent.GetCurrentTrackIndex(), this.mediaPlayerComponent.CurrentTrackIdInPlaylist, sourceTableForGridUpdate);
                }
            }
            else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
            {
                this.selectorView.UpdateAfterPlayTrackAfterPause();
            }
        }
        public void PauseTrackEvent(object sender, EventArgs e)
        {
            if (this.mediaPlayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                MediaPlayerUpdateState updateState = this.mediaPlayerComponent.PlayTrack();

                if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                {
                    this.selectorView.UpdateAfterPlayTrackAfterPause();
                }
            }
            else
            {
                this.mediaPlayerComponent.PauseTrack();
                this.selectorView.UpdateAfterPauseTrack();
            }

        }
        public void StopTrackEvent(object sender, EventArgs e)
        {
            this.mediaPlayerComponent.StopTrack();
            this.selectorView.UpdateAfterStopTrack();
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
                this.selectorView.SetVolume(volume);
            }
            else
            {
                volume = this.settingDao.GetIntegerSetting(Settings.Volume.ToString()).Value;
                this.mediaPlayerComponent.ChangeVolume(volume);
                this.selectorView.SetVolume(volume);
            }
        }
        #endregion

        #region PLAYLIST
        private void CreatePlaylist(object sender, EventArgs e)
        {
            PlaylistEditorView playlistEditorView = new PlaylistEditorView();
            PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(playlistEditorView, this.trackDao, this.settingDao);
            if (playlistEditorView.ShowDialog((SelectorView)this.selectorView) == DialogResult.OK)
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

                Playlist playlist = this.trackDao.GetPlaylist(id).Value;

                int playlistIndex = this.playlistListTable.Rows.IndexOf(playlistRow);

                PlaylistEditorView playlistEditorView = new PlaylistEditorView();
                PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(playlistEditorView, this.trackDao, this.settingDao, playlist);
                if (playlistEditorView.ShowDialog((SelectorView)this.selectorView) == DialogResult.OK)
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
            String message = String.Empty;

            if(e.IntegerField1 > 0)
            {
                if (this.isTrackListActive)
                {
                    this.currentPlaylistId = e.IntegerField1;
                }
                else
                {
                    this.currentSelectorPlaylistId = e.IntegerField1;
                }
            }

            if (this.isTrackListActive)
            {
                if (this.isTrackListChanged)
                {
                    this.SaveTrackListSync();
                    this.isTrackListChanged = false;
                    this.selectorView.ChangeSaveButtonColor(false);
                }
            }

            if (this.isTrackListActive)
            {
                this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistIdInSelector.ToString(), this.currentPlaylistId);
            }
            else
            {
                this.settingDao.SetIntegerSetting(Settings.CurrentSelectorPlaylistId.ToString(), this.currentSelectorPlaylistId);
            }

            if (this.currentPlaylistId != -1)
            {
                Playlist pls = this.trackDao.GetPlaylist(e.IntegerField1).Value;
                if(pls != null)
                {
                    message = "Playlist [" + pls.Name + "] has been loaded";
                }
                

                bool isLogMessageDisplayed = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value.Value;
                decimal logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString()).Value;
                if (isLogMessageDisplayed)
                {
                    this.selectorView.DisplayLog(message, logMessageDisplayTime);
                }
            }

            this.ReloadPlaylist();

            if (this.isTrackListActive)
            {
                this.InitializeTrackListRows(this.trackListTable, null);
            }
            else
            {
                this.InitializeSelectorTrackListRows(this.selectorTrackListTable, null);

                DataView dv = this.selectorTrackListTable.DefaultView;
                dv.RowFilter = "";
                DataTable filteredDT = dv.ToTable();
                this.filteredSelectorTrackListTable = filteredDT;

                if (!isSelectorTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(false);

                this.selectorTrackListBindingSource.DataSource = this.filteredSelectorTrackListTable;
                this.InitializeSelectorTrackList(this.filteredSelectorTrackListTable, this.mediaPlayerComponent.CurrentTrackIdInPlaylist);

                if (!isSelectorTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(true);

                ((SelectorView)this.selectorView).SetTagValueFilter(this.tagValueFilterList);

            }
        }
        private void SelectorView_LoadPlaylistIntoTracklistEvent(object sender, Messenger e)
        {
            this.isTrackListActive = true;

            String message = String.Empty;

            if (e.IntegerField1 > 0)
            {
                this.currentPlaylistId = e.IntegerField1;
            }
            if (this.isTrackListChanged)
            {
                this.SaveTrackListSync();
                this.isTrackListChanged = false;
                this.selectorView.ChangeSaveButtonColor(false);
            }

            this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistIdInSelector.ToString(), this.currentPlaylistId);

            if (this.currentPlaylistId != -1)
            {
                Playlist pls = this.trackDao.GetPlaylist(e.IntegerField1).Value;
                if (pls != null)
                {
                    message = "Playlist [" + pls.Name + "] has been loaded";
                }


                bool isLogMessageDisplayed = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value.Value;
                decimal logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString()).Value;
                if (isLogMessageDisplayed)
                {
                    this.selectorView.DisplayLog(message, logMessageDisplayTime);
                }
            }

            this.ReloadPlaylist();

            this.InitializeTrackListRows(this.trackListTable, null);
        }
        private void SelectorView_LoadPlaylistIntoSelectorEvent(object sender, Messenger e)
        {
            String message = String.Empty;

            if (e.IntegerField1 > 0)
            {
                this.currentSelectorPlaylistId = e.IntegerField1;
            }

            if (this.isTrackListActive)
            {
                if (this.isTrackListChanged)
                {
                    this.SaveTrackListSync();
                    this.isTrackListChanged = false;
                    this.selectorView.ChangeSaveButtonColor(false);
                }
            }

            this.settingDao.SetIntegerSetting(Settings.CurrentSelectorPlaylistId.ToString(), this.currentSelectorPlaylistId);

            if (this.currentPlaylistId != -1)
            {
                Playlist pls = this.trackDao.GetPlaylist(e.IntegerField1).Value;
                if (pls != null)
                {
                    message = "Playlist [" + pls.Name + "] has been loaded";
                }


                bool isLogMessageDisplayed = this.settingDao.GetBooleanSetting(Settings.IsLogMessageEnabled.ToString()).Value.Value;
                decimal logMessageDisplayTime = this.settingDao.GetDecimalSetting(Settings.LogMessageDisplayTime.ToString()).Value;
                if (isLogMessageDisplayed)
                {
                    this.selectorView.DisplayLog(message, logMessageDisplayTime);
                }
            }

            this.ReloadPlaylist();

            this.InitializeSelectorTrackListRows(this.selectorTrackListTable, null);

            DataView dv = this.selectorTrackListTable.DefaultView;
            dv.RowFilter = "";
            DataTable filteredDT = dv.ToTable();
            this.filteredSelectorTrackListTable = filteredDT;

            if (!isSelectorTrackListInitializing)
                ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(false);

            this.selectorTrackListBindingSource.DataSource = this.filteredSelectorTrackListTable;
            this.InitializeSelectorTrackList(this.filteredSelectorTrackListTable, this.mediaPlayerComponent.CurrentTrackIdInPlaylist);

            if (!isSelectorTrackListInitializing)
                ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(true);

            ((SelectorView)this.selectorView).SetTagValueFilter(this.tagValueFilterList);

        }
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

                            if (e.IntegerField1 >= this.playlistListTable.Rows.Count)
                            {
                                e.IntegerField1 = this.playlistListTable.Rows.Count - 1;
                            }

                            this.currentPlaylistId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);
                            if(this.currentPlaylistId == 0)
                            {
                                ;
                            }
                            this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistIdInSelector.ToString(), this.currentPlaylistId);
                            this.ReloadPlaylist();

                            this.InitializeTrackListRows(this.trackListTable, null);
                            
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
            ((SelectorView)this.selectorView).UpdateDisplayPlaylistList(this.isPlaylistListDisplayed);
        }
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
                    List<Model.Track> trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(playlistId, this.tagList).Value;
                    if (trackList != null && trackList.Count > 0)
                    {
                        foreach (Model.Track track in trackList)
                        {
                            row = "#EXTVDJ:";
                            row += track.Length.ToString() + ",";
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
                    List<Model.Track> trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(playlistId, this.tagList).Value;
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

                            TrackTagValue tag = track.TrackTagValues.Find(x => x.TagName == "Key");
                            if (tag != null)
                            {
                                String key = tag.TagValueName;
                                row += " (" + key + ")";
                            }



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
            List<Model.Track> trackList = this.trackDao.GetTracklistWithTagsByPlaylistId(playlistId, this.tagList).Value;
            if (trackList != null && trackList.Count > 0)
            {
                ExportToDirectoryPresenter presenter = new ExportToDirectoryPresenter(exportToDirectoryView, trackList, this.tagDao, this.settingDao);
                presenter.Initialize();


                if (exportToDirectoryView.ShowDialog((SelectorView)this.selectorView) == DialogResult.OK)
                {

                }
            }


        }
        #endregion

        #region TAG COMPONENT
        private void EnableFilterModeEvent(object sender, EventArgs e)
        {
            using (LoadingDialog loadingDialog = new LoadingDialog())
            {
                loadingDialog.Show();
                loadingDialog.SetProcessDescription("Preparing filter mode...");
                loadingDialog.Refresh();

                DataView dv = this.selectorTrackListTable.DefaultView;
                dv.RowFilter = "";
                DataTable filteredDT = dv.ToTable();
                this.filteredSelectorTrackListTable = filteredDT;

                if (!isSelectorTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(false);

                this.selectorTrackListBindingSource.DataSource = this.filteredSelectorTrackListTable;
                this.InitializeSelectorTrackList(this.filteredSelectorTrackListTable, this.mediaPlayerComponent.CurrentTrackIdInPlaylist);

                if (!isSelectorTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(true);
            }
        }
        private List<TagValueFilter> tagValueFilterList { get; set; }
        private void FilterByTagValue(String tagName, String tagValueName, String tagValueValue, int tagValueId)
        {
            if (this.tagValueFilterList == null || this.tagValueFilterList.Count == 0)
            {
                this.tagValueFilterList = new List<TagValueFilter>();
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
                if (!this.tagValueFilterList.Exists(x => x.TagName == tagName))
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
           ((SelectorView)this.selectorView).SetTagValueFilter(this.tagValueFilterList);
        }
        private void SetTagValueEvent(object sender, Messenger e)
        {
            this.FilterByTagValue(e.StringField1, e.StringField2, e.StringField3, e.IntegerField1);
        }
        private void ClearTagValueEvent(object sender, Messenger e)
        {
            for (int i = this.tagValueFilterList.Count - 1; i >= 0; i--)
            {
                if (this.tagValueFilterList[i].TagName == e.StringField1)
                {
                    this.tagValueFilterList.RemoveAt(i);
                }
            }
            ((SelectorView)this.selectorView).SetTagValueFilter(this.tagValueFilterList);
        }
        private void RemoveTagValueFilter(object sender, EventArgs e)
        {
            this.tagValueFilterList = new List<TagValueFilter>();

            ((SelectorView)this.selectorView).SetTagValueFilter(this.tagValueFilterList);
        }
        private String textFilter = String.Empty;
        private void ChangeFilterParameters(object sender, Messenger e)
        {
            String filterText = e.StringField1;
            textFilter = filterText;

            if (isPlaylistDisplayed)
            {
                DataView dv = this.selectorTrackListTable.DefaultView;
                
                String filterQuery = String.Empty;
                String filterQuery2 = String.Empty;
                List<String> filteringColumnNames = new List<String>();

                if (!String.IsNullOrEmpty(filterText))
                {
                    filterText = filterText.Replace("'", "''");

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
                                filterQuery2 += "(" + lastTagName + " = " + this.tagValueFilterList[i].TagValueValue + " ";
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
                this.filteredSelectorTrackListTable = filteredDT;

                if (!isSelectorTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(false);

                this.selectorTrackListBindingSource.DataSource = this.filteredSelectorTrackListTable;
                this.InitializeSelectorTrackList(this.filteredSelectorTrackListTable, this.mediaPlayerComponent.CurrentTrackIdInPlaylist);

                if (!isSelectorTrackListInitializing)
                    ((SelectorView)this.selectorView).ToggleSelectorTracklistSelection(true);


                this.mediaPlayerComponent.SetWorkingTable(this.filteredSelectorTrackListTable);
            }
            else
            {
                LoadTracksFromDatabase();
            }


            
        }
        #endregion
    }
}
