using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface IMainView
    {
        //OPEN VIEWS
        event EventHandler ShowProfileEditorView;
        event EventHandler ShowPlaylistView;
        event EventHandler ShowTagValueEditorView;
        event EventHandler ShowRuleEditorView;
        event EventHandler ShowTrackEditorView;
        event EventHandler ShowTemplateEditorView;
        event EventHandler ShowHarmonizerView;
        event EventHandler ShowPreferencesView;
        event EventHandler ShowAboutView;

        //MENUSTRIP
        //FILE
        event EventHandler OpenFiles;
        event EventHandler OpenDirectory;
        event EventHandler CreatePlaylist;
        event EventHandler LoadPlaylist;
        event EventHandler RenamePlaylist;
        event EventHandler DeletePlaylist;
        event EventHandler ExportToM3U;
        event EventHandler ExportToTXT;
        event EventHandler ExportToDirectory;
        event EventHandler Preferences;
        event EventHandler Exit;
        //EDIT
        event EventHandler RemoveMissingTracks;
        event EventHandler RemoveDuplicatedTracks;
        event EventHandler OrderByTitle;
        event EventHandler OrderByArtist;
        event EventHandler OrderByFileName;
        event EventHandler Reverse;
        event EventHandler Shuffle;
        event EventHandler Clear;
        //PLAYBACK
        event EventHandler PlayTrack;
        event EventHandler PauseTrack;
        event EventHandler StopTrack;
        event EventHandler PrevTrack;
        event EventHandler NextTrack;
        event EventHandler RandomTrack;
        event EventHandler<ListEventArgs> ChangeProgress;
        event EventHandler<ListEventArgs> ChangeVolume;
        event EventHandler<ListEventArgs> ChangeShuffle;
        event EventHandler<ListEventArgs> ChangeMute;

        event EventHandler GetMediaPlayerProgressStatusEvent;
        //HELP
        event EventHandler About;

        event EventHandler<ListEventArgs> ScanFiles;

        void UpdateAfterPlayTrack(String artist);
        void UpdateAfterPlayTrackAfterPause();
        void UpdateAfterStopTrack();
        void UpdateAfterPauseTrack();
        void InitializeMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString);
        void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString);
        void ResetMediaPlayerProgressStatus();

    }
}
