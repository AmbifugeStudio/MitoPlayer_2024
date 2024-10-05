using Google.Protobuf.WellKnownTypes;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using MySqlX.XDevAPI.Common;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using TagLib.Mpeg4;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Type = System.Type;

namespace MitoPlayer_2024.Presenters
{
    
    public class PlaylistPresenter
    {
        private IPlaylistView playlistView { get; set; }
        private ITrackDao trackDao { get; set; }
        private ITagDao tagDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private DataTable playlistListTable { get; set; }
        public DataTable trackListTable { get; set; }
        private DataTable selectedTrackListTable { get; set; }
        private BindingSource playlistListBindingSource { get; set; }
        private BindingSource trackListBindingSource { get; set; }
        private BindingSource selectedTrackListBindingSource { get; set; }
        public int currentPlaylistId { get; set; }
        public bool[] PlaylistColumnVisibilityArray { get; set; }
        public bool[] TrackColumnVisibilityArray { get; set; }
        public int[] TrackColumnSortingIdArray { get; set; }
        //public int[] TrackColumnDisplayIndexArray { get; set; }
        Dictionary<string, int> columnOrderStates { get; set; }
        private MediaPlayerComponent mediaPLayerComponent { get; set; }

        private bool isTagEditorDisplayed { get; set; }
        private bool isPlaylistListDisplayed { get; set; }
        private List<Tag> tagList { get; set; }
        private List<TagValue> tagValueList { get; set; }
        private Tag currentTag { get; set; }
        private TagValue currentTagValue { get; set; }

        public PlaylistPresenter(IPlaylistView view, MediaPlayerComponent mediaPlayer, ITrackDao trackDao, ITagDao tagValueDao, ISettingDao settingDao)
        {
            ResultOrError result = new ResultOrError();

            //INITIALIZE
            this.playlistView = view;
            this.trackDao = trackDao;
            this.tagDao = tagValueDao;
            this.settingDao = settingDao;

            if (result.Success)
                result = this.InitializeTaglist();
            if (result.Success)
                result = this.InitializeDataTables();
            if (result.Success)
            {
                this.mediaPLayerComponent = mediaPlayer;
                this.mediaPLayerComponent.Initialize(this.trackListTable);
            }
            if (result.Success)
                result= this.InitializeTagEditor();
            if (result.Success)
                result= this.ResetPlaylistList();
            if (result.Success)
                result = InitializeColoringByTagValues();
            if (result.Success)
                result= this.InitializeVolume();

            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //MEDIA PLAYER
            this.playlistView.SetCurrentTrackEvent += SetCurrentTrackEvent;
            this.playlistView.PlayTrackEvent += PlayTrackEvent;
            this.playlistView.PauseTrackEvent += PauseTrackEvent;
            this.playlistView.StopTrackEvent += StopTrackEvent;
            this.playlistView.PrevTrackEvent += PrevTrackEvent;
            this.playlistView.NextTrackEvent += NextTrackEvent;
            this.playlistView.RandomTrackEvent += RandomTrackEvent;
            this.playlistView.GetMediaPlayerProgressStatusEvent += GetMediaPlayerProgressStatusEvent;
            this.playlistView.SetCurrentTrackColorEvent += SetCurrentTrackColorEvent;

            //TRACKLIST
            this.playlistView.OrderByColumnEvent += OrderByColumnEvent;
            this.playlistView.DeleteTracksEvent += DeleteTracksEvent;

            this.playlistView.InternalDragAndDropIntoTracklistEvent += InternalDragAndDropIntoTracklistEvent;
            this.playlistView.InternalDragAndDropIntoPlaylistEvent += InternalDragAndDropIntoPlaylistEvent;
            this.playlistView.ExternalDragAndDropIntoTracklistEvent += ExternalDragAndDropIntoTracklistEvent;
            this.playlistView.ExternalDragAndDropIntoPlaylistEvent += ExternalDragAndDropIntoPlaylistEvent;
            this.playlistView.ChangeTracklistColorEvent += ChangeTracklistColorEvent;

            this.playlistView.ShowColumnVisibilityEditorEvent += ShowColumnVisibilityEditorEvent;
            this.playlistView.ScanBpmEvent += ScanBpmEvent;
            this.playlistView.ExportToDirectoryEvent += ExportToDirectoryEvent;

            //PLAYLIST
            this.playlistView.CreatePlaylist += CreatePlaylist;
            this.playlistView.EditPlaylist += EditPlaylist;
            this.playlistView.LoadPlaylistEvent += LoadPlaylistEvent;
            this.playlistView.MovePlaylistEvent += MovePlaylistEvent;
            this.playlistView.DeletePlaylistEvent += DeletePlaylistEvent;
            this.playlistView.SetQuickListEvent += SetQuickListEvent;
            this.playlistView.ExportToM3UEvent += ExportToM3UEvent;
            this.playlistView.ExportToTXTEvent += ExportToTXTEvent;
            this.playlistView.DisplayPlaylistListEvent += DisplayPlaylistListEvent;

            //TAG EDITOR
            this.playlistView.DisplayTagEditorEvent += DisplayTagEditorEvent;
            this.playlistView.SelectTagEvent += SelectTagEvent;
            this.playlistView.SetTagValueEvent += SetTagValueEvent;
            this.playlistView.ClearTagValueEvent += ClearTagValueEvent;

            this.playlistView.Show();
        }

        

        #region INITIALIZE

