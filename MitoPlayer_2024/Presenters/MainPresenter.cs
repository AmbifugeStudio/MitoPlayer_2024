using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Presenters
{
    public class MainPresenter
    {
        private IMainView mainView;
        private readonly string sqlConnectionString;
        private object actualView;

        private IProfileEditorView profileEditorView;
        private IPlaylistView playlistView;
        private ITagValueEditorView tagValueEditorView;
        private IRuleEditorView ruleEditorView;
        private ITrackEditorView trackEditorView;
        private ITemplateEditorView templateEditorView;
        private IHarmonizerView harmonizerView;

        public MainPresenter(IMainView mainView, string sqlConnectionString)
        {
            this.mainView = mainView;
            this.sqlConnectionString = sqlConnectionString;
            this.actualView = null;

            //OPEN VIEWS
            this.mainView.ShowProfileEditorView += ShowProfileEditorView;
            this.mainView.ShowPlaylistView += ShowPlaylistView;
            this.mainView.ShowTagValueEditorView += ShowTagValueEditorView;
            this.mainView.ShowRuleEditorView += ShowRuleEditorView;
            this.mainView.ShowTrackEditorView += ShowTrackEditorView;
            this.mainView.ShowTemplateEditorView += ShowTemplateEditorView;
            this.mainView.ShowHarmonizerView += ShowHarmonizerView;

            //MENU STRIP
            //FILE
            this.mainView.OpenFiles += OpenFiles;
            this.mainView.OpenDirectory += OpenDirectory;
            this.mainView.CreatePlaylist += CreatePlaylist;
            this.mainView.LoadPlaylist += LoadPlaylist;
            this.mainView.RenamePlaylist += RenamePlaylist;
            this.mainView.DeletePlaylist += DeletePlaylist;
            this.mainView.Preferences += Preferences;
            this.mainView.Exit += Exit;
            //EDIT
            this.mainView.RemoveMissingTracks += RemoveMissingTracks;
            this.mainView.RemoveDuplicatedTracks += RemoveDuplicatedTracks;
            this.mainView.OrderByTitle += OrderByTitle;
            this.mainView.OrderByArtist += OrderByArtist;
            this.mainView.OrderByFileName += OrderByFileName;
            this.mainView.Reverse += Reverse;
            this.mainView.Shuffle += Shuffle;
            this.mainView.Clear += Clear;
            //PLAYBACK
            this.mainView.PlayTrack += PlayTrack;
            this.mainView.PauseTrack += PauseTrack;
            this.mainView.StopTrack += StopTrack;
            this.mainView.PrevTrack += PrevTrack;
            this.mainView.NextTrack += NextTrack;
            this.mainView.RandomTrack += RandomTrack;
            //HELP
            this.mainView.About += About;
            
            //kezdooldal
            this.ShowPlaylistView(this, new EventArgs());
        }

        //VIEWS
        private void ShowProfileEditorView(object sender, EventArgs e)
        {
            IProfileEditorView view = ProfileEditorView.GetInstance((MainView)mainView);
            this.profileEditorView = view;
            this.actualView = view;

            IPlaylistDao playlistDao = new PlaylistDao(sqlConnectionString);
            ITrackDao trackDao = new TrackDao(sqlConnectionString);
            ISettingDao settingDao = new SettingDao(sqlConnectionString);

            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);

            new ProfileEditorPresenter(view, playlistDao, trackDao, settingDao);
        }
        private void ShowPlaylistView(object sender, EventArgs e)
        {
            IPlaylistView view = PlaylistView.GetInstance((MainView)mainView);
            this.playlistView = view;
            this.actualView = view;

            IPlaylistDao playlistDao = new PlaylistDao(sqlConnectionString);
            ITrackDao trackDao = new TrackDao(sqlConnectionString);
            ISettingDao settingDao = new SettingDao(sqlConnectionString);

            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);

            new PlaylistPresenter(view, playlistDao, trackDao, settingDao);
        }
        private void ShowTagValueEditorView(object sender, EventArgs e)
        {
            ITagValueEditorView view = TagValueEditorView.GetInstance((MainView)mainView);
            this.tagValueEditorView = view;
            this.actualView = view;

            IPlaylistDao playlistDao = new PlaylistDao(sqlConnectionString);
            ITrackDao trackDao = new TrackDao(sqlConnectionString);
            ISettingDao settingDao = new SettingDao(sqlConnectionString);

            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);

            new TagValueEditorPresenter(view, playlistDao, trackDao, settingDao);
        }
        private void ShowTrackEditorView(object sender, EventArgs e)
        {
            ITrackEditorView view = TrackEditorView.GetInstance((MainView)mainView);
            this.trackEditorView = view;
            this.actualView = view;

            IPlaylistDao playlistDao = new PlaylistDao(sqlConnectionString);
            ITrackDao trackDao = new TrackDao(sqlConnectionString);
            ISettingDao settingDao = new SettingDao(sqlConnectionString);

            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);

            new TrackEditorPresenter(view, playlistDao, trackDao, settingDao);
        }
        private void ShowRuleEditorView(object sender, EventArgs e)
        {
            IRuleEditorView view = RuleEditorView.GetInstance((MainView)mainView);
            this.ruleEditorView = view;
            this.actualView = view;

            IPlaylistDao playlistDao = new PlaylistDao(sqlConnectionString);
            ITrackDao trackDao = new TrackDao(sqlConnectionString);
            ISettingDao settingDao = new SettingDao(sqlConnectionString);

            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);

            new RuleEditorPresenter(view, playlistDao, trackDao, settingDao);
        }
        private void ShowTemplateEditorView(object sender, EventArgs e)
        {
            ITemplateEditorView view = TemplateEditorView.GetInstance((MainView)mainView);
            this.templateEditorView = view;
            this.actualView = view;

            IPlaylistDao playlistDao = new PlaylistDao(sqlConnectionString);
            ITrackDao trackDao = new TrackDao(sqlConnectionString);
            ISettingDao settingDao = new SettingDao(sqlConnectionString);

            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);

            new TemplateEditorPresenter(view, playlistDao, trackDao, settingDao);
        }
        private void ShowHarmonizerView(object sender, EventArgs e)
        {
            IHarmonizerView view = HarmonizerView.GetInstance((MainView)mainView);
            this.harmonizerView = view;
            this.actualView = view;

            IPlaylistDao playlistDao = new PlaylistDao(sqlConnectionString);
            ITrackDao trackDao = new TrackDao(sqlConnectionString);
            ISettingDao settingDao = new SettingDao(sqlConnectionString);

            ((MainView)mainView).SetMenuStripAccessibility(this.actualView);

            new HarmonizerPresenter(view, playlistDao, trackDao, settingDao);
        }

        //MENU STRIP
        //FILE
        private void OpenFiles(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).OpenFilesExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).OpenFiles();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).OpenFiles();
            }
        }
        private void OpenDirectory(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).OpenDirectoryExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).OpenDirectory();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).OpenDirectory();
            }
        }
        private void CreatePlaylist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).CreatePlaylist();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).CreatePlaylist();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).CreatePlaylist();
            }
        }
        private void LoadPlaylist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).LoadPlaylistExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).LoadPlaylist();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).LoadPlaylist();
            }
        }
        private void RenamePlaylist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).RenamePlaylist();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).RenamePlaylist();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).RenamePlaylist();
            }
        }
        private void DeletePlaylist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).DeletePlaylistExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).DeletePlaylist();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).DeletePlaylist();
            }
        }
        private void Preferences(object sender, EventArgs e)
        {
        }
        private void Exit(object sender, EventArgs e)
        {
        }

        //EDIT
        private void RemoveMissingTracks(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).RemoveMissingTracksExt();
            }
        }
        private void RemoveDuplicatedTracks(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).RemoveDuplicatedTracksExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).RemoveDuplicatedTracks();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).RemoveDuplicatedTracks();
            }
        }
        private void OrderByTitle(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).OrderByTitleExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).OrderByTitle();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).OrderByTitle();
            }
        }
        private void OrderByArtist(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).OrderByArtistExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).OrderByArtist();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).OrderByArtist();
            }
        }
        private void OrderByFileName(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).OrderByFileNameExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).OrderByFileName();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).OrderByFileName();
            }
        }
        private void Shuffle(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).ShuffleExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).Shuffle();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).Shuffle();
            }
        }
        private void Reverse(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).ReverseExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).Reverse();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).Reverse();
            }
        }
        private void Clear(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).ClearExt();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).Clear();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).Clear();
            }
        }

        //PLAYBACK
        private void PlayTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).Play();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).Play();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).Play();
            }
        }
        private void PauseTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).Pause();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).Pause();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).Pause();
            }
        }
        private void StopTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).Stop();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).Stop();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).Stop();
            }
        }
        private void PrevTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).Prev();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).Prev();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).Prev();
            }
        }
        private void NextTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).Next();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).Next();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).Next();
            }
        }
        private void RandomTrack(object sender, EventArgs e)
        {
            if (this.playlistView != null && this.actualView.GetType() == typeof(PlaylistView))
            {
                ((PlaylistView)this.playlistView).Random();
            }
            else if (this.trackEditorView != null && this.actualView.GetType() == typeof(TrackEditorView))
            {
                ((TrackEditorView)this.trackEditorView).Random();
            }
            else if (this.harmonizerView != null && this.actualView.GetType() == typeof(HarmonizerView))
            {
                ((HarmonizerView)this.harmonizerView).Random();
            }
        }

        //HELP
        private void About(object sender, EventArgs e)
        {
        }

    }
}
