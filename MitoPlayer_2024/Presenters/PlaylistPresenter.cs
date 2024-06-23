using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using TagLib.Mpeg4;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Type = System.Type;

namespace MitoPlayer_2024.Presenters
{
    
    public class PlaylistPresenter
    {
        private IPlaylistView view { get; set; }
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
        Dictionary<string, int> columnOrderStates { get; set; }
        private MediaPlayerComponent mediaPLayerComponent { get; set; }

        private bool isTagEditorDisplayed { get; set; }
        private List<Tag> tagList { get; set; }
        private List<TagValue> tagValueList { get; set; }
        private Tag currentTag { get; set; }
        private TagValue currentTagValue { get; set; }

        public PlaylistPresenter(IPlaylistView view, MediaPlayerComponent mediaPlayer, ITrackDao trackDao, ITagDao tagValueDao, ISettingDao settingDao)
        {
            //INITIALIZE
            this.view = view;
            this.trackDao = trackDao;
            this.tagDao = tagValueDao;
            this.settingDao = settingDao;

            this.InitializeDataTables();

            this.mediaPLayerComponent = mediaPlayer;
            this.mediaPLayerComponent.Initialize(this.trackListTable);

            this.InitializeTagEditor();
            this.InitializeVolume();

            //MEDIA PLAYER
            this.view.SetCurrentTrackEvent += SetCurrentTrackEvent;
            this.view.PlayTrackEvent += PlayTrackEvent;
            this.view.PauseTrackEvent += PauseTrackEvent;
            this.view.StopTrackEvent += StopTrackEvent;
            this.view.PrevTrackEvent += PrevTrackEvent;
            this.view.NextTrackEvent += NextTrackEvent;
            this.view.RandomTrackEvent += RandomTrackEvent;
            this.view.GetMediaPlayerProgressStatusEvent += GetMediaPlayerProgressStatusEvent;
            this.view.SetCurrentTrackColorEvent += SetCurrentTrackColorEvent;

            //TRACKLIST
            this.view.OrderByColumnEvent += OrderByColumnEvent;
            this.view.DeleteTracksEvent += DeleteTracksEvent;
            this.view.TrackDragAndDropEvent += TrackDragAndDropEvent;
            this.view.CopyTracksToPlaylistViaHotkeyEvent += CopyTracksToPlaylistViaHotkeyEvent;

            this.view.InternalDragAndDropIntoTracklistEvent += InternalDragAndDropIntoTracklistEvent;
            this.view.InternalDragAndDropIntoPlaylistEvent += InternalDragAndDropIntoPlaylistEvent;
            this.view.ExternalDragAndDropIntoTracklistEvent += ExternalDragAndDropIntoTracklistEvent;
            this.view.ExternalDragAndDropIntoPlaylistEvent += ExternalDragAndDropIntoPlaylistEvent;

            this.view.ShowColumnVisibilityEditorEvent += ShowColumnVisibilityEditorEvent;

            //PLAYLIST
            this.view.ShowPlaylistEditorViewEvent += ShowPlaylistEditorViewEvent;
            this.view.LoadPlaylistEvent += LoadPlaylistEvent;
            this.view.DeletePlaylistEvent += DeletePlaylistEvent;
            this.view.SetQuickListEvent += SetQuickListEvent;
            this.view.ExportToM3UEvent += ExportToM3UEvent;
            this.view.ExportToTXTEvent += ExportToTXTEvent;

            //TAG EDITOR
            this.view.DisplayTagEditorEvent += DisplayTagEditorEvent;
            this.view.SelectTagEvent += SelectTagEvent;
            this.view.SetTagValueEvent += SetTagValueEvent;

            this.view.Show();
        }

