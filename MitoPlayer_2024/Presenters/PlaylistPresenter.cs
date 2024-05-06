using AxWMPLib;
using MitoPlayer_2024._Repositories;
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

namespace MitoPlayer_2024.Presenters
{
    public enum TableName {
        Playlist,
        Track,
        PlaylistContent
    }
    public class PlaylistPresenter
    {
        //FIELDS
        private IPlaylistView playlistView;
        private IPlaylistDao playlistDao;
        private ITrackDao trackDao;

        private BindingSource playlistListBindingSource;
        private BindingSource trackListBindingSource;
        private BindingSource selectedTrackListBindingSource;

        private DataTable playlistListTable;
        private DataTable trackListTable;
        private DataTable selectedTrackListTable;

        private List<PlaylistModel> playlistList;
        private List<TrackModel> tracklist;

        private string[] scannedFiles;
        private int[] columnOrder;

        private int lastTrackID = 0;

        private int CurrentPlaylistId;

        //private IPlaylistEditorView playlistEditorView;

        //CONSTRUCTOR
        public PlaylistPresenter(IPlaylistView view, IPlaylistDao playlistDao, ITrackDao trackDao)
        {
            this.playlistView = view;
            this.playlistDao = playlistDao;
            this.trackDao = trackDao;
            this.columnOrder = new int[8] {-1,-1,-1,-1,-1,-1,-1,-1};


            //TABLE BINDING SOURCES

            this.playlistListBindingSource = new BindingSource();
            playlistListTable = new DataTable();
            playlistListTable.Columns.Add("Id", typeof(int));
            playlistListTable.Columns.Add("Name", typeof(string));
            playlistListTable.Columns.Add("OrderInList", typeof(int));

            this.SetPlaylistList(playlistListTable);

            this.trackListBindingSource = new BindingSource();
            trackListTable = new DataTable();
            trackListTable.Columns.Add("Id", typeof(int));
            trackListTable.Columns.Add("Album", typeof(string));
            trackListTable.Columns.Add("Artist", typeof(string));
            trackListTable.Columns.Add("Title", typeof(string));
            trackListTable.Columns.Add("Year", typeof(string));
            trackListTable.Columns.Add("Length", typeof(string));
            trackListTable.Columns.Add("IsMissing", typeof(bool));
            trackListTable.Columns.Add("Path", typeof(string));
            trackListTable.Columns.Add("FileName", typeof(string));
            trackListTable.Columns.Add("IdInPlaylist", typeof(int));
            this.SetTrackList(trackListTable);

            this.selectedTrackListBindingSource = new BindingSource();
            selectedTrackListTable = new DataTable();
            selectedTrackListTable.Columns.Add("Id", typeof(int));
            selectedTrackListTable.Columns.Add("Album", typeof(string));
            selectedTrackListTable.Columns.Add("Artist", typeof(string));
            selectedTrackListTable.Columns.Add("Title", typeof(string));
            selectedTrackListTable.Columns.Add("Year", typeof(string));
            selectedTrackListTable.Columns.Add("Length", typeof(string));
            selectedTrackListTable.Columns.Add("IsMissing", typeof(bool));
            selectedTrackListTable.Columns.Add("Path", typeof(string));
            selectedTrackListTable.Columns.Add("FileName", typeof(string));
            selectedTrackListTable.Columns.Add("IdInPlaylist", typeof(int));

            
            selectedTrackListBindingSource.DataSource = selectedTrackListTable;
            this.playlistView.SetSelectedTrackListBindingSource(selectedTrackListBindingSource);


            //EVENTS
            this.playlistView.OpenFiles += OpenFiles;
            this.playlistView.OpenDirectory += OpenDirectory;
            this.playlistView.ScanFiles += ScanFiles;
            this.playlistView.OrderByArtist += OrderByArtist;
            this.playlistView.OrderByTitle += OrderByTitle;
            this.playlistView.OrderByFileName += OrderByFileName;
            this.playlistView.Reverse += Reverse;
            this.playlistView.Shuffle += Shuffle;
            this.playlistView.Clear += Clear;
            this.playlistView.DeleteRows += DeleteRows;
            this.playlistView.OrderTableByColumn += OrderTableByColumn;
            this.playlistView.DragAndDrop += DragAndDrop;

            this.playlistView.ShowPlaylistEditorView += ShowPlaylistEditorView;
            this.playlistView.DeletePlaylist += DeletePlaylist;
            this.playlistView.LoadPlaylist += LoadPlaylist;

            //LOAD ALL PLAYLIST
            this.LoadAllPlaylist();

            //SHOW VIEW
            this.playlistView.Show();
        }

        


