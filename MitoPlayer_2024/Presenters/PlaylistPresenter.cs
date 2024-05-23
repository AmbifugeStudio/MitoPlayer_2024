using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Type = System.Type;

namespace MitoPlayer_2024.Presenters
{
    public enum TableName {
        Playlist,
        Track,
        PlaylistContent,
        Profile
    }
    public class PlaylistPresenter
    {
        private IPlaylistView playlistView { get; set; }
        private IPlaylistDao playlistDao { get; set; }
        private ITrackDao trackDao { get; set; }
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
        Dictionary<string, int> columnOrderStates { get; set; }
        private MediaPlayerComponent mediaPLayerComponent { get; set; }
        public PlaylistPresenter(IPlaylistView view, MediaPlayerComponent mediaPlayer, IPlaylistDao playlistDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            //INITIALIZE
            this.playlistView = view;
            this.playlistDao = playlistDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;

            this.InitializeDataTables();
            this.InitializePlaylistListAndTrackList();

            this.mediaPLayerComponent = mediaPlayer;
            this.mediaPLayerComponent.Initialize(this.trackListTable); 
            this.InitializeVolume();

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
            this.playlistView.TrackDragAndDropEvent += TrackDragAndDropEvent;
            this.playlistView.CopyTracksToPlaylistEvent += CopyTracksToPlaylistEvent;

            //PLAYLIST
            this.playlistView.ShowPlaylistEditorViewEvent += ShowPlaylistEditorViewEvent;
            this.playlistView.LoadPlaylistEvent += LoadPlaylistEvent;
            this.playlistView.DeletePlaylistEvent += DeletePlaylistEvent;
            this.playlistView.SetQuickListEvent += SetQuickListEvent;

            this.playlistView.Show();
        }

        #region INITIALIZE
        private void InitializeVolume()
        {
            int volume = this.settingDao.GetIntegerSetting(Settings.Volume.ToString());
            if (volume == -1)
                volume = 50;
            this.playlistView.SetVolume(volume);
            this.mediaPLayerComponent.MediaPlayer.settings.volume = volume;
        }
        private void InitializeDataTables()
        {
            //PLAYLIST GRID
            String colNames = this.settingDao.GetStringSetting(Settings.PlaylistColumnNames.ToString(), true);
            String colTypes = this.settingDao.GetStringSetting(Settings.PlaylistColumnTypes.ToString(), true);
            String colVisibility = this.settingDao.GetStringSetting(Settings.PlaylistColumnVisibility.ToString(), true);
            String[] playlistColumnNames = Array.ConvertAll(colNames.Split(','), s => s);
            String[] playlistColumnTypes = Array.ConvertAll(colTypes.Split(','), s => s);
            this.PlaylistColumnVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));

            this.playlistListBindingSource = new BindingSource();
            this.playlistListTable = new DataTable();
            for (int i = 0; i <= playlistColumnTypes.Length - 1; i++)
                this.playlistListTable.Columns.Add(playlistColumnNames[i], Type.GetType(playlistColumnTypes[i]));

            this.SetPlaylistList(this.playlistListTable);