        #region INITIALIZE
        private void InitializeVolume()
        {
            int volume = this.settingDao.GetIntegerSetting(Settings.Volume.ToString());
            if (volume == -1)
                volume = 50;
            this.view.SetVolume(volume);
            this.mediaPLayerComponent.MediaPlayer.settings.volume = volume;
        }
        private void InitializeTagEditor()
        {
            this.isTagEditorDisplayed = false;
            List<Tag> tagList = this.tagDao.GetAllTag();
            if (tagList != null && tagList.Count > 0)
            {
                this.tagList = tagList;
            }
            ((PlaylistView)this.view).InitializeTagEditor(this.tagList);
            ((PlaylistView)this.view).InitializeTagValueEditor(null);
            ((PlaylistView)this.view).CallDisplayTagEditor(this.isTagEditorDisplayed);
        }
        private void InitializeDataTables()
        {
            this.InitializePlaylistDataTable();
            this.InitializeTracklistDataTable();
            this.InitializePlaylistListAndTrackList();
        }
        private void InitializePlaylistDataTable()
        {
            //PLAYLIST GRID
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

            this.SetPlaylistList(this.playlistListTable);
        }
        private void InitializeTracklistDataTable() { 
            //TRACKLIST GRID
            this.trackListBindingSource = new BindingSource();
            this.trackListTable = new DataTable();
            this.columnOrderStates = new Dictionary<string, int>();
            List<TrackProperty> tpList = new List<TrackProperty>();
            tpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString());
            if (tpList != null && tpList.Count > 0)
            {
                foreach (TrackProperty tp in tpList)
                {
                    this.columnOrderStates.Add(tp.Name, -1);
                    this.trackListTable.Columns.Add(tp.Name, Type.GetType(tp.Type));
                }
                this.TrackColumnVisibilityArray = tpList.Select(x => x.IsEnabled).ToArray();
                this.TrackColumnSortingIdArray = tpList.Select(x => x.SortingId).ToArray();
            }

            this.SetTrackList(this.trackListTable);

            this.selectedTrackListBindingSource = new BindingSource();
            this.selectedTrackListTable = new DataTable();
            if (tpList != null && tpList.Count > 0)
            {
                foreach (TrackProperty tp in tpList)
                {
                    this.selectedTrackListTable.Columns.Add(tp.Name, Type.GetType(tp.Type));
                }
            }

            this.SetSelectedTrackList(this.selectedTrackListTable);
        }
        private void InitializePlaylistListAndTrackList()
        {
            this.playlistListTable.Clear();

            List<Playlist> plsList = this.trackDao.GetAllPlaylist();
            foreach (Playlist playlist in plsList)
                this.playlistListTable.Rows.Add(playlist.Id,playlist.QuickListGroup, playlist.Name, playlist.OrderInList, playlist.ProfileId, playlist.IsActive);

            Playlist activePlaylist = this.trackDao.GetActivePlaylist();
            this.currentPlaylistId = activePlaylist.Id;
            this.LoadPlaylist(activePlaylist);

            this.SetPlaylistList(playlistListTable);
        }
        private void LoadPlaylist(Playlist pls)
        {
            List<Track> trackList = this.trackDao.GetTracklistByPlaylistId(pls.Id, tagList);
            if (trackList != null && trackList.Count > 0)
            {
                foreach (Track track in trackList)
                {
                    String length = this.LengthToString(track.Length);

                    int tagCount = 0;
                    if(track.TrackTagValues != null && track.TrackTagValues.Count > 0)
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

                    if(tagCount > 0)
                    {
                        int i = 11;
                        foreach (TrackTagValue ttv in track.TrackTagValues)
                        {
                            args[i] = ttv.TagValueName;
                            i++;
                        }
                    }
                    
                    this.trackListTable.Rows.Add(args);
                }
                this.SetTrackList(this.trackListTable);
            }
            ((PlaylistView)this.view).UpdateTrackCountAndLength(this.currentPlaylistId);
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
        #endregion

        #region DATATABLES
        private void SetPlaylistList(DataTable playlistListTable)
        {
            this.playlistListBindingSource.DataSource = playlistListTable;
            this.view.SetPlaylistListBindingSource(this.playlistListBindingSource, this.PlaylistColumnVisibilityArray, this.currentPlaylistId);
        }
        private void SetTrackList(DataTable trackListTable)
        {
            this.trackListBindingSource.DataSource = trackListTable;
            this.view.SetTrackListBindingSource(this.trackListBindingSource, this.TrackColumnVisibilityArray, this.TrackColumnSortingIdArray);
        }
        private void SetSelectedTrackList(DataTable trackListTable)
        {
            this.selectedTrackListBindingSource.DataSource = trackListTable;
            this.view.SetSelectedTrackListBindingSource(this.selectedTrackListBindingSource);
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
                this.SetTrackList(this.trackListTable);
                ((PlaylistView)this.view).CallSetCurrentTrackColorEvent();
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
                this.SetTrackList(trackListTable);
                ((PlaylistView)this.view).CallSetCurrentTrackColorEvent();
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
                this.SetTrackList(this.trackListTable);
                ((PlaylistView)this.view).CallSetCurrentTrackColorEvent();
            }
        }
        #endregion