        private ResultOrError InitializeTaglist()
        {
            ResultOrError result = new ResultOrError();
            try
            {
                List<Tag> tagList = this.tagDao.GetAllTag();
                if (tagList != null && tagList.Count > 0)
                {
                    this.tagList = tagList;
                }
            }
            catch(Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }
        private ResultOrError InitializeDataTables()
        {
            ResultOrError result = new ResultOrError();
            if (result.Success)
                result = this.InitializePlaylistDataTable();
            if (result.Success)
                result = this.InitializeTracklistDataTable();
            if (result.Success)
                result = this.InitializePlaylistListAndTrackList();
            return result;
        }
        private ResultOrError InitializePlaylistDataTable()
        {
            //PLAYLIST GRID
            ResultOrError result = new ResultOrError();
            try
            {
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
                    this.PlaylistColumnVisibilityArray = tpList.Select(x => x.IsEnabled).ToArray();
                }

                this.SetPlaylistDatagGridView(this.playlistListTable);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            
            return result;
        }
        private ResultOrError InitializeTracklistDataTable()
        {
            //TRACKLIST GRID
            ResultOrError result = new ResultOrError();
            try
            {
                this.trackListBindingSource = new BindingSource();
                this.trackListTable = new DataTable();
                this.columnOrderStates = new Dictionary<string, int>();
                List<TrackProperty> tpList = new List<TrackProperty>();
                tpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString());
                if (tpList != null && tpList.Count > 0)
                {
                    this.TrackColumnSortingIdArray = tpList.Select(x => x.SortingId).ToArray();

                    tpList = tpList.OrderBy(x => x.SortingId).ToList();
                    for (int i = 0; i <= tpList.Count - 1; i++)
                    {
                        this.columnOrderStates.Add(tpList[i].Name, -1);
                        this.trackListTable.Columns.Add(tpList[i].Name, Type.GetType(tpList[i].Type));
                    }

                    this.TrackColumnVisibilityArray = tpList.Select(x => x.IsEnabled).ToArray();

                }
                this.SetTrackListDataGridView(this.trackListTable);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
           
            return result;
        }
        private ResultOrError InitializePlaylistListAndTrackList()
        {
            ResultOrError result = new ResultOrError();
            try
            {
                this.playlistListTable.Clear();

                List<Playlist> plsList = this.trackDao.GetAllPlaylist();
                foreach (Playlist playlist in plsList)
                    this.playlistListTable.Rows.Add(playlist.Id, playlist.QuickListGroup, playlist.Name, playlist.OrderInList, playlist.ProfileId, playlist.IsActive);

                Playlist activePlaylist = this.trackDao.GetActivePlaylist();
                this.currentPlaylistId = activePlaylist.Id;

                if (result.Success)
                    result = this.LoadPlaylist(activePlaylist);

                if(result.Success)
                    result = this.SetPlaylistDatagGridView(playlistListTable);
            }
            catch(Exception ex)
            {
                result.AddError(ex.Message);
            }
            
            return result;
        }
        private ResultOrError LoadPlaylist(Playlist pls)
        {
            ResultOrError result = new ResultOrError();

            try
            {
                List<Track> trackList = this.trackDao.GetTracklistByPlaylistId(pls.Id, tagList);
                if (trackList != null && trackList.Count > 0)
                {
                    foreach (Track track in trackList)
                    {
                        String length = this.LengthToString(track.Length);

                        int tagCount = 0;
                        if (track.TrackTagValues != null && track.TrackTagValues.Count > 0)
                        {
                            tagCount = track.TrackTagValues.Count;
                        }

                        object[] args = new object[11 + tagCount];
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

                        if (tagCount > 0)
                        {
                            int i = 11;
                            foreach (TrackTagValue ttv in track.TrackTagValues)
                            {
                                if (ttv.HasValue)
                                {
                                    args[i] = ttv.Value;
                                }
                                else
                                {
                                    args[i] = ttv.TagValueName;
                                }
                                i++;
                            }
                        }

                        this.trackListTable.Rows.Add(args);
                    }

                    result = this.SetTrackListDataGridView(this.trackListTable);

                }
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            if (result.Success)
            {
                try
                {
                    ((PlaylistView)this.playlistView).SetCurrentPlaylistColor(pls.Id);
                    ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }
            }

            return result;
        }
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
        private ResultOrError InitializeTagEditor()
        {
            ResultOrError result = new ResultOrError();
            try
            {
                this.isTagEditorDisplayed = this.settingDao.GetBooleanSetting(Settings.IsTagEditorDisplayed.ToString()).Value;
               // ((PlaylistView)this.playlistView).InitializeTagEditor(this.tagList);

                List<List<TagValue>> tagValueListContainer = new List<List<TagValue>>();
                foreach(Tag tag in this.tagList)
                {
                    List<TagValue> tagValues = new List<TagValue>();
                    tagValues = this.tagDao.GetTagValuesByTagId(tag.Id);
                    tagValueListContainer.Add(tagValues);
                }


                ((PlaylistView)this.playlistView).InitializeTagValueEditor2(this.tagList, tagValueListContainer, this.isTagEditorDisplayed);
               // ((PlaylistView)this.playlistView).InitializeTagValueEditor(null, this.isTagEditorDisplayed);
                ((PlaylistView)this.playlistView).CallDisplayTagEditor(this.isTagEditorDisplayed);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }
        private ResultOrError ResetPlaylistList()
        {
            ResultOrError result = new ResultOrError();
            try
            {
                this.isPlaylistListDisplayed = this.settingDao.GetBooleanSetting(Settings.IsPlaylistListDisplayed.ToString()).Value;
                ((PlaylistView)this.playlistView).ResetPlaylistList(this.isPlaylistListDisplayed);
                ((PlaylistView)this.playlistView).CallDisplayPlaylistList(this.isPlaylistListDisplayed);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }
        private ResultOrError InitializeColoringByTagValues()
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
        }
        private ResultOrError InitializeVolume()
        {
            ResultOrError result = new ResultOrError();
            try
            {
                int volume = this.settingDao.GetIntegerSetting(Settings.Volume.ToString());
                if (volume == -1)
                    volume = 50;
                this.playlistView.SetVolume(volume);
                this.mediaPLayerComponent.MediaPlayer.settings.volume = volume;
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }

        private void ChangeTracklistColorEvent(object sender, ListEventArgs e)
        {
            this.ChangeColoringByTagValues(e.StringField1);
        }
        private void ChangeColoringByTagValues(String tagName)
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
            this.SetTrackListDataGridView(this.trackListTable);
        }
       
        
        #endregion

        #region DATATABLES

        private ResultOrError SetPlaylistDatagGridView(DataTable playlistListTable)
        {
            ResultOrError result = new ResultOrError();
            try
            {
                this.playlistListBindingSource.DataSource = playlistListTable;
                this.playlistView.SetPlaylistListBindingSource(this.playlistListBindingSource, this.PlaylistColumnVisibilityArray, this.currentPlaylistId);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }
        private ResultOrError SetTrackListDataGridView(DataTable trackListTable)
        {
            ResultOrError result = new ResultOrError();
            try
            {
                int currentTrackIdInPlaylist = -1;
                if (this.mediaPLayerComponent != null)
                {
                    currentTrackIdInPlaylist = this.mediaPLayerComponent.CurrentTrackIdInPlaylist;
                }

                this.trackListBindingSource.DataSource = trackListTable;
                this.playlistView.SetTrackListBindingSource(this.trackListBindingSource, this.TrackColumnVisibilityArray, this.TrackColumnSortingIdArray, currentTrackIdInPlaylist);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }
        private void SetSelectedTrackList(DataTable trackListTable)
        {
            this.selectedTrackListBindingSource.DataSource = trackListTable;
            this.playlistView.SetSelectedTrackListBindingSource(this.selectedTrackListBindingSource);
        }
        private void SavePlaylistList(DataTable playlistListTable)
        {
            int orderInList = 0;
            List<Playlist> playlistlist = this.ConvertPlaylistDataTableToList(playlistListTable);
            foreach (Playlist playlist in playlistlist)
            {
                playlist.OrderInList = orderInList;
                this.trackDao.UpdatePlaylist(playlist);
                orderInList++;
            }
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
                playlist.QuickListGroup = Convert.ToInt32(dt.Rows[i]["G"]);
                playlist.ProfileId = Convert.ToInt32(dt.Rows[i]["ProfileId"]);
                playlist.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"]);
                playlistList.Add(playlist);
            }

            return playlistList;
        }
        private void SaveTrackList(DataTable trackListTable, int playlistId)
        {
            this.trackDao.DeletePlaylistContentByPlaylistId(playlistId);
            List<Track> tracklist = this.ConvertTrackDataTableToList(trackListTable);
            int orderInList = 0;
            foreach (Track track in tracklist)
            {
                track.OrderInList = orderInList;

                PlaylistContent plc = new PlaylistContent();
                plc.Id = this.trackDao.GetNextId(TableName.PlaylistContent.ToString());
                plc.PlaylistId = playlistId;
                plc.TrackId = track.Id;
                plc.OrderInList = track.OrderInList;
                plc.TrackIdInPlaylist = track.TrackIdInPlaylist;
                this.trackDao.CreatePlaylistContent(plc);

                orderInList++;
            }
            this.mediaPLayerComponent.SetWorkingTable(trackListTable);
        }
        private List<Track> ConvertTrackDataTableToList(DataTable dt)
        {
            List<Track> trackList = new List<Track>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Track track = new Track();
                track.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                track.Album = dt.Rows[i]["Album"].ToString();
                track.Artist = dt.Rows[i]["Artist"].ToString();
                track.Title = dt.Rows[i]["Title"].ToString();
                track.Year = Convert.ToInt32(dt.Rows[i]["Year"]);

                String length = dt.Rows[i]["Length"].ToString();
                track.Length = this.StringToLength(length);

                track.IsMissing = Convert.ToBoolean(dt.Rows[i]["IsMissing"]);
                track.Path = dt.Rows[i]["Path"].ToString();
                track.FileName = dt.Rows[i]["FileName"].ToString();
                track.OrderInList = Convert.ToInt32(dt.Rows[i]["OrderInList"]);
                track.TrackIdInPlaylist = Convert.ToInt32(dt.Rows[i]["TrackIdInPlaylist"]);
                trackList.Add(track);
            }

            return trackList;
        }
        private double StringToLength(String length)
        {
            String[] lengthParts = length.Split(':');

            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            if (lengthParts.Length == 2)
            {
                minutes = Convert.ToInt32(lengthParts[0]);
                seconds = Convert.ToInt32(lengthParts[1]);
            }
            else if (lengthParts.Length == 3)
            {
                hours = Convert.ToInt32(lengthParts[0]);
                minutes = Convert.ToInt32(lengthParts[1]);
                seconds = Convert.ToInt32(lengthParts[2]);
            }
            TimeSpan t = new TimeSpan(0, hours, minutes, seconds, 0);

            return t.TotalSeconds;
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
            if(this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                DataTable reversedDt = this.trackListTable.Clone();
                for (var row = this.trackListTable.Rows.Count - 1; row >= 0; row--)
                    reversedDt.ImportRow(this.trackListTable.Rows[row]);
                this.trackListTable = reversedDt;

                this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
                this.SetTrackListDataGridView(this.trackListTable);
                ((PlaylistView)this.playlistView).CallSetCurrentTrackColorEvent();
            }
        }
        public void Shuffle()
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
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

                this.SaveTrackList(trackListTable, this.currentPlaylistId);
                this.SetTrackListDataGridView(trackListTable);
                ((PlaylistView)this.playlistView).CallSetCurrentTrackColorEvent();
            }
            
        }
        public void OrderByColumnEvent(object sender, ListEventArgs e)
        {
            this.OrderByColumn(e.StringField1);
        }
        private void OrderByColumn(String columnName)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                DataView dv = this.trackListTable.DefaultView;
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

                    DataTable reversedDt = this.trackListTable.Clone();
                    for (var row = this.trackListTable.Rows.Count - 1; row >= 0; row--)
                        reversedDt.ImportRow(this.trackListTable.Rows[row]);
                    this.trackListTable = reversedDt;
                }
                else if (this.columnOrderStates[columnName] == 1)
                {
                    this.columnOrderStates[columnName] = 0;
                    this.trackListTable = sortedDT;
                }