        //METHODS



        //LOAD ALL PLAYLIST

        /*
         * betölti a rendszerbe már felvett plalyist-eket
         * ha még nincs playlist egyáltalán, akkor megcsinálta a default playlist-et
         * betölti a current playlist-et (ebből csak 1 lehet)
         */
        private void LoadAllPlaylist()
        {
            playlistListTable.Clear();

            playlistList = playlistDao.GetAllPlaylist();

            if(playlistList == null || playlistList.Count() <= 0)
            {
                PlaylistModel playlist = new PlaylistModel();
                playlist.Id = this.GetNewPlaylistId();
                playlist.Name = ConfigurationManager.AppSettings["DefaultPlaylistName"];
                playlist.OrderInList = 0;
                playlistList.Add(playlist);
                CreatePlaylist(playlist);

                this.CurrentPlaylistId = playlist.Id;
                ConfigurationManager.AppSettings["CurrentPlaylistId"] = this.CurrentPlaylistId.ToString();

                LoadPlaylist(playlist);
            }
            else
            {
                this.CurrentPlaylistId = int.Parse(ConfigurationManager.AppSettings["CurrentPlaylistId"]);

                LoadPlaylist(playlistList[this.CurrentPlaylistId]);
            }

            foreach (var playlist in playlistList)
            {
                playlistListTable.Rows.Add(playlist.Id, playlist.Name, playlist.OrderInList);
            }

            this.SetPlaylistList(playlistListTable);
        }
        private void SetPlaylistList(DataTable playlistListTable)
        {
            this.playlistListBindingSource.DataSource = playlistListTable;
            this.playlistView.SetPlaylistListBindingSource(this.playlistListBindingSource);
        }
        /*
         * létrehoz egy új üres playlist-et a megadott paraméterekkel
         */
        private void CreatePlaylist(PlaylistModel model)
        {
            try
            {
                playlistDao.CreatePlaylist(model);
                playlistView.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                playlistView.IsSuccessful = false;
                playlistView.Message = ex.Message;
            }
        }
        /*
         * betölti a megadott playlist track-jeit
         */
        private void LoadPlaylist(PlaylistModel playlist)
        {
            List<TrackModel> trackList = playlistDao.LoadPlaylist(playlist);
            if(trackList != null && trackList.Count > 0)
            {
                LoadTrackList(trackList);
            }
        }