            //TRACKLIST GRID
            colNames = this.settingDao.GetStringSetting(Settings.TrackColumnNames.ToString(), true);
            colTypes = this.settingDao.GetStringSetting(Settings.TrackColumnTypes.ToString(), true);
            colVisibility = this.settingDao.GetStringSetting(Settings.TrackColumnVisibility.ToString(), true);
            String[] trackColumnNames = Array.ConvertAll(colNames.Split(','), s => s);
            String[] trackColumnTypes = Array.ConvertAll(colTypes.Split(','), s => s);
            this.TrackColumnVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));

            this.trackListBindingSource = new BindingSource();
            this.trackListTable = new DataTable();
            this.columnOrderStates = new Dictionary<string, int>();
            for (int i = 0; i <= trackColumnNames.Count() - 1; i++)
            {
                this.columnOrderStates.Add(trackColumnNames[i], -1);
                this.trackListTable.Columns.Add(trackColumnNames[i], Type.GetType((trackColumnTypes[i])));
            }

            this.SetTrackList(this.trackListTable);

            this.selectedTrackListBindingSource = new BindingSource();
            this.selectedTrackListTable = new DataTable();
            for (int i = 0; i <= trackColumnNames.Count() - 1; i++)
                this.selectedTrackListTable.Columns.Add(trackColumnNames[i], Type.GetType((trackColumnTypes[i])));

            this.SetSelectedTrackList(this.selectedTrackListTable);
        }
        /*
         * betölti a rendszerbe már felvett plalyist-eket
         * ha még nincs playlist egyáltalán, akkor megcsinálta a default playlist-et
         * betölti a current playlist-et (ebből csak 1 lehet)
         */
        private void InitializePlaylistListAndTrackList()
        {
            //csak clear mert az oszlopok kellenek
            this.playlistListTable.Clear();

            String defaultPlaylistName = this.settingDao.GetStringSetting(Settings.DefaultPlaylistName.ToString(), true);

            List<Playlist> plsList = this.playlistDao.GetDefaultPlaylist(defaultPlaylistName);

            if (plsList == null || plsList.Count() <= 0)
            {
                Playlist playlist = new Playlist();
                playlist.Id = this.GetNewPlaylistId();
                playlist.Name = defaultPlaylistName;
                playlist.OrderInList = 0;

                plsList.Add(playlist);

                this.playlistDao.CreatePlaylist(playlist);
                this.currentPlaylistId = playlist.Id;

                this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId,true);

                this.LoadPlaylist(playlist);
            }
            else
            {
                if (plsList != null && plsList.Count() > 0)
                {
                    List<Playlist> list = this.playlistDao.GetAllPlaylist();
                    list.RemoveAll(x => x.Name.Equals(defaultPlaylistName));
                    plsList.AddRange(list);
                } 
            }

            //TODO CSEKKOLNI? HA TÖRÖPJÜK
            this.currentPlaylistId = this.settingDao.GetIntegerSetting(Settings.CurrentPlaylistId.ToString(), true);

            if (this.currentPlaylistId == -1)
                this.currentPlaylistId = 0;

            this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId, true);

            foreach (Playlist playlist in plsList)
                this.playlistListTable.Rows.Add(playlist.Id,playlist.QuickListGroup, playlist.Name, playlist.OrderInList);

            Playlist pls = plsList.Find(x => x.Id == this.currentPlaylistId);
            if (pls != null)
                this.LoadPlaylist(pls);

            this.SetPlaylistList(playlistListTable);
        }
        private int GetNewPlaylistId()
        {
            int id = this.playlistDao.GetLastObjectId(TableName.Playlist.ToString());
            if (id == -1)
                return 0;
            else
                return id + 1;
        }
        private void LoadPlaylist(Playlist pls)
        {
            List<Track> trackList = this.playlistDao.LoadPlaylist(pls.Id);
            if (trackList != null && trackList.Count > 0)
            {
                foreach (Track track in trackList)
                {
                    String length = this.LengthToString(track.Length);
                    this.trackListTable.Rows.Add(track.Id, track.Album, track.Artist, track.Title, track.Year.ToString(), length, track.IsMissing, track.Path, track.FileName, track.OrderInList, track.TrackIdInPlaylist);
                }
                this.SetTrackList(this.trackListTable);
            }
            ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
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
            this.playlistView.SetPlaylistListBindingSource(this.playlistListBindingSource, this.PlaylistColumnVisibilityArray, this.currentPlaylistId);
        }
        private void SetTrackList(DataTable trackListTable)
        {
            this.trackListBindingSource.DataSource = trackListTable;
            this.playlistView.SetTrackListBindingSource(this.trackListBindingSource, this.TrackColumnVisibilityArray);
        }
        private void SetSelectedTrackList(DataTable trackListTable)
        {
            this.selectedTrackListBindingSource.DataSource = trackListTable;
            this.playlistView.SetSelectedTrackListBindingSource(this.selectedTrackListBindingSource);
        }
        private void SavePlaylistList(DataTable playlistListTable)
        {
            this.playlistDao.DeleteAllPlaylist();
            List<Playlist> playlistlist = this.ConvertPlaylistDataTableToList(playlistListTable);
            int orderInList = 0;
            foreach (Playlist playlist in playlistlist)
            {
                playlist.OrderInList = orderInList;
                this.playlistDao.CreatePlaylist(playlist);
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
                playlistList.Add(playlist);
            }

            return playlistList;
        }
        private void SaveTrackList(DataTable trackListTable, int playlistId)
        {
            this.playlistDao.DeletePlaylistContent(playlistId);
            List<Track> tracklist = this.ConvertTrackDataTableToList(trackListTable);
            int orderInList = 0;
            foreach (Track track in tracklist)
            {
                track.OrderInList = orderInList;
                this.trackDao.AddTrackToPlaylist(GetNewPlaylistContentId(), playlistId, track.Id, track.OrderInList, track.TrackIdInPlaylist);
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
        private int GetNewPlaylistContentId()
        {
            int id = this.playlistDao.GetLastObjectId(TableName.PlaylistContent.ToString());
            if (id == -1)
                return 0;
            else
                return id + 1;
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
                this.SetTrackList(trackListTable);
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
                this.SetTrackList(this.trackListTable);
                ((PlaylistView)this.playlistView).CallSetCurrentTrackColorEvent();
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
        private void CopyTracksToPlaylistEvent(object sender, ListEventArgs e)
        {
            int quickPlaylistId = -1;
            List<Track> sourceTrackList = null;
            List<Track> targetTrackList = null;
            int playlistIndex = 0;
            DataRow playlistRow = null;
            String playlistName = "";

            if (e.SelectedRows != null && e.SelectedRows.Count > 0)
            {
                sourceTrackList = this.ConvertSelectedRowsToList(e.SelectedRows);
                playlistRow = this.playlistListTable.Select("G = " + e.IntegerField1).First();

                if (playlistRow != null)
                {
                    quickPlaylistId = Convert.ToInt32(playlistRow["Id"]);
                    playlistName = playlistRow["Name"].ToString();
                    targetTrackList = this.playlistDao.LoadPlaylist(quickPlaylistId);
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
                ((PlaylistView)this.playlistView).UpdateAfterCopyTracksToPlaylist(sourceTrackList.Count, playlistName);
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
        internal void CopyTracksToPlaylist(int playlistId, List<Track> sourceTrackList, List<Track> targetTrackList)
        {
            if (sourceTrackList != null && sourceTrackList.Count > 0)
            {
                int orderInList = targetTrackList.Count;
                foreach (Track track in sourceTrackList)
                {
                    track.OrderInList = orderInList;
                    track.TrackIdInPlaylist = this.GetNewTrackIdInPlaylist();
                    this.trackDao.AddTrackToPlaylist(this.GetNewPlaylistContentId(), playlistId, track.Id, track.OrderInList, track.TrackIdInPlaylist);
                    orderInList++;
                }
            }
        }
        private int GetNewTrackIdInPlaylist()
        {
            return this.playlistDao.GetNextLastSmallestTrackIdInPlaylist();
        }
        #endregion

        #region TRACKLIST - DRAG FILES FROM OUTSIDE
        internal void CallAddTrackToTrackListEvent(List<Track> trackList, int dragIndex)
        {
            if (trackList != null && trackList.Count > 0)
            {
                int orderInList = this.trackListTable.Rows.Count;
                foreach (Track track in trackList)
                {
                    track.OrderInList = orderInList;
                    track.TrackIdInPlaylist = this.GetNewTrackIdInPlaylist();
                    this.trackDao.AddTrackToPlaylist(this.GetNewPlaylistContentId(), this.currentPlaylistId, track.Id, track.OrderInList, track.TrackIdInPlaylist);
                    orderInList++;
                }
                this.LoadTrackList(trackList, dragIndex);
                if (this.mediaPLayerComponent.MediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                {
                    this.mediaPLayerComponent.SetCurrentTrackIndex(0);
                    this.PlayTrack();
                }
            }
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
                        trackListTable.Rows.InsertAt(userRow, dragIndex);
                    }
                    else
                    {
                        trackListTable.Rows.Add(track.Id, track.Album, track.Artist, track.Title, track.Year.ToString(), length, track.IsMissing, track.Path, track.FileName, track.OrderInList, track.TrackIdInPlaylist);
                    }
                }

                this.SetTrackList(trackListTable);
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
                this.SaveTrackList(trackListTable, this.currentPlaylistId);
                this.SetTrackList(trackListTable);
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
                this.SetTrackList(this.trackListTable);
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
                this.SetTrackList(this.trackListTable);
                ((PlaylistView)this.playlistView).CallSetCurrentTrackColorEvent();
                ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);
            }
        }
        public void Clear()
        {
            this.trackListTable.Rows.Clear();

            this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
            this.SetTrackList(this.trackListTable);
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
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
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
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
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
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
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
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
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
                    this.playlistView.UpdateAfterPlayTrack(this.mediaPLayerComponent.GetCurrentTrackIndex());
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
        private void ShowPlaylistEditorViewEvent(object sender, ListEventArgs e)
        {
            if (e.IntegerField1 == -1)
            {
                //CREATE PLAYLIST
                PlaylistEditorView editorView = new PlaylistEditorView();
                PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(editorView, this.playlistDao, this.settingDao);

                if (editorView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
                {
                    Playlist newPlaylist = presenter.newPlaylist;
                    if (newPlaylist != null)
                    {
                        this.playlistDao.CreatePlaylist(newPlaylist);
                        this.playlistListTable.Rows.Add(newPlaylist.Id, newPlaylist.QuickListGroup, newPlaylist.Name, newPlaylist.OrderInList);
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

                String defaultPlaylistName = this.settingDao.GetStringSetting(Settings.DefaultPlaylistName.ToString(), true);

                if (playlist.Name.Equals(defaultPlaylistName))
                {
                    MessageBox.Show("Default playlist cannot be renamed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    PlaylistEditorView editorView = new PlaylistEditorView();
                    PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(editorView, this.playlistDao, playlist);

                    if (editorView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
                    {
                        Playlist newPlaylist = presenter.newPlaylist;
                        if (newPlaylist != null)
                        {
                            this.playlistDao.UpdatePlaylist(newPlaylist);
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
                Playlist playlist = new Playlist();
                playlist.Id = Convert.ToInt32(this.playlistListTable.Rows[playlistIndex]["Id"]);
                playlist.Name = this.playlistListTable.Rows[playlistIndex]["Name"].ToString();
                playlist.OrderInList = Convert.ToInt32(this.playlistListTable.Rows[playlistIndex]["OrderInList"]);

                this.currentPlaylistId = playlist.Id;

                this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId, true);

                this.trackListTable.Rows.Clear();

                //this.mediaPLayerComponent.Initialize(this.trackListTable);
                this.mediaPLayerComponent.Initialize2(this.trackListTable);

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

                String defaultPlaylistName = this.settingDao.GetStringSetting(Settings.DefaultPlaylistName.ToString(), true);

                if (playlistName.Equals(defaultPlaylistName))
                {
                    MessageBox.Show("Default playlist cannot be deleted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DialogResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        this.playlistListTable.Rows.RemoveAt(e.IntegerField1);
                        this.playlistDao.DeletePlaylist(playlistId);

                        this.SavePlaylistList(this.playlistListTable);
                        this.SetPlaylistList(this.playlistListTable);

                        if (playlistId == this.currentPlaylistId)
                        {
                            this.trackListTable.Rows.Clear();

                            this.SaveTrackList(this.trackListTable, this.currentPlaylistId);
                            this.SetTrackList(this.trackListTable);

                            this.currentPlaylistId = Convert.ToInt32(this.playlistListTable.Rows[0]["Id"]);
                            this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId, true);

                            ((PlaylistView)this.playlistView).UpdateTrackCountAndLength(this.currentPlaylistId);

                        }
                    }
                }

            }
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
        #endregion
    }
}