                this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
                this.SetTrackListDataGridView(this.trackListTable);
                ((PlaylistView)this.playlistView).CallSetCurrentTrackColorEvent();
            }
        }
        #endregion

        #region TRACKLIST - TRACK COPY TO PLAYLIST

        private void InternalDragAndDropIntoTracklistEvent(object sender, ListEventArgs e)
        {
            List<Track> sourceTrackList = new List<Track>();
            if (e.SelectedRows != null && e.SelectedRows.Count > 0)
            {
                for(int i = 0;i<= e.SelectedRows.Count - 1; i++)
                {
                    int trackId = (int)e.SelectedRows[i].Cells["Id"].Value;
                    Track track = this.trackDao.GetTrack(trackId, this.tagList);
                    sourceTrackList.Add(track);
                }

               // sourceTrackList = this.ConvertSelectedRowsToList(e.SelectedRows);
                if(sourceTrackList != null && sourceTrackList.Count > 0)
                {
                    this.AddTracksToPlaylist(this.currentPlaylistId, sourceTrackList, e.IntegerField1, true);
                }
            }
        }
        private void InternalDragAndDropIntoPlaylistEvent(object sender, ListEventArgs e)
        {
            List<Track> sourceTrackList = new List<Track>();
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
                        Track track = this.trackDao.GetTrack(trackId, this.tagList);
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
        /*private List<Track> ConvertSelectedRowsToList(DataGridViewSelectedRowCollection selectedRows)
        {
            List<Track> trackList = new List<Track>();

            for (int i = 0; i <= selectedRows.Count - 1; i++)
            {
                Track track = new Track();
                track.Id = Convert.ToInt32(selectedRows[i].Cells["Id"].Value);
                track.Album = selectedRows[i].Cells["Album"].Value.ToString();
                track.Artist = selectedRows[i].Cells["Artist"].Value.ToString();
                track.Title = selectedRows[i].Cells["Title"].Value.ToString();
                track.Year = Convert.ToInt32(selectedRows[i].Cells["Year"].Value);

                String length = selectedRows[i].Cells["Length"].Value.ToString();
                track.Length = this.StringToLength(length);

                track.IsMissing = Convert.ToBoolean(selectedRows[i].Cells["IsMissing"].Value);
                track.Path = selectedRows[i].Cells["Path"].Value.ToString();
                track.FileName = selectedRows[i].Cells["FileName"].Value.ToString();
                track.OrderInList = Convert.ToInt32(selectedRows[i].Cells["OrderInList"].Value);
                track.TrackIdInPlaylist = Convert.ToInt32(selectedRows[i].Cells["TrackIdInPlaylist"].Value);


                trackList.Add(track);
            }

            return trackList;
        }*/
        private string[] scannedFileNames;
        private List<Track> ReadFiles(string[] fileNames)
        {
            List<String> filePathList = new List<String>();
            List<Track> trackList = new List<Track>();
            if (fileNames != null && fileNames.Length > 0)
            {
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

                List<Tag> tagList = this.tagDao.GetAllTag();

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

                    if (!System.IO.File.Exists(path))
                    {
                        track.Artist = fileName;
                        track.IsMissing = true;
                    }
                    else
                    {
                        Track trackFromDb = this.trackDao.GetTrackByPath(track.Path, tagList);
                        if (trackFromDb != null)
                            track = trackFromDb;

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
                            track.Artist = fileName;
                        }
                    }

                    if (track.Id == -1)
                    {
                        track.Id = this.trackDao.GetNextId(TableName.Track.ToString());

                        track.TrackTagValues = new List<TrackTagValue>();

                        this.trackDao.CreateTrack(track);

                        foreach (Tag tag in tagList)
                        {
                            TrackTagValue ttv = new TrackTagValue();
                            ttv.Id = this.trackDao.GetNextId(TableName.TrackTagValue.ToString());
                            ttv.TrackId = track.Id;
                            ttv.TagId = tag.Id;
                            ttv.TagName = tag.Name;
                            ttv.TagValueId = -1;
                            ttv.TagValueName = String.Empty;
                            this.trackDao.CreateTrackTagValue(ttv);
                        }
                    }
                    trackList.Add(track);

                }
            }
            return trackList;
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
        private void ExternalDragAndDropIntoTracklistEvent(object sender, ListEventArgs e)
        {
            List<Track> trackList = new List<Track>();
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
        private void ExternalDragAndDropIntoPlaylistEvent(object sender, ListEventArgs e)
        {
            List<Track> trackList = new List<Track>();
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
        private void AddTracksToPlaylist(int playlistId, List<Track> trackList, int dragIndex = -1, bool internalDragAndDrop = false)
        {
            List<Track> playlistTracklist = this.trackDao.GetTracklistByPlaylistId(playlistId, this.tagList);

            if (trackList != null && trackList.Count > 0)
            {
                int orderInList = playlistTracklist.Count;
                foreach (Track track in trackList)
                {
                    PlaylistContent plc = new PlaylistContent();
                    plc.Id = this.trackDao.GetNextId(TableName.PlaylistContent.ToString());
                    plc.PlaylistId = playlistId;
                    plc.TrackId = track.Id;
                    plc.OrderInList = orderInList;
                    plc.TrackIdInPlaylist = this.trackDao.GetNextSmallestTrackIdInPlaylist();

                    if (internalDragAndDrop && track.TrackIdInPlaylist == this.mediaPLayerComponent.CurrentTrackIdInPlaylist)
                    {
                        this.mediaPLayerComponent.CurrentTrackIdInPlaylist = plc.TrackIdInPlaylist;
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
                        if (this.mediaPLayerComponent.MediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                        {
                            this.mediaPLayerComponent.SetCurrentTrackIndex(0);
                            this.PlayTrack();
                        }
                    }
                   
                }

            }
            
        }

        private void MovePlaylistEvent(object sender, ListEventArgs e)
        {
            int oldId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);

            DataRow row = this.playlistListTable.NewRow();
            row["Id"] = -1;
            row["G"] = this.playlistListTable.Rows[e.IntegerField1]["G"];
            row["Name"] = this.playlistListTable.Rows[e.IntegerField1]["Name"];
            row["OrderInList"] = this.playlistListTable.Rows[e.IntegerField1]["OrderInList"];
            row["ProfileId"] = this.playlistListTable.Rows[e.IntegerField1]["ProfileId"];
            row["IsActive"] = this.playlistListTable.Rows[e.IntegerField1]["IsActive"];

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

            this.SavePlaylistList(this.playlistListTable);
            this.SetPlaylistDatagGridView(this.playlistListTable);
        }
        internal void CallAddTrackToTrackListEvent(List<Track> trackList, int dragIndex)
        {
            this.AddTracksToPlaylist(this.currentPlaylistId, trackList, dragIndex);
        }
        private void LoadTrackList(List<Track> trackList, int dragIndex)
        {
            if (trackList != null && trackList.Count > 0)
            {
                foreach (Track track in trackList)
                {
                    String length = this.LengthToString(track.Length);
                    if (dragIndex > -1)
                    {
                        DataRow userRow = trackListTable.NewRow();
                        userRow["Id"] = track.Id;
                        userRow["Album"] = track.Album;
                        userRow["Artist"] = track.Artist;
                        userRow["Title"] = track.Title;
                        userRow["Year"] = track.Year;
                        userRow["Length"] = length;
                        userRow["IsMissing"] = track.IsMissing;
                        userRow["Path"] = track.Path;
                        userRow["FileName"] = track.FileName;
                        userRow["OrderInList"] = track.OrderInList;
                        userRow["TrackIdInPlaylist"] = track.TrackIdInPlaylist;
                        if (track.TrackTagValues != null)
                        {
                            foreach (TrackTagValue ttv in track.TrackTagValues)
                            {
                                userRow[ttv.TagName] = ttv.TagValueName;
                            }
                        }
                        this.trackListTable.Rows.InsertAt(userRow, dragIndex);
                    }
                    else
                    {
                        object[] args = null;
                        if (track.TrackTagValues != null)
                        {
                            args = new object[11 + track.TrackTagValues.Count];
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
                                if (ttv.HasValue)
                                {
                                    args[i] = ttv.Value;
                                }
                                else
                                {
                                    args[i] = ttv.TagValueName;
                                }
                                i++;
                            }
                        }
                        
                        this.trackListTable.Rows.Add(args);
                    }
                }

                this.SetTrackListDataGridView(trackListTable);
                this.SaveTrackList(trackListTable, this.currentPlaylistId);
                ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
            }
        }
        #endregion

        #region TRACKLIST - REMOVE TRACKS
        private void DeleteTracksEvent(object sender, ListEventArgs e)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                for (int i = e.Rows.Count - 1; i >= 0; i--)
                {
                    if (e.Rows[i].Selected)
                    {
                        this.trackListTable.Rows[i].Delete();
                    }
                }
               // this.SaveTrackList(trackListTable, this.currentPlaylistId);
                this.SetTrackListDataGridView(trackListTable);
                ((PlaylistView)this.playlistView).CallSetCurrentTrackColorEvent();
                ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
            }
        }
        public void RemoveMissingTracks()
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                for (int i = this.trackListTable.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(this.trackListTable.Rows[i]["IsMissing"]))
                    {
                        this.trackListTable.Rows[i].Delete();
                    }
                }

                this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
                this.SetTrackListDataGridView(this.trackListTable);
                ((PlaylistView)this.playlistView).CallSetCurrentTrackColorEvent();
                ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
            } 
        }
        public void RemoveDuplicatedTracks()
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                List<int> trackIds = new List<int>();
                List<int> trackIdsToRemove = new List<int>();

                int trackIdInPlaylist = 0;
                int rowIndexToKeep = 0;

                for (int i = this.trackListTable.Rows.Count - 1; i >= 0; i--)
                {
                    if (!trackIds.Contains(Convert.ToInt32(this.trackListTable.Rows[i]["Id"])))
                    {
                        trackIdInPlaylist = Convert.ToInt32(this.trackListTable.Rows[i]["TrackIdInPlaylist"]);
                        if (this.mediaPLayerComponent.CurrentTrackIdInPlaylist != trackIdInPlaylist)
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


                this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
                this.SetTrackListDataGridView(this.trackListTable);
                ((PlaylistView)this.playlistView).CallSetCurrentTrackColorEvent();
                ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
            }
        }
        public void Clear()
        {
            this.trackListTable.Rows.Clear();

            this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
            this.SetTrackListDataGridView(this.trackListTable);
            ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
        }
        #endregion

        #region PLAYER
        public void SetCurrentTrackEvent(object sender, ListEventArgs e)
        {
            this.mediaPLayerComponent.SetCurrentTrackIndex(e.IntegerField1);
        }
        public void PlayTrackEvent(object sender, ListEventArgs e)
        {
            this.PlayTrack();
        }
        public void PlayTrack()
        {
            MediaPlayerUpdateState updateState = this.mediaPLayerComponent.PlayTrack();

            if (updateState == MediaPlayerUpdateState.AfterPlay)
            {
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex(), this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
            }
            else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
            {
                this.playlistView.UpdateAfterPlayTrackAfterPause();
            }
        }
        public void PauseTrackEvent(object sender, EventArgs e)
        {
            this.mediaPLayerComponent.PauseTrack();
            this.playlistView.UpdateAfterPauseTrack();
        }
        public void StopTrackEvent(object sender, EventArgs e)
        {
            this.mediaPLayerComponent.StopTrack();
            this.playlistView.UpdateAfterStopTrack();
        }
        public void PrevTrackEvent(object sender, ListEventArgs e)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                MediaPlayerUpdateState updateState = this.mediaPLayerComponent.PrevTrack();

                if (updateState == MediaPlayerUpdateState.AfterPlay)
                {
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex(), this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
                }
                else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                {
                    this.playlistView.UpdateAfterPlayTrackAfterPause();
                }
            }
            
        }
        public void NextTrackEvent(object sender, ListEventArgs e)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                MediaPlayerUpdateState updateState = this.mediaPLayerComponent.NextTrack();

                if (updateState == MediaPlayerUpdateState.AfterPlay)
                {
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex(), this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
                }
                else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                {
                    this.playlistView.UpdateAfterPlayTrackAfterPause();
                }
            }
            
        }
        public void RandomTrackEvent(object sender, EventArgs e)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                MediaPlayerUpdateState updateState = this.mediaPLayerComponent.RandomTrack();
                if (updateState == MediaPlayerUpdateState.AfterPlay)
                {
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex(), this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
                }
                else
                {
                    this.playlistView.UpdateAfterPlayTrackAfterPause();
                }
            }
             
        }
        private void GetMediaPlayerProgressStatusEvent(object sender, EventArgs e)
        {
            double duration = mediaPLayerComponent.GetDuration();
            String durationString = mediaPLayerComponent.GetDurationString();
            double currentPosition = mediaPLayerComponent.GetCurrentPosition();
            String currentPositionString = mediaPLayerComponent.GetCurrentPositionString();

            if (duration > 0 && currentPosition >= duration - 0.3)
            {
                MediaPlayerUpdateState updateState = this.mediaPLayerComponent.NextTrack();

                if (updateState == MediaPlayerUpdateState.AfterPlay)
                {
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex(), this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
                }
                else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                {
                    this.playlistView.UpdateAfterPlayTrackAfterPause();
                }
            }

            this.playlistView.UpdateMediaPlayerProgressStatus(duration, durationString, currentPosition, currentPositionString);
        }
        private void SetCurrentTrackColorEvent(object sender, ListEventArgs e)
        {
            this.playlistView.SetCurrentTrackColor(this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
        }
        internal void CallChangeProgressEvent(int currentPosX, int width)
        {
            this.mediaPLayerComponent.ChangeProgress(currentPosX, width);
        }
        internal void CallChangeVolumeEvent(int volume)
        {
            this.mediaPLayerComponent.ChangeVolume(volume);
            this.settingDao.SetIntegerSetting(Settings.Volume.ToString(), volume);
        }
        #endregion

        #region PLAYLIST


        private void CreatePlaylist(object sender, EventArgs e)
        {
            PlaylistEditorView playlistEditorView = new PlaylistEditorView();
            PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(playlistEditorView, this.trackDao, this.settingDao);
            if (playlistEditorView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
            {
                this.playlistListTable.Rows.Add(presenter.newPlaylist.Id, presenter.newPlaylist.QuickListGroup, presenter.newPlaylist.Name, presenter.newPlaylist.OrderInList, presenter.newPlaylist.ProfileId, presenter.newPlaylist.IsActive);
                this.SavePlaylistList(playlistListTable);
                this.SetPlaylistDatagGridView(playlistListTable);
                ((PlaylistView)this.playlistView).SetCurrentPlaylistColor(this.currentPlaylistId);
            }
        }
        private void EditPlaylist(object sender, ListEventArgs e)
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
                    //this.playlistListTable.Rows[playlistIndex]["Hotkey"] = presenter.newPlaylist?.Hotkey;

                    this.SavePlaylistList(playlistListTable);
                    this.SetPlaylistDatagGridView(playlistListTable);
                    ((PlaylistView)this.playlistView).SetCurrentPlaylistColor(this.currentPlaylistId);
                }
            }
        }

        private void LoadPlaylistEvent(object sender, ListEventArgs e)
        {
            this.LoadPlaylist(e.IntegerField1);
        }
        private void LoadPlaylist(int playlistIndex)
        {
            if(playlistIndex != -1)
            {
                this.currentPlaylistId = Convert.ToInt32(this.playlistListTable.Rows[playlistIndex]["Id"]);
                this.trackDao.SetActivePlaylist(this.currentPlaylistId);

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

                this.trackListTable.Rows.Clear();

                //this.mediaPLayerComponent.Initialize(this.trackListTable);
                this.mediaPLayerComponent.Initialize2(this.trackListTable);

                Playlist playlist = this.trackDao.GetActivePlaylist();
                this.LoadPlaylist(playlist);

                //ha a szám ebben a listából szól a lejátszóban, ki kellene szinezni
                if (this.mediaPLayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying ||
                    this.mediaPLayerComponent.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
                {
                    int currentTrackIdInPlaylist = this.mediaPLayerComponent.CurrentTrackIdInPlaylist;
                    int trackIdInPlaylist = -1;
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

            }
            
        }
        private void DeletePlaylistEvent(object sender, ListEventArgs e)
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

                        this.SavePlaylistList(this.playlistListTable);
                        this.SetPlaylistDatagGridView(this.playlistListTable);

                        if (playlistId == this.currentPlaylistId)
                        {
                            this.trackListTable.Rows.Clear();
                            this.currentPlaylistId = Convert.ToInt32(this.playlistListTable.Rows[0]["Id"]);

                            this.LoadPlaylist(this.currentPlaylistId);

                            this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
                            this.SetTrackListDataGridView(this.trackListTable);
                            this.SetPlaylistAsCurrent(this.currentPlaylistId);

                            ((PlaylistView)this.playlistView).SetCurrentPlaylistColor(this.currentPlaylistId);
                            ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);

                        }
                    }
                }

            }
        }
        private void SetPlaylistAsCurrent(int playlistId)
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
        }
        private void SetQuickListEvent(object sender, ListEventArgs e)
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
            this.SetPlaylistDatagGridView(this.playlistListTable);
        }
        private void ExportToM3UEvent(object sender, ListEventArgs e)
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
                    List<Track> trackList = this.trackDao.GetTracklistByPlaylistId(playlistId, this.tagList);
                    if (trackList != null && trackList.Count > 0)
                    {
                        foreach (Track track in trackList)
                        {
                            row = "#EXTVDJ:";
                            row += track.Length.ToString() + "," ;
                            if (!String.IsNullOrEmpty(track.Title))
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
        private void ExportToTXTEvent(object sender, ListEventArgs e)
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
                    List<Track> trackList = this.trackDao.GetTracklistByPlaylistId(playlistId, this.tagList);
                    if (trackList != null && trackList.Count > 0)
                    {
                        foreach (Track track in trackList)
                        {
                            row = index.ToString() + ". ";
                            
                            if (!String.IsNullOrEmpty(track.Title))
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
        private void ExportToDirectoryEvent(object sender, ListEventArgs e)
        {
            ExportToDirectoryView exportToDirectoryView = new ExportToDirectoryView();

            int playlistId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);
            List<Track> trackList = this.trackDao.GetTracklistByPlaylistId(playlistId, this.tagList);
            if (trackList != null && trackList.Count > 0)
            {
                ExportToDirectoryPresenter presenter = new ExportToDirectoryPresenter(exportToDirectoryView, trackList, this.tagDao, this.settingDao);

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
                this.TrackColumnVisibilityArray = presenter.TrackPropertyList.Select(x => x.IsEnabled).ToArray();

                int i = 0;
                foreach(TrackProperty tp in presenter.TrackPropertyList)
                {
                    tp.SortingId = i;
                    this.settingDao.UpdateTrackProperty(tp);
                    i++;
                }
                this.InitializeDataTables();
                ((PlaylistView)this.playlistView).CallSetCurrentTrackColorEvent();
            }
        }
        private void ScanBpmEvent(object sender, EventArgs e)
        {
            VirtualDJReader vdjReader = new VirtualDJReader(this.trackDao, this.settingDao, null);

            if (this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {

                for (int i = 0; i <= this.trackListTable.Rows.Count - 1; i++) {

                    String key = Convert.ToString(this.trackListTable.Rows[i]["Key"]);
                    String bpm = Convert.ToString(this.trackListTable.Rows[i]["Bpm"]);

                    if(String.IsNullOrEmpty(key) || String.IsNullOrEmpty(bpm))
                    {
                        VirtualDJReader.Instance.ReadKeyAndBpmFromVirtualDJDatabase(Convert.ToString(this.trackListTable.Rows[i]["Path"]), ref key, ref bpm);

                        int id = Convert.ToInt32(this.trackListTable.Rows[i]["Id"]);
                        Track track = this.trackDao.GetTrack(id, this.tagList);
                        if(track != null)
                        {
                            foreach (Tag tag in this.tagList)
                            {
                                if (tag.Name == "Key")
                                {
                                    List<TagValue> keyList = this.tagDao.GetTagValuesByTagId(tag.Id);
                                    if (track.TrackTagValues != null)
                                    {
                                        TrackTagValue ttv = track.TrackTagValues.Find(x => x.TagId == tag.Id);
                                        if (keyList != null && keyList.Count > 0 && ttv != null)
                                        {
                                            // vdjReader.ReadKeyFromVirtualDJDatabase(track.Path, ref ttv, keyList);

                                            TagValue keyTagValue = keyList.Find(x => x.Name == key);
                                            if (keyTagValue != null)
                                            {
                                                ttv.TagValueId = keyTagValue.Id;
                                                ttv.TagValueName = keyTagValue.Name;
                                            }

                                            this.trackListTable.Rows[i]["Key"] = ttv.TagValueName;
                                            this.trackDao.UpdateTrackTagValue(ttv);
                                        }
                                    }

                                }
                                else if (tag.Name == "Bpm")
                                {
                                    List<TagValue> keyList = this.tagDao.GetTagValuesByTagId(tag.Id);
                                    if (track.TrackTagValues != null)
                                    {
                                        TrackTagValue ttv = track.TrackTagValues.Find(x => x.TagId == tag.Id);
                                        if (keyList != null && keyList.Count > 0)
                                        {
                                            TagValue keyTagValue = keyList.Find(x => x.Name == tag.Name);
                                            if (keyTagValue != null)
                                            {
                                                ttv.TagValueId = keyTagValue.Id;
                                                ttv.TagValueName = keyTagValue.Name;
                                                ttv.Value = bpm;
                                                ttv.HasValue = true;
                                            }

                                            this.trackListTable.Rows[i]["Bpm"] = ttv.Value;
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
        private void DisplayTagEditorEvent(object sender, EventArgs e)
        {
            this.isTagEditorDisplayed = !this.isTagEditorDisplayed;

            bool inputTextBoxEnabled = false;
            if(this.currentTag != null && this.currentTag.HasMultipleValues)
            {
                inputTextBoxEnabled = true;
            }
            this.settingDao.SetBooleanSetting(Settings.IsTagEditorDisplayed.ToString(), this.isTagEditorDisplayed);
            ((PlaylistView)this.playlistView).CallDisplayTagEditor(this.isTagEditorDisplayed, inputTextBoxEnabled);
        }
        
        private void DisplayPlaylistListEvent(object sender, EventArgs e)
        {
            this.isPlaylistListDisplayed = !this.isPlaylistListDisplayed;
            this.settingDao.SetBooleanSetting(Settings.IsPlaylistListDisplayed.ToString(), this.isPlaylistListDisplayed);
            ((PlaylistView)this.playlistView).CallDisplayPlaylistList(this.isPlaylistListDisplayed);
        }

        private void SelectTagEvent(object sender, ListEventArgs e)
        {
            if(this.tagList != null && this.tagList.Count > 0)
            {
                Tag tag = tagList[e.IntegerField1];
                if(tag != null)
                {
                    this.currentTag = tag;
                    List<TagValue> tagValueList = this.tagDao.GetTagValuesByTagId(tag.Id);
                    if(tagValueList != null && tagValueList.Count > 0)
                    {
                        this.tagValueList = tagValueList;

                        bool inputTextBoxEnabled = false;
                        if (this.currentTag != null && this.currentTag.HasMultipleValues)
                        {
                            inputTextBoxEnabled = true;
                        }

                        List<List<TagValue>> tagValueListContainer = new List<List<TagValue>>();
                        tagValueListContainer.Add(tagValueList);

                        ((PlaylistView)this.playlistView).InitializeTagValueEditor2(this.tagList, tagValueListContainer, this.isTagEditorDisplayed);
                        ((PlaylistView)this.playlistView).CallDisplayTagEditor(this.isTagEditorDisplayed, inputTextBoxEnabled);
                    }
                }
            }
           
        }

        private void ClearTagValueEvent(object sender, ListEventArgs e)
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
                            for (int i = e.Rows.Count - 1; i >= 0; i--)
                            {
                                if (e.Rows[i].Selected)
                                {
                                    Track track = this.trackDao.GetTrack(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                    if (track != null)
                                    {
                                        foreach (TrackTagValue ttv in track.TrackTagValues)
                                        {
                                            if (ttv.TagId == currentTag.Id)
                                            {
                                                ttv.TagValueName = String.Empty;
                                                this.trackDao.UpdateTrackTagValue(ttv);
                                                break;
                                            }
                                        }
                                    }
                                    this.trackListTable.Rows[i][currentTag.Name] = "";
                                }
                            }
                            this.SaveTrackList(trackListTable, this.currentPlaylistId);
                            this.SetTrackListDataGridView(trackListTable);

                        }

                    }
                }
            }
        }

        private void SetTagValueEvent(object sender, ListEventArgs e)
        {
            if(this.tagList != null && this.tagList.Count > 0)
            {
                Tag currentTag = this.tagList.Find(x => x.Name == e.StringField1);
                if(currentTag != null){

                    List<TagValue> tagValueList = this.tagDao.GetTagValuesByTagId(currentTag.Id);
                    if(tagValueList != null && tagValueList.Count > 0){
                        if (currentTag.HasMultipleValues)
                        {
                            TagValue tv = tagValueList[0];
                            if (tv != null)
                            {
                                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                                {
                                    for (int i = e.Rows.Count - 1; i >= 0; i--)
                                    {
                                        if (e.Rows[i].Selected)
                                        {
                                            Track track = this.trackDao.GetTrack(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                            if (track != null)
                                            {
                                                foreach (TrackTagValue ttv in track.TrackTagValues)
                                                {
                                                    if (ttv.TagId == currentTag.Id)
                                                    {
                                                        ttv.TagValueId = tv.Id;
                                                        ttv.TagValueName = tv.Name;
                                                        ttv.HasValue = true;
                                                        ttv.Value = e.StringField3;
                                                        this.trackDao.UpdateTrackTagValue(ttv);
                                                        break;
                                                    }
                                                }
                                            }
                                            this.trackListTable.Rows[i][currentTag.Name] = e.StringField3;
                                        }
                                    }
                                    this.SaveTrackList(trackListTable, this.currentPlaylistId);
                                    this.SetTrackListDataGridView(trackListTable);

                                }
                            }
                        }
                        else
                        {
                            TagValue tv = tagValueList.Find(x => x.Name == e.StringField2);
                            if (tv != null)
                            {
                                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                                {
                                    for (int i = e.Rows.Count - 1; i >= 0; i--)
                                    {
                                        if (e.Rows[i].Selected)
                                        {
                                            Track track = this.trackDao.GetTrack(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
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
                                        }
                                    }
                                    this.SaveTrackList(trackListTable, this.currentPlaylistId);
                                    this.SetTrackListDataGridView(trackListTable);

                                }
                            }

                        }
                    }

                   
                }
            }
        }
        /*
        private void SetTagValueEvent(object sender, ListEventArgs e)
        {
            if (this.currentTag != null)
            {
                String result = String.Empty;
                if (this.currentTag.HasMultipleValues)
                {
                    TagValue tv = this.tagValueList[0];
                    if (tv != null)
                    {
                        if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                        {
                            for (int i = e.Rows.Count - 1; i >= 0; i--)
                            {
                                if (e.Rows[i].Selected)
                                {
                                    Track track = this.trackDao.GetTrack(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                    if (track != null)
                                    {
                                        foreach (TrackTagValue ttv in track.TrackTagValues)
                                        {
                                            if (ttv.TagId == this.currentTag.Id)
                                            {
                                                ttv.TagValueId = tv.Id;
                                                ttv.TagValueName = tv.Name;
                                                ttv.HasValue = true;
                                                ttv.Value = e.StringField1;
                                                this.trackDao.UpdateTrackTagValue(ttv);
                                                break;
                                            }
                                        }
                                    }
                                    this.trackListTable.Rows[i][this.currentTag.Name] = e.StringField1;
                                }
                            }
                            this.SaveTrackList(trackListTable, this.currentPlaylistId);
                            this.SetTrackListDataGridView(trackListTable);

                        }
                    }
                }
                else
                {
                    TagValue tv = this.tagValueList[e.IntegerField1];
                    if (tv != null)
                    {
                        if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                        {
                            for (int i = e.Rows.Count - 1; i >= 0; i--)
                            {
                                if (e.Rows[i].Selected)
                                {
                                    Track track = this.trackDao.GetTrack(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                                    if (track != null)
                                    {
                                        foreach (TrackTagValue ttv in track.TrackTagValues)
                                        {
                                            if (ttv.TagId == this.currentTag.Id)
                                            {
                                                ttv.TagValueId = tv.Id;
                                                ttv.TagValueName = tv.Name;
                                                this.trackDao.UpdateTrackTagValue(ttv);
                                                break;
                                            }
                                        }
                                    }
                                    this.trackListTable.Rows[i][this.currentTag.Name] = tv.Name;
                                }
                            }
                            this.SaveTrackList(trackListTable, this.currentPlaylistId);
                            this.SetTrackListDataGridView(trackListTable);

                        }
                    }
                   
                }
            }
           
            
        }
        */
    }
}