        private void LoadPlaylist(object sender, ListEventArgs e)
        {
            PlaylistModel playlistModel = this.playlistDao.GetPlaylistByName(e.StringField1);
            if (playlistModel != null)
            {
                this.CurrentPlaylistId = playlistModel.Id;
                ConfigurationManager.AppSettings["CurrentPlaylistId"] = this.CurrentPlaylistId.ToString();

                trackListTable.Rows.Clear();

                List<TrackModel> trackList = playlistDao.LoadPlaylist(playlistModel);

                foreach (TrackModel track in trackList)
                {
                    String length = this.LengthToString(track.Length);
                    trackListTable.Rows.Add(track.Id, track.Album, track.Artist, track.Title, track.Year.ToString(), length, track.IsMissing, track.Path, track.FileName, track.IdInPlaylist);
                }

                this.SetTrackList(trackListTable);
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

            if(lengthParts.Length == 2)
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

        /*
         * végig meg a playlist felvételein, átkonvertálja a hosszt sztringekké és hozzáadja a grid datatable-jéhez őket
         */
        private void LoadTrackList(List<TrackModel> trackList)
        {
            if (trackList != null && trackList.Count > 0)
            {
                foreach (TrackModel track in trackList)
                {
                    String length = this.LengthToString(track.Length);
                    trackListTable.Rows.Add(track.Id, track.Album, track.Artist, track.Title, track.Year.ToString(), length, track.IsMissing, track.Path, track.FileName, track.IdInPlaylist);
                }
               
                this.SetTrackList(trackListTable);
            }
        }
        private void SetTrackList(DataTable trackListTable)
        {
            this.trackListBindingSource.DataSource = trackListTable;
            this.playlistView.SetTrackListBindingSource(this.trackListBindingSource);
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
            filterIndex = int.Parse(ConfigurationManager.AppSettings["LastOpenFilesFilterIndex"]); 
            ofd.FilterIndex = filterIndex;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.ReadFiles(ofd.FileNames);
            }

            ConfigurationManager.AppSettings["LastOpenFilesFilterIndex"] = ofd.FilterIndex.ToString(); 
        }
        //OPEN DIRECTORY
        private void OpenDirectory(object sender, EventArgs e)
        {
            scannedFiles = null;
            using (var fbd = new FolderBrowserDialog())
            {
                String path = ConfigurationManager.AppSettings["LastOpenDirectoryPath"];
                if (System.IO.File.Exists(path))
                {
                    fbd.SelectedPath = path;
                }

                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    ScanDirectory(fbd.SelectedPath);
                    ReadFiles(scannedFiles);
                }
                ConfigurationManager.AppSettings["LastOpenDirectoryPath"] = fbd.SelectedPath;
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
                ScanDirectory(dir);
            }
        }
        private void ScanFiles(object sender, ListEventArgs e)
        {

            string[] mediaFiles;
            string[] directories;
            if(e.DragAndDropFiles != null && e.DragAndDropFiles.Length > 0)
            {
                mediaFiles = e.DragAndDropFiles.Where(x => x.EndsWith(".mp3") || x.EndsWith(".wav") || x.EndsWith(".flac") || x.EndsWith(".m3u")).ToArray();
                directories = e.DragAndDropFiles.Where(x => !x.EndsWith(".mp3") && !x.EndsWith(".wav") && !x.EndsWith(".flac") && !x.EndsWith(".m3u")).ToArray();

                if (mediaFiles != null && mediaFiles.Length > 0)
                {
                    ReadFiles(mediaFiles);
                }
                if (directories != null && directories.Length > 0)
                {
                    scannedFiles = null;
                    foreach (string dir in directories)
                    {
                        ScanDirectory(dir);
                        ReadFiles(scannedFiles);
                    }
                }

            }
        }
        //CREATE TRACKS
        private void ReadFiles(string[] fileNames)
        {
            List<String> filePathList = new List<String>();
            List<TrackModel> trList = new List<TrackModel>();
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

                int sortingId= trackListTable.Rows.Count;
                int idInPlaylist = trackListTable.Rows.Count;

                foreach (string path in filePathList)
                {
                    TrackModel track = new TrackModel();
                    track.Path = path;

                    string fileName = path.Substring(path.LastIndexOf(@"\") + 1);
                    fileName = fileName.Remove(fileName.LastIndexOf("."), 4);
                    track.FileName = fileName;

                    if (!System.IO.File.Exists(path))
                    {
                        track.Artist = fileName;
                        track.IsMissing = true;
                    }
                    else
                    {
                        TrackModel trackFromDb = this.trackDao.GetTrackByPath(track.Path);
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
                        track.Id = GetNewTrackId();
                        this.trackDao.AddTrackToDatabase(track);
                    }

                    track.IdInPlaylist = idInPlaylist;

                    int currentPlaylistIndex = 0;
                    PlaylistModel pls = playlistList.Find(x => x.Id == this.CurrentPlaylistId);
                    if (pls != null)
                    {
                        currentPlaylistIndex = pls.Id; 
                    }
                    this.trackDao.AddTrackToPlaylist(GetNewPlaylistContentId(), currentPlaylistIndex, track.Id, sortingId++, idInPlaylist++);

                    trList.Add(track);
                }
                LoadTrackList(trList);
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
        private void Clear(object sender, EventArgs e)
        {
            trackListTable.Rows.Clear();
            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }       
        private void DeleteRows(object sender, ListEventArgs e)
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
        private void OrderTableByColumn(object sender, ListEventArgs e)
        {
            DataView dv = trackListTable.DefaultView;
            dv.Sort = trackListTable.Columns[e.ColumnIndex].ColumnName;
            DataTable sortedDT = dv.ToTable();

            if (columnOrder[e.ColumnIndex] == -1)
            {
                columnOrder[e.ColumnIndex] = 0;
                trackListTable = sortedDT;
            } 
            else if (columnOrder[e.ColumnIndex] == 0)
            {
                columnOrder[e.ColumnIndex] = 1;

                DataTable reversedDt = trackListTable.Clone();
                for (var row = trackListTable.Rows.Count - 1; row >= 0; row--)
                    reversedDt.ImportRow(trackListTable.Rows[row]);
                trackListTable = reversedDt;
                trackListBindingSource.DataSource = trackListTable;
                this.playlistView.SetTrackListBindingSource(trackListBindingSource);
            }
            else if(columnOrder[e.ColumnIndex] == 1)
            {
                columnOrder[e.ColumnIndex] = 0;
                trackListTable = sortedDT;
            }

            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }

        private void DragAndDrop(object sender, ListEventArgs e)
        {
            this.SaveTrackList(trackListTable);
            this.SetTrackList(trackListTable);
        }


        private void SaveTrackList(DataTable trackListTable)
        {
            this.playlistDao.DeletePlaylistContent(this.CurrentPlaylistId);
            this.tracklist = ConvertDataTableToList(trackListTable);
            int sortingId = 0;

            int currentPlaylistIndex = 0;
            PlaylistModel pls = playlistList.Find(x => x.Id == this.CurrentPlaylistId);
            if (pls != null)
            {
                currentPlaylistIndex = pls.Id;
            }

            foreach (TrackModel track in this.tracklist)
            {
                this.trackDao.AddTrackToPlaylist(GetNewPlaylistContentId(), playlistList[currentPlaylistIndex].Id, track.Id, sortingId++, track.IdInPlaylist);
            }
        }
        private List<TrackModel> ConvertDataTableToList(DataTable dt)
        {
            List<TrackModel> trackList = new List<TrackModel>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TrackModel track = new TrackModel();
                track.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                track.Album = dt.Rows[i]["Album"].ToString();
                track.Artist = dt.Rows[i]["Artist"].ToString();
                track.Title = dt.Rows[i]["Title"].ToString();
                track.Year = Convert.ToInt32(dt.Rows[i]["Year"]);

                String length = dt.Rows[i]["Length"].ToString();
                track.Length = this.StringToLength(length);

                track.IsMissing = Convert.ToBoolean(dt.Rows[i]["IsMissing"]);
                track.Path = dt.Rows[i]["Path"].ToString();
                track.FileName = dt.Rows[i]["FileName"].ToString();
                track.IdInPlaylist = Convert.ToInt32(dt.Rows[i]["IdInPlaylist"]);
                trackList.Add(track);
            }

            return trackList;
        }


        private void ShowPlaylistEditorView(object sender, ListEventArgs e)
        {
            if (String.IsNullOrEmpty(e.StringField1))
            {
                //ADD
                PlaylistEditorView editorView = new PlaylistEditorView();
                PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(editorView, this.playlistDao);

                if (editorView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
                {
                    PlaylistModel newPlaylist = presenter.newPlaylist;
                    if(newPlaylist != null)
                    {
                       // playlistListTable.Rows.Add(newPlaylist.Id, newPlaylist.Name, );
                    }

                    playlistListTable.Rows.Clear();
                    playlistList = playlistDao.GetAllPlaylist();
                    foreach (var playlist in playlistList)
                    {
                        playlistListTable.Rows.Add(playlist.Id, playlist.Name, playlist.OrderInList);
                    }
                    this.SetPlaylistList(playlistListTable);
                }
            }
            else
            {
                //EDIT
                PlaylistModel playlistModel = this.playlistDao.GetPlaylistByName(e.StringField1);
                if(playlistModel != null)
                {
                    PlaylistEditorView editorView = new PlaylistEditorView();
                    PlaylistEditorPresenter presenter = new PlaylistEditorPresenter(editorView, this.playlistDao, playlistModel);

                    if (editorView.ShowDialog((PlaylistView)this.playlistView) == DialogResult.OK)
                    {
                        
                        playlistListTable.Rows.Clear();
                        playlistList = playlistDao.GetAllPlaylist();
                        foreach (var pls in playlistList)
                        {
                            playlistListTable.Rows.Add(pls.Name);
                        }
                        this.SetPlaylistList(playlistListTable);
                    }
                }

                
            }
            
        }

        private void DeletePlaylist(object sender, ListEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.StringField1))
            {
                PlaylistModel playlistModel = this.playlistDao.GetPlaylistByName(e.StringField1);
                if (playlistModel != null)
                {
                    if (playlistModel.Name.Equals(ConfigurationManager.AppSettings["DefaultPlaylistName"]))
                    {
                        MessageBox.Show("Default playlist cannot be deleted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DialogResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButtons.YesNo);
                        if (messageBoxResult == DialogResult.Yes)
                        {
                            if (this.CurrentPlaylistId == playlistModel.Id)
                            {
                                trackListTable.Rows.Clear();
                            }
                            this.SetTrackList(trackListTable);

                            playlistDao.DeletePlaylist(playlistModel);

                            DataRow row = playlistListTable.Select("Id = " + playlistModel.Id.ToString()).First();
                            if(row != null)
                            {
                                int idx = playlistListTable.Rows.IndexOf(row);
                                playlistListTable.Rows.RemoveAt(idx);
                            }

                            this.SetPlaylistList(playlistListTable);
                        }

                    }

                }
            }
        }
    }
}
