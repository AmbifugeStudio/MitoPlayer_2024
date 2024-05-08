using Google.Protobuf.WellKnownTypes;
using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using MySqlX.XDevAPI.Relational;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib.Jpeg;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Type = System.Type;

namespace MitoPlayer_2024.Presenters
{
    public enum TableName {
        Playlist,
        Track,
        PlaylistContent
    }
    public class PlaylistPresenter
    {
        private IPlaylistView playlistView;
        private IPlaylistDao playlistDao;
        private ITrackDao trackDao;
        private ISettingDao settingDao;

        private DataTable playlistListTable;
        private DataTable trackListTable;
        private DataTable selectedTrackListTable;
        private BindingSource playlistListBindingSource;
        private BindingSource trackListBindingSource;
        private BindingSource selectedTrackListBindingSource;

        public bool[] PlaylistColumnVisibilityArray;
        public bool[] TrackColumnVisibilityArray;
        private int[] trackColumnOrderingFlagArray;

        private string[] scannedFiles;
        private int currentPlaylistId;

        public PlaylistPresenter(IPlaylistView view, IPlaylistDao playlistDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.playlistView = view;
            this.playlistDao = playlistDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;

            //VOLUME
            int volume = this.settingDao.GetIntegerSettingByName(Settings.Volume.ToString(), true);
            if(volume == -1)
                volume = 50;
            this.playlistView.SetVolume(volume);

            //PLAYLIST GRID
            String colNames = this.settingDao.GetStringSettingByName(Settings.PlaylistColumnNames.ToString(), true);
            String colTypes = this.settingDao.GetStringSettingByName(Settings.PlaylistColumnTypes.ToString(), true);
            String colVisibility = this.settingDao.GetStringSettingByName(Settings.PlaylistColumnVisibility.ToString(), true);
            String[] playlistColumnNames = Array.ConvertAll(colNames.Split(','), s => s);
            String[] playlistColumnTypes = Array.ConvertAll(colTypes.Split(','), s => s);
            this.PlaylistColumnVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));

            this.playlistListBindingSource = new BindingSource();
            playlistListTable = new DataTable();
            for (int i = 0; i <= playlistColumnTypes.Length - 1; i++)
            {
                playlistListTable.Columns.Add(playlistColumnNames[i], Type.GetType(playlistColumnTypes[i]));
            }

            this.SetPlaylistList(playlistListTable);

