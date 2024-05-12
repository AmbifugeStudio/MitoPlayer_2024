using AxWMPLib;
using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using NAudio.Wave;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Presenters
{
    public class MainPresenter
    {
        private IMainView mainView { get; set; }
        private IPlaylistDao playlistDao { get; set; }
        private ITrackDao trackDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private MediaPlayerComponent mediaPlayerComponent { get; set; }
        private string sqlConnectionString { get; set; }
      //  private DataTable workingTable { get; set; }
      // private int currentPlaylistId { get; set; }
        private object actualView { get => null; set
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                this.workingTable = this.playlistPresenter.trackListTable;
                this.currentPlaylistId = this.playlistPresenter.currentPlaylistId;
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView)){
                this.workingTable = this.trackEditorPresenter.trackListTable;
                this.currentPlaylistId = this.trackEditorPresenter.currentPlaylistId;
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                this.workingTable = this.harmonizerPresenter.trackListTable;
                this.currentPlaylistId = this.harmonizerPresenter.currentPlaylistId;
            }
        }}
        private IProfileEditorView profileEditorView { get; set; }
        private IPlaylistView playlistView { get; set; }
        private ITagValueEditorView tagValueEditorView { get; set; }
        private IRuleEditorView ruleEditorView { get; set; }
        private ITrackEditorView trackEditorView { get; set; }
        private ITemplateEditorView templateEditorView { get; set; }
        private IHarmonizerView harmonizerView { get; set; }
        private IPreferencesView preferencesView { get; set; }
        private IAboutView aboutView { get; set; }
        private ProfileEditorPresenter profileEditorPresenter { get; set; }
        private PlaylistPresenter playlistPresenter { get; set; }
        private TagValueEditorPresenter tagValueEditorPresenter { get; set; }
        private RuleEditorPresenter ruleEditorPresenter { get; set; }
        private TrackEditorPresenter trackEditorPresenter { get; set; }
        private TemplateEditorPresenter templateEditorPresenter { get; set; }
        private HarmonizerPresenter harmonizerPresenter { get; set; }
        private PreferencesPresenter preferencesPresenter { get; set; }
        private AboutPresenter aboutPresenter { get; set; }
        private string[] scannedFileNames { get; set; }

        public MainPresenter(IMainView mainView, string sqlConnectionString)
        {
            this.mainView = mainView;
            this.sqlConnectionString = sqlConnectionString;

            this.mainView.ShowProfileEditorView += ShowProfileEditorView;
            this.mainView.ShowPlaylistView += ShowPlaylistView;
            this.mainView.ShowTagValueEditorView += ShowTagValueEditorView;
            this.mainView.ShowRuleEditorView += ShowRuleEditorView;
            this.mainView.ShowTrackEditorView += ShowTrackEditorView;
            this.mainView.ShowTemplateEditorView += ShowTemplateEditorView;
            this.mainView.ShowHarmonizerView += ShowHarmonizerView;
            this.mainView.ShowPreferencesView += ShowPreferencesView;
            this.mainView.ShowAboutView += ShowAboutView;

            this.mainView.OpenFiles += OpenFiles;
            this.mainView.OpenDirectory += OpenDirectory;
            this.mainView.CreatePlaylist += CreatePlaylist;
            this.mainView.LoadPlaylist += LoadPlaylist;
            this.mainView.RenamePlaylist += RenamePlaylist;
            this.mainView.DeletePlaylist += DeletePlaylist;
            this.mainView.Preferences += Preferences;
            this.mainView.Exit += Exit;

            this.mainView.RemoveMissingTracks += RemoveMissingTracks;
            this.mainView.RemoveDuplicatedTracks += RemoveDuplicatedTracks;
            this.mainView.OrderByTitle += OrderByTitle;
            this.mainView.OrderByArtist += OrderByArtist;
            this.mainView.OrderByFileName += OrderByFileName;
            this.mainView.Reverse += Reverse;
            this.mainView.Shuffle += Shuffle;
            this.mainView.Clear += Clear;

            this.mainView.PlayTrack += PlayTrack;
            this.mainView.PauseTrack += PauseTrack;
            this.mainView.StopTrack += StopTrack;
            this.mainView.PrevTrack += PrevTrack;
            this.mainView.NextTrack += NextTrack;
            this.mainView.RandomTrack += RandomTrack;

            this.mainView.About += About;

            this.mainView.ScanFiles += ScanFiles;
        }

        public void Initialize()
        {
            this.mediaPlayerComponent = new MediaPlayerComponent(((MainView)this.mainView).mediaPlayer);

            this.actualView = null;
            this.playlistDao = new PlaylistDao(sqlConnectionString);
            this.trackDao = new TrackDao(sqlConnectionString);
            this.settingDao = new SettingDao(sqlConnectionString);

            this.ShowPlaylistView(this, new EventArgs());
        }

        //VIEWS
        private void ShowProfileEditorView(object sender, EventArgs e)
        {
            this.profileEditorView = ProfileEditorView.GetInstance((MainView)mainView);
            this.actualView = this.profileEditorView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.profileEditorPresenter = new ProfileEditorPresenter(this.profileEditorView, this.playlistDao, this.trackDao, this.settingDao);
        }
        private void ShowPlaylistView(object sender, EventArgs e)
        {
            this.playlistView = PlaylistView.GetInstance((MainView)mainView); 
            this.actualView = this.playlistView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.playlistPresenter = new PlaylistPresenter(this.playlistView, this.mediaPlayerComponent, this.playlistDao, this.trackDao, this.settingDao);
        }
        private void ShowTagValueEditorView(object sender, EventArgs e)
        {
            this.tagValueEditorView = TagValueEditorView.GetInstance((MainView)mainView);
            this.actualView = this.tagValueEditorView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.tagValueEditorPresenter = new TagValueEditorPresenter(this.tagValueEditorView, this.playlistDao, this.trackDao, this.settingDao);
        }
        private void ShowTrackEditorView(object sender, EventArgs e)
        {
            this.trackEditorView = TrackEditorView.GetInstance((MainView)mainView);
            this.actualView = this.trackEditorView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.trackEditorPresenter = new TrackEditorPresenter(this.trackEditorView, this.mediaPlayerComponent, this.playlistDao, this.trackDao, this.settingDao);
        }
        private void ShowRuleEditorView(object sender, EventArgs e)
        {
            this.ruleEditorView = RuleEditorView.GetInstance((MainView)mainView);
            this.actualView = this.ruleEditorView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.ruleEditorPresenter = new RuleEditorPresenter(this.ruleEditorView, this.playlistDao, this.trackDao, this.settingDao);
        }
        private void ShowTemplateEditorView(object sender, EventArgs e)
        {
            this.templateEditorView = TemplateEditorView.GetInstance((MainView)mainView);
            this.actualView = this.templateEditorView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.templateEditorPresenter = new TemplateEditorPresenter(this.templateEditorView, this.playlistDao, this.trackDao, this.settingDao);
        }
        private void ShowHarmonizerView(object sender, EventArgs e)
        {
            this.harmonizerView = HarmonizerView.GetInstance((MainView)mainView);
            this.actualView = this.harmonizerView;
            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);
            this.harmonizerPresenter = new HarmonizerPresenter(this.harmonizerView, this.mediaPlayerComponent, this.playlistDao, this.trackDao, this.settingDao);
        }
        private void ShowPreferencesView(object sender, EventArgs e)
        {
            this.preferencesView = PreferencesView.GetInstance((MainView)mainView);
            this.actualView = this.preferencesView;
            this.preferencesPresenter = new PreferencesPresenter(this.preferencesView,  this.playlistDao, this.trackDao, this.settingDao);
        }
        private void ShowAboutView(object sender, EventArgs e)
        {
            this.aboutView = AboutView.GetInstance((MainView)mainView);
            this.actualView = this.aboutView;
            this.aboutPresenter = new AboutPresenter(this.aboutView, this.playlistDao, this.trackDao, this.settingDao);
        }

        //MENU STRIP
        //FILE
        private void OpenFiles(object sender, EventArgs e)
        {
            List<Track> trackList = new List<Track>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Audio files (*.mp3,*.wav,*.flac)|*.mp3;*.wav;*.flac|Playlist files (*.m3u)|*.m3u";
            ofd.FilterIndex = this.settingDao.GetIntegerSettingByName(Settings.LastOpenFilesFilterIndex.ToString(), true);

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                trackList = this.ReadFiles(ofd.FileNames);
            }

            this.settingDao.SetIntegerSetting(Settings.LastOpenFilesFilterIndex.ToString(), ofd.FilterIndex);

            this.AddTracksToTrackList(trackList);
            
            if (this.mediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.PlayTrack();
            }
        }
        private void OpenDirectory(object sender, EventArgs e)
        {
            scannedFileNames = null;
            List<Track> trackList = new List<Track>();

            using (var fbd = new FolderBrowserDialog())
            {
                String lastDirectoryPath = this.settingDao.GetStringSettingByName(Settings.LastOpenDirectoryPath.ToString(), true);
                if (System.IO.File.Exists(lastDirectoryPath))
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
            
            if (this.mediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.PlayTrack();
            }
        }
        private void CreatePlaylist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallCreatePlaylistEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallCreatePlaylistEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallCreatePlaylistEvent();
        }
        private void LoadPlaylist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallLoadPlaylistEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallLoadPlaylistEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallLoadPlaylistEvent();
        }
        private void RenamePlaylist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallRenamePlaylistEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallRenamePlaylistEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallRenamePlaylistEvent();
        }
        private void DeletePlaylist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallDeletePlaylistEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallDeletePlaylistEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallDeletePlaylistEvent();
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
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.RemoveMissingTracks();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.RemoveMissingTracks();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.RemoveMissingTracks();
        }
        private void RemoveDuplicatedTracks(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.RemoveDuplicatedTracks();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.RemoveDuplicatedTracks();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.RemoveDuplicatedTracks();
        }
        private void OrderByTitle(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.OrderByTitle();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.OrderByTitle();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.OrderByTitle();
        }
        private void OrderByArtist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.OrderByArtist();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.OrderByArtist();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.OrderByArtist();
        }
        private void OrderByFileName(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.OrderByFileName();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.OrderByFileName();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.OrderByFileName();
        }
        private void Shuffle(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.Shuffle();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.Shuffle();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.Shuffle();
        }
        private void Reverse(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.Reverse();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.Reverse();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.Reverse();
        }
        private void Clear(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                this.playlistPresenter.Clear();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                this.trackEditorPresenter.Clear();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                this.harmonizerPresenter.Clear();
        }

        //PLAYBACK
        private void PlayTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallPlayTrackEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallPlayTrackEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallPlayTrackEvent();
        }
       
        private void PauseTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallPauseTrackEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallPauseTrackEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallPauseTrackEvent();
        }
        
        private void StopTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallStopTrackEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallStopTrackEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallStopTrackEvent();
        }
        
        private void PrevTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallPrevTrackEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallPrevTrackEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallPrevTrackEvent();
        }
        private void NextTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallNextTrackEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallNextTrackEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallNextTrackEvent();
        }
        private void RandomTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
                ((PlaylistView)this.playlistView).CallRandomTrackEvent();
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
                ((TrackEditorView)this.trackEditorView).CallRandomTrackEvent();
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
                ((HarmonizerView)this.harmonizerView).CallRandomTrackEvent();
        }

