using FlacLibSharp;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.IViews;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    internal class LiveStreamAnimationPresenter
    {

        private ILiveStreamAnimationView view;
        private ITagDao tagDao { get; set; }
        private ITrackDao trackDao { get; set; }
        private ISettingDao settingDao { get; set; }
        private MediaPlayerComponent mediaPlayerComponent { get; set; }
        private PlaylistPresenter playlistPresenter { get; set; }

        public bool ViewIsReadyToShow { get; set; }

        private string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        private List<String> imagePathList = new List<String>();
        private Random random { get; set; }
        public LiveStreamAnimationPresenter(ILiveStreamAnimationView liveStreamAnimationView, PlaylistPresenter playlistPresenter, MediaPlayerComponent mediaPlayerComponent, ITagDao tagDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = liveStreamAnimationView;
            this.playlistPresenter = playlistPresenter;

            this.mediaPlayerComponent = mediaPlayerComponent;
            this.tagDao = tagDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;
            this.random = new Random();

            this.view.TrackChangeCheckEvent += TrackChangeCheckEvent;
            this.view.CloseViewEvent += View_CloseViewEvent;

            this.ViewIsReadyToShow = this.IsImageDirectoryPrepared();

            if (this.ViewIsReadyToShow)
            {
                this.InitializeView(true);
                this.playlistPresenter.IsPlayTrackEnabled = false;
            }
            
        }

        private void View_CloseViewEvent(object sender, EventArgs e)
        {
            this.playlistPresenter.IsPlayTrackEnabled = true;
            ((LiveStreamAnimationView)this.view).Close();
        }

        private bool IsImageDirectoryPrepared()
        {
            bool result = false;

            String imageDirectoryPath = this.settingDao.GetStringSetting(Settings.LiveStreamAnimationImagePath.ToString()).Value;
            if (!Directory.Exists(imageDirectoryPath))
            {
                MessageBox.Show("Image directory does not exist. Please set it in the Live Stream Animation Setting menu!\n", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                result = false;
            }
            else
            {
                this.imagePathList = new List<String>();
                foreach (string extension in imageExtensions)
                {
                    this.imagePathList.AddRange(Directory.GetFiles(imageDirectoryPath, "*" + extension));
                }
                if (this.imagePathList == null || this.imagePathList.Count == 0)
                {
                    MessageBox.Show("Directory does not have images!\n", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        public void InitializeView(bool firstInitialization = false)
        {
            Messenger msg = new Messenger();
            

            if (firstInitialization)
            {
                msg.StringList = this.imagePathList;
                msg.BooleanField1 = firstInitialization;
                this.view.FirstInitialization(msg);
            }
            else
            {
                msg.Image = this.currentCoverImage;
                msg.StringField2 = this.currentTrackArtist;
                msg.StringField3 = this.currentTrackTitle;
                msg.StringField4 = this.currentTrackAlbum;
                this.view.InitializeView(msg);
            }
            
        }

        private bool IsHistoryFileExists { get; set; }
        private String currentTrackArtist { get; set; }
        private String currentTrackTitle { get; set; }
        private String currentTrackAlbum { get; set; }
        private void TrackChangeCheckEvent(object sender, EventArgs e)
        {
            this.CheckTrackChange();
        }
        public void CheckTrackChange()
        {
            const Int32 BufferSize = 128;

            DateTime date = DateTime.Now;
            if (date.Hour < 6)
            {
                date = date.AddDays(-1);
            }

            String vdjHistoryDirectory = "C:\\Users\\Mitoklin\\Documents\\VirtualDJ\\History";
            String dateInString = date.Year.ToString() + "-" + date.Month.ToString("00") + "-" + date.Day.ToString("00");
            String extension = "m3u";
            String fileName = vdjHistoryDirectory + "\\" + dateInString + "." + extension;

            //Meph, Notequal, Konquest - We Are The Darkshire (Official Darkshire Festival 2023 Anthem)
            //22,42,

            if (!File.Exists(fileName))
            {
                this.IsHistoryFileExists = false;
            }
            else
            {
                this.IsHistoryFileExists = true;

                using (var fileStream = File.OpenRead(fileName))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line; String filePath = String.Empty;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (!line.StartsWith("#EXTVDJ"))
                        {
                            if (line.EndsWith(".mp3") || line.EndsWith(".wav") || line.EndsWith(".flac"))
                            {
                                filePath = line;
                                //line = line.Remove(line.Length - 4);
                            }
                           /* else if (line.EndsWith(".flac"))
                            {
                                filePath = line;
                               // line = line.Remove(line.Length - 5);
                            }*/

                            //line = line.Substring(line.LastIndexOf("\\") + 1);
                            //this.currentTrackTitle = line;
                        }
                    }

                    /*if (String.IsNullOrEmpty(this.Fsm.GetFsmString("STR_Track_Title").Value))
                    {
                        this.Fsm.GetFsmString("STR_Track_Title").Value = this.TrackTitle.ToUpper();
                        this.SetTrackCover(filePath);
                    }
                    else
                    {
                        if (!this.Fsm.GetFsmString("STR_Track_Title").Value.Equals(this.TrackTitle.ToUpper()))
                        {

                            this.Fsm.GetFsmString("STR_Track_Title").Value = this.TrackTitle.ToUpper();
                            this.SetTrackCover(filePath);
                        }
                    }*/

                    this.SetTrackTitleAndCover(filePath);

                }
            }


        }
        private Image currentCoverImage { get; set; }
        private void SetTrackTitleAndCover(String path)
        {
            String fileName = Path.GetFileName(path);

            this.currentCoverImage = null;
            this.currentTrackArtist = String.Empty;
            this.currentTrackTitle = String.Empty;
            this.currentTrackAlbum = String.Empty;

            if (path.EndsWith(".mp3"))
            {
                using (var file = TagLib.File.Create(path))
                {
                    this.currentTrackArtist = file.Tag.FirstArtist ?? file.Tag.FirstAlbumArtist ?? file.Tag.Artists.FirstOrDefault() ?? path;
                    this.currentTrackTitle = file.Tag.Title;
                    this.currentTrackAlbum = file.Tag.Album;

                    if (file?.Tag.Pictures.Length > 0)
                    {
                        var pictureData = file.Tag.Pictures[0].Data.Data;
                        using (var ms = new MemoryStream(pictureData))
                        {
                            ms.Position = 0;
                            using (var img = Image.FromStream(ms))
                            {
                                this.currentCoverImage = img.GetThumbnailImage(521, 512, () => false, IntPtr.Zero);
                            }
                        }
                    }
                }
            }
            else if (path.EndsWith(".wav"))
            {
                using (var wf = new WaveFileReader(path))
                {
                    this.currentTrackArtist = fileName;
                }
            }
            else if (path.EndsWith(".flac"))
            {
                using (var file = new FlacFile(path))
                {
                    var vorbisComment = file.VorbisComment;
                    if (vorbisComment != null)
                    {
                        if(vorbisComment.Artist != null && vorbisComment.Title != null)
                        {
                            this.currentTrackArtist = vorbisComment.Artist.FirstOrDefault();
                            this.currentTrackTitle = vorbisComment.Title.FirstOrDefault();
                        }
                        else if(vorbisComment.Artist == null && vorbisComment.Title != null)
                        {
                            this.currentTrackArtist = vorbisComment.Title.FirstOrDefault();
                        }
                        else if (vorbisComment.Artist != null && vorbisComment.Title == null)
                        {
                            this.currentTrackTitle = vorbisComment.Artist.FirstOrDefault();
                        }
                        else
                        {
                            this.currentTrackArtist = fileName;
                        }
                        this.currentTrackAlbum = vorbisComment.Album.FirstOrDefault();

                        foreach (var block in file.Metadata)
                        {
                            if (block is Picture picture)
                            {

                                var pictureData = picture.Data;
                                using (var ms = new MemoryStream(pictureData))
                                {
                                    ms.Position = 0;
                                    using (var img = Image.FromStream(ms))
                                    {
                                        this.currentCoverImage = img.GetThumbnailImage(521, 512, () => false, IntPtr.Zero);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            if (this.currentCoverImage == null)
            {
                this.currentCoverImage = Properties.Resources.MissingCover;
            }

            this.InitializeView();
        }

        private void CloseViewWithOkEvent(object sender, EventArgs e)
        {
            ((LiveStreamAnimationView)this.view).DialogResult = DialogResult.OK;
            ((LiveStreamAnimationView)this.view).Close();
        }
        private void CloseViewWithCancelEvent(object sender, EventArgs e)
        {
            ((LiveStreamAnimationView)this.view).DialogResult = DialogResult.Cancel;
            ((LiveStreamAnimationView)this.view).Close();
        }
    }
}