            //TRACKLIST GRID
            colNames = this.settingDao.GetStringSettingByName(Settings.TrackColumnNames.ToString(), true);
            colTypes = this.settingDao.GetStringSettingByName(Settings.TrackColumnTypes.ToString(), true);
            colVisibility = this.settingDao.GetStringSettingByName(Settings.TrackColumnVisibility.ToString(), true);
            String[] trackColumnNames = Array.ConvertAll(colNames.Split(','), s => s);
            String[] trackColumnTypes = Array.ConvertAll(colTypes.Split(','), s => s);
            this.TrackColumnVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));

            this.trackListBindingSource = new BindingSource();
            trackListTable = new DataTable();
            for(int i = 0; i <= trackColumnNames.Count() - 1; i++)
            {
                trackListTable.Columns.Add(trackColumnNames[i], Type.GetType((trackColumnTypes[i])));
            }
            this.SetTrackList(trackListTable);

            this.selectedTrackListBindingSource = new BindingSource();
            selectedTrackListTable = new DataTable();
            for (int i = 0; i <= trackColumnNames.Count() - 1; i++)
            {
                selectedTrackListTable.Columns.Add(trackColumnNames[i], Type.GetType((trackColumnTypes[i])));
            }
            this.SetSelectedTrackList(selectedTrackListTable);

            String trackColumnOrderingFlags =  this.settingDao.GetStringSettingByName(Settings.TrackColumnOrderingFlags.ToString(), true);
            this.trackColumnOrderingFlagArray = Array.ConvertAll(trackColumnOrderingFlags.Split(','), s => int.Parse(s));

            //EVENTS
            this.playlistView.OpenFiles += OpenFiles;
            this.playlistView.OpenDirectory += OpenDirectory;
            this.playlistView.ScanFiles += ScanFiles;
            this.playlistView.OrderTableByColumn += OrderTableByColumn;
            this.playlistView.OrderByArtist += OrderByArtist;
            this.playlistView.OrderByTitle += OrderByTitle;
            this.playlistView.OrderByFileName += OrderByFileName;
            this.playlistView.Reverse += Reverse;
            this.playlistView.Shuffle += Shuffle;
            this.playlistView.RemoveMissingTracks += RemoveMissingTracks;
            this.playlistView.RemoveDuplicatedTracks += RemoveDuplicatedTracks;
            this.playlistView.Clear += Clear;
            this.playlistView.DeleteTracks += DeleteTracks;
            
            this.playlistView.TrackDragAndDrop += TrackDragAndDrop;

            this.playlistView.LoadPlaylist += LoadPlaylist;
            this.playlistView.ShowPlaylistEditorView += ShowPlaylistEditorView;
            this.playlistView.DeletePlaylist += DeletePlaylist;

            this.playlistView.ChangeVolume += ChangeVolume;

            this.LoadAllPlaylist();
            this.playlistView.Show();
        }

        private void SavePlaylistList(DataTable playlistListTable)
        {
            this.playlistDao.DeleteAllPlaylist();
            List<Playlist> playlistlist = ConvertPlaylistDataTableToList(playlistListTable);
            int sortingId = 0;
            foreach (Playlist playlist in playlistlist)
            {
                playlist.OrderInList = sortingId;
                this.playlistDao.CreatePlaylist(playlist);
                sortingId++;
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
                playlistList.Add(playlist);
            }

            return playlistList;
        }
        private void SetPlaylistList(DataTable playlistListTable)
        {
            this.playlistListBindingSource.DataSource = playlistListTable;
            this.playlistView.SetPlaylistListBindingSource(this.playlistListBindingSource, this.PlaylistColumnVisibilityArray, this.currentPlaylistId);
        }
        private void SaveTrackList(DataTable trackListTable)
        {
            this.playlistDao.DeletePlaylistContent(this.currentPlaylistId);
            List<Track> tracklist = this.ConvertTrackDataTableToList(trackListTable);
            int sortingId = 0;
            foreach (Track track in tracklist)
            {
                track.OrderInList = sortingId;
                this.trackDao.AddTrackToPlaylist(GetNewPlaylistContentId(), this.currentPlaylistId, track.Id, track.OrderInList);
                sortingId++;
            }
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
                trackList.Add(track);
            }

            return trackList;
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
        private void ChangeVolume(object sender, ListEventArgs e)
        {
            this.settingDao.SetIntegerSetting(Settings.Volume.ToString(), e.IntegerField1);
        }

        //START - LOAD ALL PLAYLIST
        /*
         * betölti a rendszerbe már felvett plalyist-eket
         * ha még nincs playlist egyáltalán, akkor megcsinálta a default playlist-et
         * betölti a current playlist-et (ebből csak 1 lehet)
         */
        private void LoadAllPlaylist()
        {
            playlistListTable.Clear();

            List<Playlist> plsList = playlistDao.GetAllPlaylist();

            if(plsList == null || plsList.Count() <= 0)
            {
                Playlist playlist = new Playlist();
                playlist.Id = this.GetNewPlaylistId();
                playlist.Name = this.settingDao.GetStringSettingByName(Settings.DefaultPlaylistName.ToString(), true);
                playlist.OrderInList = 0;
                plsList.Add(playlist);

                playlistDao.CreatePlaylist(playlist);

                this.currentPlaylistId = playlist.Id;
                this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId);

                this.LoadPlaylist(playlist);
            }
            else
            {
                this.currentPlaylistId = this.settingDao.GetIntegerSettingByName(Settings.CurrentPlaylistId.ToString(), true);
                if(this.currentPlaylistId == -1)
                {
                    this.currentPlaylistId = 0;
                }

                Playlist pls = plsList.Find(x => x.Id == this.currentPlaylistId);
                if(pls != null)
                {
                    this.LoadPlaylist(pls);
                }
                
            }

            foreach (Playlist playlist in plsList)
            {
                playlistListTable.Rows.Add(playlist.Id, playlist.Name, playlist.OrderInList);
            }

            this.SetPlaylistList(playlistListTable);
        }
        //START - LOAD TRACKLIST
        /*
         * betölti a megadott playlist track-jeit
         */
        private void LoadPlaylist(Playlist pls)
        {
            List<Track> trackList = playlistDao.LoadPlaylist(pls.Id);
            if(trackList != null && trackList.Count > 0)
            {
                foreach (Track track in trackList)
                {
                    String length = this.LengthToString(track.Length);
                    trackListTable.Rows.Add(track.Id, track.Album, track.Artist, track.Title, track.Year.ToString(), length, track.IsMissing, track.Path, track.FileName, track.OrderInList);
                }
                this.SetTrackList(trackListTable);
            }
        }
        private void LoadTrackList(List<Track> trackList)
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
                        trackListTable.Rows.InsertAt(userRow,dragIndex); 
                    }
                    else
                    {
                        trackListTable.Rows.Add(track.Id, track.Album, track.Artist, track.Title, track.Year.ToString(), length, track.IsMissing, track.Path, track.FileName, track.OrderInList);
                    }
                }
                int sortingId = 0;
                for(int i = 0;i <= trackListTable.Rows.Count -1;i++)
                {
                    trackListTable.Rows[i]["OrderInList"] = sortingId;
                    sortingId++;
                }
                dragIndex = -1;
                this.SetTrackList(trackListTable);
                this.SaveTrackList(trackListTable);
            }
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

        //OPEN FILES - mp3, wav, m3u
        /*
         * feldob a fájlkiválasztó ablakot, megjeleníti az utolsájra kiválasztott kiterjesztésű fájlokat
         * kiválasztást követően beolvassa a fájlokat
         */
        private void OpenFiles(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Audio files (*.mp3,*.wav,*.flac)|*.mp3;*.wav;*.flac|Playlist files (*.m3u)|*.m3u";

            int filterIndex = 0;
            filterIndex = this.settingDao.GetIntegerSettingByName(Settings.LastOpenFilesFilterIndex.ToString(), true);

            ofd.FilterIndex = filterIndex;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.ReadFiles(ofd.FileNames);
            }

            this.settingDao.SetIntegerSetting(Settings.LastOpenFilesFilterIndex.ToString(), ofd.FilterIndex);
        }
        //OPEN DIRECTORY
        private void OpenDirectory(object sender, EventArgs e)
        {
            scannedFiles = null;
            using (var fbd = new FolderBrowserDialog())
            {
                String path = this.settingDao.GetStringSettingByName(Settings.LastOpenDirectoryPath.ToString(), true);
                if (System.IO.File.Exists(path))
                {
                    fbd.SelectedPath = path;
                }

                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    this.ScanDirectory(fbd.SelectedPath);
                    this.ReadFiles(scannedFiles);
                }
                this.settingDao.SetStringSetting(Settings.LastOpenDirectoryPath.ToString(), fbd.SelectedPath);
            }
        }
        private void ScanDirectory(string path)
        {
            string[] files = Directory.GetFiles(path);
            files = files.Where(x => x.Contains(".mp3") || x.Contains(".m3u") || x.Contains(".wav")).ToArray();

            if(files != null && files.Length > 0)
            {
                if(scannedFiles == null)
                {
                    scannedFiles = files;
                }
                else
                {
                    scannedFiles = scannedFiles.Concat(files).ToArray();
                }
            }

            string[] directories = Directory.GetDirectories(path);
            foreach (String dir in directories)
            {
                this.ScanDirectory(dir);
            }
        }
        //OPEN FILES BY DRAG AND DROP
        private int dragIndex = -1;
        private void ScanFiles(object sender, ListEventArgs e)
        {
            string[] mediaFiles;
            string[] directories;
            dragIndex = e.IntegerField1;
            if (e.DragAndDropFiles != null && e.DragAndDropFiles.Length > 0)
            {
                mediaFiles = e.DragAndDropFiles.Where(x => x.EndsWith(".mp3") || x.EndsWith(".wav") || x.EndsWith(".flac") || x.EndsWith(".m3u")).ToArray();
                directories = e.DragAndDropFiles.Where(x => !x.EndsWith(".mp3") && !x.EndsWith(".wav") && !x.EndsWith(".flac") && !x.EndsWith(".m3u")).ToArray();

                if (mediaFiles != null && mediaFiles.Length > 0)
                {
                    this.ReadFiles(mediaFiles);
                }
                if (directories != null && directories.Length > 0)
                {
                    scannedFiles = null;
                    foreach (string dir in directories)
                    {
                        this.ScanDirectory(dir);
                        this.ReadFiles(scannedFiles);
                    }
                }

            }
        }
        //READ AND CREATE TRACKS
        private void ReadFiles(string[] fileNames)
        {
            List<String> filePathList = new List<String>();
            List<Track> trList = new List<Track>();
            if(fileNames != null && fileNames .Length > 0)
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

                int sortingId = trackListTable.Rows.Count;

                foreach (string path in filePathList)
                {
                    Track track = new Track();
                    track.Path = path;

                    string fileName = "";

                    if (path.EndsWith("flac"))
                    {
                        fileName = path.Substring(path.LastIndexOf(@"\") + 1);
                        fileName = fileName.Remove(fileName.LastIndexOf("."), 5);
                    }
                    else
                    {
                        fileName = path.Substring(path.LastIndexOf(@"\") + 1);
                        fileName = fileName.Remove(fileName.LastIndexOf("."), 4);
                    }

                    track.FileName = fileName;

                    if (!System.IO.File.Exists(path))
                    {
                        track.Artist = fileName;
                        track.IsMissing = true;
                    }
                    else
                    {
                        Track trackFromDb = this.trackDao.GetTrackByPath(track.Path);
                        if (trackFromDb != null)
                        {
                            track = trackFromDb;
                        }

                        if (path.Contains(".mp3"))
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
                            {
                                track.Artist = fileName;
                            }
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

                    if(track.Id == -1)
                    {
                        track.Id = this.GetNewTrackId();
                        this.trackDao.AddTrackToDatabase(track);
                    }

                    track.OrderInList = sortingId;
                    this.trackDao.AddTrackToPlaylist(this.GetNewPlaylistContentId(), this.currentPlaylistId, track.Id, track.OrderInList);
                    sortingId++;

                    trList.Add(track);

                }

                this.LoadTrackList(trList);
            }
        }
        private int GetNewPlaylistId()
        {
            int id = this.playlistDao.GetLastObjectId(TableName.Playlist.ToString());
            if(id == -1)
                return 0;
            else
                return id + 1;
        }
        private int GetNewTrackId()
        {
            int id = this.playlistDao.GetLastObjectId(TableName.Track.ToString());
            if (id == -1)
                return 0;
            else
                return id + 1;
        }
        private int GetNewPlaylistContentId()
        {
            int id = this.playlistDao.GetLastObjectId(TableName.PlaylistContent.ToString()) ;
            if (id == -1)
                return 0;
            else
                return id + 1;
        }

        //ORDER TRACKLIST
        private void OrderByArtist(object sender, EventArgs e)
        {
            DataView dv = trackListTable.DefaultView;
            dv.Sort = "Artist ASC";
            DataTable sortedDT = dv.ToTable();
            trackListTable = sortedDT;

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }
        private void OrderByTitle(object sender, EventArgs e)
        {
            DataView dv = trackListTable.DefaultView;
            dv.Sort = "Title ASC";
            DataTable sortedDT = dv.ToTable();
            trackListTable = sortedDT;

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }
        private void OrderByFileName(object sender, EventArgs e)
        {
            DataView dv = trackListTable.DefaultView;
            dv.Sort = "FileName ASC";
            DataTable sortedDT = dv.ToTable();
            trackListTable = sortedDT;

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }
        private void Reverse(object sender, EventArgs e)
        {
            DataTable reversedDt = trackListTable.Clone();
            for (var row = trackListTable.Rows.Count - 1; row >= 0; row--)
                reversedDt.ImportRow(trackListTable.Rows[row]);
            trackListTable = reversedDt;
            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }
        private void Shuffle(object sender, EventArgs e)
        {
            DataTable shuffledDt = trackListTable.Copy();

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
            trackListTable = sortedDT;

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }
        private void OrderTableByColumn(object sender, ListEventArgs e)
        {
            DataView dv = trackListTable.DefaultView;
            dv.Sort = trackListTable.Columns[e.ColumnIndex].ColumnName;
            DataTable sortedDT = dv.ToTable();

            if (this.trackColumnOrderingFlagArray[e.ColumnIndex] == -1)
            {
                this.trackColumnOrderingFlagArray[e.ColumnIndex] = 0;
                trackListTable = sortedDT;
            }
            else if (this.trackColumnOrderingFlagArray[e.ColumnIndex] == 0)
            {
                this.trackColumnOrderingFlagArray[e.ColumnIndex] = 1;

                DataTable reversedDt = trackListTable.Clone();
                for (var row = trackListTable.Rows.Count - 1; row >= 0; row--)
                    reversedDt.ImportRow(trackListTable.Rows[row]);
                trackListTable = reversedDt;
            }
            else if (this.trackColumnOrderingFlagArray[e.ColumnIndex] == 1)
            {
                this.trackColumnOrderingFlagArray[e.ColumnIndex] = 0;
                trackListTable = sortedDT;
            }

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }

        //REMOVE TRACKS
        private void Clear(object sender, EventArgs e)
        {
            trackListTable.Rows.Clear();

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }
        private void RemoveMissingTracks(object sender, EventArgs e)
        {
            for (int i = trackListTable.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(trackListTable.Rows[i]["IsMissing"]))
                {
                    trackListTable.Rows[i].Delete();
                }
            }

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }
        private void RemoveDuplicatedTracks(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            List<int> removableIds = new List<int>();
            for (int i = trackListTable.Rows.Count - 1; i >= 0; i--)
            {
                if (!ids.Contains(Convert.ToInt32(trackListTable.Rows[i]["Id"]))){
                    ids.Add(Convert.ToInt32(trackListTable.Rows[i]["Id"]));
                }
                else
                {
                    trackListTable.Rows[i].Delete();
                }
            }

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }
        private void DeleteTracks(object sender, ListEventArgs e)
        {
            for(int i = e.Rows.Count-1; i >=0;i--)
            {
                if (e.Rows[i].Selected)
                {
                    trackListTable.Rows[i].Delete();
                }
            }

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        } 
       
        //DRAG AND DROP
        private void TrackDragAndDrop(object sender, ListEventArgs e)
        {
            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }

        //ADD AND EDIT PLAYLIST
        private void ShowPlaylistEditorView(object sender, ListEventArgs e)
        {
            if (e.IntegerField1 == -1)
            {
                //ADD
                PlaylistEditorView editorView = new PlaylistEditorView();
                PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(editorView, this.playlistDao, this.settingDao);

                if (editorView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
                {
                    Playlist newPlaylist = presenter.newPlaylist;
                    if(newPlaylist != null)
                    {
                        this.playlistDao.CreatePlaylist(newPlaylist);
                        playlistListTable.Rows.Add(newPlaylist.Id, newPlaylist.Name, newPlaylist.OrderInList);
                    }
                    this.SavePlaylistList(playlistListTable);
                    this.SetPlaylistList(playlistListTable);
                }
            }
            else
            {
                //EDIT

                Playlist playlist = new Playlist();
                playlist.Id = Convert.ToInt32(playlistListTable.Rows[e.IntegerField1]["Id"]);
                playlist.Name = playlistListTable.Rows[e.IntegerField1]["Name"].ToString();
                playlist.OrderInList = Convert.ToInt32(playlistListTable.Rows[e.IntegerField1]["OrderInList"]);

                String defaultPlaylistName = this.settingDao.GetStringSettingByName(Settings.DefaultPlaylistName.ToString(), true);
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
                            playlistListTable.Rows[e.IntegerField1]["Name"] = newPlaylist.Name;
                        }
                        this.SavePlaylistList(playlistListTable);
                        this.SetPlaylistList(playlistListTable);
                    }
                }

                
            }
        }

        //LOAD PLAYLIST
        private void LoadPlaylist(object sender, ListEventArgs e)
        {
            if (e.IntegerField1 > -1)
            {
                Playlist playlist = new Playlist();
                playlist.Id = Convert.ToInt32(playlistListTable.Rows[e.IntegerField1]["Id"]);
                playlist.Name = playlistListTable.Rows[e.IntegerField1]["Name"].ToString();
                playlist.OrderInList = Convert.ToInt32(playlistListTable.Rows[e.IntegerField1]["OrderInList"]);

                this.currentPlaylistId = playlist.Id;
                this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId);

                trackListTable.Rows.Clear();

                this.LoadPlaylist(playlist);
            }
        }

        //DELETE PLAYLIST
        private void DeletePlaylist(object sender, ListEventArgs e)
        {
            if (e.IntegerField1 > -1)
            {
                String playlistName = playlistListTable.Rows[e.IntegerField1]["Name"].ToString();
                int playlistId = Convert.ToInt32(playlistListTable.Rows[e.IntegerField1]["Id"]);

                String defaultPlaylistName = this.settingDao.GetStringSettingByName(Settings.DefaultPlaylistName.ToString(), true);
                if (playlistName.Equals(defaultPlaylistName))
                {
                    MessageBox.Show("Default playlist cannot be deleted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DialogResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        playlistListTable.Rows.RemoveAt(e.IntegerField1);
                        playlistDao.DeletePlaylist(playlistId);

                        this.SavePlaylistList(playlistListTable);
                        this.SetPlaylistList(playlistListTable);
                        
                        if (playlistId == this.currentPlaylistId)
                        {
                            trackListTable.Rows.Clear();
                            this.SaveTrackList(trackListTable);
                            this.SetTrackList(trackListTable);

                            this.currentPlaylistId = Convert.ToInt32(playlistListTable.Rows[0]["Id"]);
                            this.settingDao.SetIntegerSetting(Settings.CurrentPlaylistId.ToString(), this.currentPlaylistId);
                        }
                    }
                }

            }
        }
    }
}