//HELP
        private void About(object sender, EventArgs e)
        {
            this.ShowAboutView(this, new EventArgs());
        }

//ADD FILES
        private void AddTracksToTrackList(List<Track> trackList)
        {
            if (trackList != null && trackList.Count > 0)
            {
                int orderInList = this.workingTable.Rows.Count;
                foreach (Track track in trackList)
                {
                    track.OrderInList = orderInList;
                    this.trackDao.AddTrackToPlaylist(this.GetNewPlaylistContentId(), this.currentPlaylistId, track.Id, track.OrderInList);
                    orderInList++;
                }
            }
        }
        private int GetNewPlaylistContentId()
        {
            int id = this.playlistDao.GetLastObjectId(TableName.PlaylistContent.ToString());
            if (id == -1)
                return 0;
            else
                return id + 1;
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

                foreach (string path in filePathList)
                {
                    string fileName = "";

                    Track track = new Track();
                    track.Path = path;

                    int extCharCount = path.EndsWith("flac") ? 5 : 4;
                    fileName = path.Substring(path.LastIndexOf(@"\") + 1);
                    fileName = fileName.Remove(fileName.LastIndexOf("."), extCharCount);

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
                        track.Id = this.GetNewTrackId();
                        this.trackDao.AddTrackToDatabase(track);
                    }
                    trackList.Add(track);
                }
            }
            return trackList;
        }
        private int GetNewTrackId()
        {
            int id = this.playlistDao.GetLastObjectId(TableName.Track.ToString());
            if (id == -1)
                return 0;
            else
                return id + 1;
        }
        private int dragIndex = -1;
        private String[] scannedFiles;
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

    }
}