        #region TRACKLIST - DRAG AND DROP
        private void TrackDragAndDropEvent(object sender, ListEventArgs e)
        {
            this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
            this.SetTrackList(this.trackListTable);
        }
        #endregion

        #region TRACKLIST - TRACK COPY TO PLAYLIST
        private void CopyTracksToPlaylistViaHotkeyEvent(object sender, ListEventArgs e)
        {
            if (e.SelectedRows != null && e.SelectedRows.Count > 0)
            {

            }


            int quickPlaylistId = -1;
            List<Track> sourceTrackList = null;
            List<Track> targetTrackList = null;
            int playlistIndex = 0;
            DataRow playlistRow = null;
            String playlistName = "";

            List<Tag> tagList = this.tagDao.GetAllTag();

            if (e.SelectedRows != null && e.SelectedRows.Count > 0)
            {
                sourceTrackList = this.ConvertSelectedRowsToList(e.SelectedRows);
                DataRow[] elements = this.playlistListTable.Select("G = " + e.IntegerField1);
                if(elements != null && elements.Count() > 0)
                {
                    playlistRow = elements[0];
                }

                if (playlistRow != null)
                {
                    quickPlaylistId = Convert.ToInt32(playlistRow["Id"]);
                    playlistName = playlistRow["Name"].ToString();
                    targetTrackList = this.trackDao.GetTracklistByPlaylistId(quickPlaylistId, tagList);
                    if (targetTrackList != null)
                    {
                        this.CopyTracksToPlaylist(quickPlaylistId, sourceTrackList, targetTrackList);

                        if (this.currentPlaylistId == quickPlaylistId)
                        {
                            playlistIndex = this.playlistListTable.Rows.IndexOf(playlistRow);
                            this.LoadPlaylist(playlistIndex);
                        }
                    }
                }
            }

            if (sourceTrackList != null && sourceTrackList.Count > 0 && playlistRow != null)
                ((PlaylistView)this.view).UpdateAfterCopyTracksToPlaylist(sourceTrackList.Count, playlistName);
        }
        private void CopyTracksToPlaylistViaDragAndDropEvent(object sender, ListEventArgs e)
        {
            int playlistId = -1;
            List<Track> sourceTrackList = null;
            List<Track> targetTrackList = null;
            int playlistIndex = 0;
            DataRow playlistRow = null;
            String playlistName = "";

            if (e.SelectedRows != null && e.SelectedRows.Count > 0)
            {
                sourceTrackList = this.ConvertSelectedRowsToList(e.SelectedRows);
                playlistRow = this.playlistListTable.Rows[e.IntegerField1];

                if (playlistRow != null)
                {
                    playlistId = Convert.ToInt32(playlistRow["Id"]);
                    playlistName = playlistRow["Name"].ToString();
                    targetTrackList = this.trackDao.GetTracklistByPlaylistId(playlistId, this.tagList);
                    if (targetTrackList != null)
                    {
                        this.CopyTracksToPlaylist(playlistId, sourceTrackList, targetTrackList);

                        if (this.currentPlaylistId == playlistId)
                        {
                            playlistIndex = this.playlistListTable.Rows.IndexOf(playlistRow);
                            this.LoadPlaylist(playlistIndex);
                        }
                    }
                }
            }

            if (sourceTrackList != null && sourceTrackList.Count > 0 && playlistRow != null)
                ((PlaylistView)this.view).UpdateAfterCopyTracksToPlaylist(sourceTrackList.Count, playlistName);
        }
       
        internal void CopyTracksToPlaylist(int playlistId, List<Track> sourceTrackList, List<Track> targetTrackList)
        {
            if (sourceTrackList != null && sourceTrackList.Count > 0)
            {
                int orderInList = targetTrackList.Count;
                foreach (Track track in sourceTrackList)
                {
                    PlaylistContent plc = new PlaylistContent();
                    plc.Id = this.trackDao.GetNextId(TableName.PlaylistContent.ToString());
                    plc.PlaylistId = playlistId;
                    plc.TrackId = track.Id;
                    plc.OrderInList = orderInList;
                    plc.TrackIdInPlaylist = track.TrackIdInPlaylist;
                    this.trackDao.CreatePlaylistContent(plc);
                    orderInList++;
                }
            }
        }

        #endregion


        private void InternalDragAndDropIntoTracklistEvent(object sender, ListEventArgs e)
        {
            List<Track> sourceTrackList = null;
            if (e.SelectedRows != null && e.SelectedRows.Count > 0)
            {
                sourceTrackList = this.ConvertSelectedRowsToList(e.SelectedRows);
                if(sourceTrackList != null && sourceTrackList.Count > 0)
                {
                    this.AddTracksToPlaylist(this.currentPlaylistId, sourceTrackList, e.IntegerField1);

                    List<int> oldListTrackIdInPlaylistList = new List<int>();
                    for (int i = 0; i <= sourceTrackList.Count - 1; i++)
                    {
                        oldListTrackIdInPlaylistList.Add(sourceTrackList[i].TrackIdInPlaylist);
                    }
                }
            }
        }
        private void InternalDragAndDropIntoPlaylistEvent(object sender, ListEventArgs e)
        {
            List<Track> sourceTrackList = null;
            DataRow playlistRow = null;
            int playlistId = -1;

            if (e.SelectedRows != null && e.SelectedRows.Count > 0)
            {
                List<int> oldListTrackIdInPlaylistList = new List<int>();
                for (int i = 0; i <= this.trackListTable.Rows.Count - 1; i++)
                {
                    int trackIdInPlaylist = Convert.ToInt32(this.trackListTable.Rows[i]["TrackIdInPlaylist"]);
                    oldListTrackIdInPlaylistList.Add(trackIdInPlaylist);
                }

                sourceTrackList = this.ConvertSelectedRowsToList(e.SelectedRows);
                if (sourceTrackList != null && sourceTrackList.Count > 0)
                {
                    playlistRow = this.playlistListTable.Rows[e.IntegerField1];
                    if (playlistRow != null)
                    {
                        playlistId = Convert.ToInt32(playlistRow["Id"]);
                    }
                    this.AddTracksToPlaylist(playlistId, sourceTrackList);
                  //  ((PlaylistView)this.view).SetSelectionAfterDragAndDrop(oldListTrackIdInPlaylistList);
                }
            }
        }
        private List<Track> ConvertSelectedRowsToList(DataGridViewSelectedRowCollection selectedRows)
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
        }
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
        private void AddTracksToPlaylist(int playlistId, List<Track> trackList, int dragIndex = -1)
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
                    track.TrackIdInPlaylist = plc.TrackIdInPlaylist;
                    this.trackDao.CreatePlaylistContent(plc);
                    orderInList++;

                  /*  if (insertIntoDefault && fromDragAndDrop)
                    {
                        if (trackToReplaceInMediaPlayer != null &&
                            trackToReplaceInMediaPlayer.Id == track.Id &&
                            trackToReplaceInMediaPlayer.Path == track.Path)
                        {
                            this.mediaPLayerComponent.CurrentTrackIdInPlaylist = plc.TrackIdInPlaylist;
                        }
                    }*/
                }


                if(playlistId == this.currentPlaylistId)
                {
                    this.LoadTrackList(trackList, dragIndex);
                    if (this.mediaPLayerComponent.MediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        this.mediaPLayerComponent.SetCurrentTrackIndex(0);
                        this.PlayTrack();
                    }
                }



                /*  if (insertIntoDefault)
                  {
                      this.LoadTrackList(trackList, dragIndex);
                      if (this.mediaPLayerComponent.MediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                      {
                          this.mediaPLayerComponent.SetCurrentTrackIndex(0);
                          this.PlayTrack();
                      }
                  }

                  if (insertIntoDefault && fromDragAndDrop && fromExternalDragAndDrop)
                  {
                      List<int> oldListTrackIdInPlaylistList = new List<int>();
                      for (int i = 0; i <= trackList.Count - 1; i++)
                      {
                          oldListTrackIdInPlaylistList.Add(trackList[i].TrackIdInPlaylist);
                      }
                      this.SetTrackList(trackListTable);
                      this.SaveTrackList(trackListTable, this.currentPlaylistId);
                      ((PlaylistView)this.view).SetSelectionAfterExternalDragAndDrop(oldListTrackIdInPlaylistList);
                  }*/

            }

            /*
            Track trackToReplaceInMediaPlayer = null;
            if (insertIntoDefault && fromDragAndDrop)
            {
                PlaylistContent currentPlayedTrackPlc = this.trackDao.GetPlaylistContentByTrackIdInPlaylist(this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
                if (currentPlayedTrackPlc != null)
                {
                    trackToReplaceInMediaPlayer = this.trackDao.GetTrack(currentPlayedTrackPlc.TrackId, this.tagList);
                }
            }
            */
 

            
        }


        #region TRACKLIST - DRAG FILES FROM OUTSIDE




        internal void CallAddTrackToTrackListEvent(List<Track> trackList, int dragIndex)
        {
            this.AddTracksToPlaylist(this.currentPlaylistId, trackList, dragIndex);

            /*
            Track trackToReplaceInMediaPlayer = null;
            if (insertIntoDefault && fromDragAndDrop)
            {
                PlaylistContent currentPlayedTrackPlc = this.trackDao.GetPlaylistContentByTrackIdInPlaylist(this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
                if(currentPlayedTrackPlc != null)
                {
                    trackToReplaceInMediaPlayer = this.trackDao.GetTrack(currentPlayedTrackPlc.TrackId, this.tagList);
                }
            }

            DataRow playlistRow = null;
            int quickPlaylistId = -1;
            if (!insertIntoDefault && dragIndex > -1)
            {
                playlistRow = this.playlistListTable.Rows[dragIndex];
                if (playlistRow != null)
                {
                    quickPlaylistId = Convert.ToInt32(playlistRow["Id"]);
                }
            }

            if (trackList != null && trackList.Count > 0)
            {
                int orderInList = this.trackListTable.Rows.Count;
                foreach (Track track in trackList)
                {
                    PlaylistContent plc = new PlaylistContent();
                    plc.Id = this.trackDao.GetNextId(TableName.PlaylistContent.ToString());

                    if (!insertIntoDefault && dragIndex > -1)
                    {
                        plc.PlaylistId = quickPlaylistId;
                    }
                    else
                    {
                        plc.PlaylistId = this.currentPlaylistId;
                    }

                    plc.TrackId = track.Id;
                    plc.OrderInList = orderInList;
                    plc.TrackIdInPlaylist = this.trackDao.GetNextSmallestTrackIdInPlaylist();
                    track.TrackIdInPlaylist = plc.TrackIdInPlaylist;
                    this.trackDao.CreatePlaylistContent(plc);
                    orderInList++;

                    if (insertIntoDefault && fromDragAndDrop)
                    {
                        if (trackToReplaceInMediaPlayer != null &&
                            trackToReplaceInMediaPlayer.Id == track.Id &&
                            trackToReplaceInMediaPlayer.Path == track.Path)
                        {
                            this.mediaPLayerComponent.CurrentTrackIdInPlaylist = plc.TrackIdInPlaylist;
                        }
                    }
                }
                if (insertIntoDefault)
                {
                    this.LoadTrackList(trackList, dragIndex);
                    if (this.mediaPLayerComponent.MediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        this.mediaPLayerComponent.SetCurrentTrackIndex(0);
                        this.PlayTrack();
                    }
                }

                if (insertIntoDefault && fromDragAndDrop && fromExternalDragAndDrop)
                {
                    List<int> oldListTrackIdInPlaylistList = new List<int>();
                    for (int i = 0; i <= trackList.Count - 1; i++)
                    {
                        oldListTrackIdInPlaylistList.Add(trackList[i].TrackIdInPlaylist);
                    }
                    this.SetTrackList(trackListTable);
                    this.SaveTrackList(trackListTable, this.currentPlaylistId);
                    ((PlaylistView)this.view).SetSelectionAfterExternalDragAndDrop(oldListTrackIdInPlaylistList);
                }

            }*/
        }
       /* public void CallAddTracksToTrackListFromExternalDragAndDropEvent(List<Track> trackList, int dragIndex)
        {
            Track trackToReplaceInMediaPlayer = null;
            if (insertIntoDefault && fromDragAndDrop)
            {
                PlaylistContent currentPlayedTrackPlc = this.trackDao.GetPlaylistContentByTrackIdInPlaylist(this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
                if (currentPlayedTrackPlc != null)
                {
                    trackToReplaceInMediaPlayer = this.trackDao.GetTrack(currentPlayedTrackPlc.TrackId, this.tagList);
                }
            }

            DataRow playlistRow = null;
            int quickPlaylistId = -1;
            if (!insertIntoDefault && dragIndex > -1)
            {
                playlistRow = this.playlistListTable.Rows[dragIndex];
                if (playlistRow != null)
                {
                    quickPlaylistId = Convert.ToInt32(playlistRow["Id"]);
                }
            }

            if (trackList != null && trackList.Count > 0)
            {
                int orderInList = this.trackListTable.Rows.Count;
                foreach (Track track in trackList)
                {
                    PlaylistContent plc = new PlaylistContent();
                    plc.Id = this.trackDao.GetNextId(TableName.PlaylistContent.ToString());

                    if (!insertIntoDefault && dragIndex > -1)
                    {
                        plc.PlaylistId = quickPlaylistId;
                    }
                    else
                    {
                        plc.PlaylistId = this.currentPlaylistId;
                    }

                    plc.TrackId = track.Id;
                    plc.OrderInList = orderInList;
                    plc.TrackIdInPlaylist = this.trackDao.GetNextSmallestTrackIdInPlaylist();
                    track.TrackIdInPlaylist = plc.TrackIdInPlaylist;
                    this.trackDao.CreatePlaylistContent(plc);
                    orderInList++;

                    if (insertIntoDefault && fromDragAndDrop)
                    {
                        if (trackToReplaceInMediaPlayer != null &&
                            trackToReplaceInMediaPlayer.Id == track.Id &&
                            trackToReplaceInMediaPlayer.Path == track.Path)
                        {
                            this.mediaPLayerComponent.CurrentTrackIdInPlaylist = plc.TrackIdInPlaylist;
                        }
                    }
                }
                if (insertIntoDefault)
                {
                    this.LoadTrackList(trackList, dragIndex);
                    if (this.mediaPLayerComponent.MediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        this.mediaPLayerComponent.SetCurrentTrackIndex(0);
                        this.PlayTrack();
                    }
                }

                if (insertIntoDefault && fromDragAndDrop && fromExternalDragAndDrop)
                {
                    List<int> oldListTrackIdInPlaylistList = new List<int>();
                    for (int i = 0; i <= trackList.Count - 1; i++)
                    {
                        oldListTrackIdInPlaylistList.Add(trackList[i].TrackIdInPlaylist);
                    }
                    this.SetTrackList(trackListTable);
                    this.SaveTrackList(trackListTable, this.currentPlaylistId);
                    ((PlaylistView)this.view).SetSelectionAfterExternalDragAndDrop(oldListTrackIdInPlaylistList);
                }

            }
        }*/
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
                        int i = 11;

                        if (track.TrackTagValues != null)
                        {
                            foreach (TrackTagValue ttv in track.TrackTagValues)
                            {
                                args[i] = ttv.TagValueName;
                            }
                        }
                        
                        this.trackListTable.Rows.Add(args);
                    }
                }

                this.SetTrackList(trackListTable);
                this.SaveTrackList(trackListTable, this.currentPlaylistId);
                ((PlaylistView)this.view).UpdateTrackCountAndLength(this.currentPlaylistId);
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
                this.SaveTrackList(trackListTable, this.currentPlaylistId);
                this.SetTrackList(trackListTable);
                ((PlaylistView)this.view).CallSetCurrentTrackColorEvent();
                ((PlaylistView)this.view).UpdateTrackCountAndLength(this.currentPlaylistId);
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
                this.SetTrackList(this.trackListTable);
                ((PlaylistView)this.view).CallSetCurrentTrackColorEvent();
                ((PlaylistView)this.view).UpdateTrackCountAndLength(this.currentPlaylistId);
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
                this.SetTrackList(this.trackListTable);
                ((PlaylistView)this.view).CallSetCurrentTrackColorEvent();
                ((PlaylistView)this.view).UpdateTrackCountAndLength(this.currentPlaylistId);
            }
        }
        public void Clear()
        {
            this.trackListTable.Rows.Clear();

            this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
            this.SetTrackList(this.trackListTable);
            ((PlaylistView)this.view).UpdateTrackCountAndLength(this.currentPlaylistId);
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
                    this.view.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
            }
            else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
            {
                this.view.UpdateAfterPlayTrackAfterPause();
            }
        }
        public void PauseTrackEvent(object sender, EventArgs e)
        {
            this.mediaPLayerComponent.PauseTrack();
            this.view.UpdateAfterPauseTrack();
        }
        public void StopTrackEvent(object sender, EventArgs e)
        {
            this.mediaPLayerComponent.StopTrack();
            this.view.UpdateAfterStopTrack();
        }
        public void PrevTrackEvent(object sender, ListEventArgs e)
        {
            if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
            {
                MediaPlayerUpdateState updateState = this.mediaPLayerComponent.PrevTrack();

                if (updateState == MediaPlayerUpdateState.AfterPlay)
                {
                    this.view.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
                }
                else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                {
                    this.view.UpdateAfterPlayTrackAfterPause();
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
                    this.view.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
                }
                else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                {
                    this.view.UpdateAfterPlayTrackAfterPause();
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
                    this.view.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
                }
                else
                {
                    this.view.UpdateAfterPlayTrackAfterPause();
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
                    this.view.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
                }
                else if (updateState == MediaPlayerUpdateState.AfterPlayAfterPause)
                {
                    this.view.UpdateAfterPlayTrackAfterPause();
                }
            }

            this.view.UpdateMediaPlayerProgressStatus(duration, durationString, currentPosition, currentPositionString);
        }
        private void SetCurrentTrackColorEvent(object sender, ListEventArgs e)
        {
            this.view.SetCurrentTrackColor(this.mediaPLayerComponent.CurrentTrackIdInPlaylist);
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
        private void ShowPlaylistEditorViewEvent(object sender, ListEventArgs e)
        {
            if (e.IntegerField1 == -1)
            {
                //CREATE PLAYLIST
                PlaylistEditorView editorView = new PlaylistEditorView();
                PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(editorView, this.trackDao, this.settingDao);

                if (editorView.ShowDialog((PlaylistView)this.view) == DialogResult.OK)
                {
                    Playlist newPlaylist = presenter.newPlaylist;
                    if (newPlaylist != null)
                    {
                        this.trackDao.CreatePlaylist(newPlaylist);
                        this.playlistListTable.Rows.Add(newPlaylist.Id, newPlaylist.QuickListGroup, newPlaylist.Name, newPlaylist.OrderInList, newPlaylist.ProfileId, newPlaylist.IsActive);
                    }
                    this.SavePlaylistList(playlistListTable);
                    this.SetPlaylistList(playlistListTable);
                }
            }
            else
            {
                //EDIT PLAYLIST
                Playlist playlist = new Playlist();
                playlist.Id = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["Id"]);
                playlist.Name = this.playlistListTable.Rows[e.IntegerField1]["Name"].ToString();
                playlist.OrderInList = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["OrderInList"]);
                playlist.QuickListGroup = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["G"]);
                playlist.IsActive = Convert.ToBoolean(this.playlistListTable.Rows[e.IntegerField1]["IsActive"]);
                playlist.ProfileId = Convert.ToInt32(this.playlistListTable.Rows[e.IntegerField1]["ProfileId"]);

                if (playlist.Name.Equals("Default Playlist"))
                {
                    MessageBox.Show("Default playlist cannot be renamed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    PlaylistEditorView editorView = new PlaylistEditorView();
                    PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(editorView, this.trackDao, playlist);

                    if (editorView.ShowDialog((PlaylistView)this.view) == DialogResult.OK)
                    {
                        Playlist newPlaylist = presenter.newPlaylist;
                        if (newPlaylist != null)
                        {
                            this.trackDao.UpdatePlaylist(newPlaylist);
                            this.playlistListTable.Rows[e.IntegerField1]["Name"] = newPlaylist.Name;
                        }
                        this.SavePlaylistList(this.playlistListTable);
                        this.SetPlaylistList(this.playlistListTable);
                    }
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
                            this.view.SetCurrentTrackColor(currentTrackIdInPlaylist);
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
                        this.SetPlaylistList(this.playlistListTable);

                        if (playlistId == this.currentPlaylistId)
                        {
                            this.trackListTable.Rows.Clear();
                            this.currentPlaylistId = Convert.ToInt32(this.playlistListTable.Rows[0]["Id"]);

                            this.LoadPlaylist(this.currentPlaylistId);

                            this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
                            this.SetTrackList(this.trackListTable);
                            this.SetPlaylistAsCurrent(this.currentPlaylistId);

                            ((PlaylistView)this.view).UpdateTrackCountAndLength(this.currentPlaylistId);

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
            for(int i = 0; i <= this.playlistListTable.Rows.Count - 1; i++)
            {
                if (Convert.ToInt32(this.playlistListTable.Rows[i]["G"]) == e.IntegerField2)
                {
                    this.playlistListTable.Rows[i]["G"] = 0;
                }
            }

            if(Convert.ToInt32(playlistListTable.Rows[e.IntegerField1]["G"]) == e.IntegerField2)
            {
                playlistListTable.Rows[e.IntegerField1]["G"] = 0;
            }
            else
            {
                playlistListTable.Rows[e.IntegerField1]["G"] = e.IntegerField2.ToString();
            }
           
            this.SavePlaylistList(this.playlistListTable);
            this.SetPlaylistList(this.playlistListTable);
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
        #endregion

        private void ShowColumnVisibilityEditorEvent(object sender, EventArgs e)
        {
            ColumnVisibilityEditorView columnVisibilityEditorView = new ColumnVisibilityEditorView();
            ColumnVisibilityEditorPresenter presenter = new ColumnVisibilityEditorPresenter(columnVisibilityEditorView,this.trackDao, this.settingDao);
            if (columnVisibilityEditorView.ShowDialog((PlaylistView)this.view) == DialogResult.OK)
            {
                this.TrackColumnVisibilityArray = presenter.TrackPropertyList.Select(x => x.IsEnabled).ToArray();
                foreach(TrackProperty tp in presenter.TrackPropertyList)
                {
                    this.settingDao.UpdateTrackProperty(tp, true);
                }
                this.InitializeDataTables();
                ((PlaylistView)this.view).CallSetCurrentTrackColorEvent();
            }
        }
        private void DisplayTagEditorEvent(object sender, EventArgs e)
        {
            this.isTagEditorDisplayed = !this.isTagEditorDisplayed;
            ((PlaylistView)this.view).CallDisplayTagEditor(this.isTagEditorDisplayed);
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
                        ((PlaylistView)this.view).InitializeTagValueEditor(tagValueList);
                    }
                }
            }
           
        }
        private void SetTagValueEvent(object sender, ListEventArgs e)
        {
            TagValue tv = this.tagValueList[e.IntegerField1];
            if(tv != null)
            {
                if (this.trackListTable != null && this.trackListTable.Rows != null && this.trackListTable.Rows.Count > 0)
                {
                    for (int i = e.Rows.Count - 1; i >= 0; i--)
                    {
                        if (e.Rows[i].Selected)
                        {
                            Track track = this.trackDao.GetTrack(Convert.ToInt32(this.trackListTable.Rows[i]["Id"]), this.tagList);
                            if(track != null)
                            {
                                foreach(TrackTagValue ttv in track.TrackTagValues)
                                {
                                    if(ttv.TagId == this.currentTag.Id)
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
                      this.SetTrackList(trackListTable);

                }
            }

            
        }

    }
}
