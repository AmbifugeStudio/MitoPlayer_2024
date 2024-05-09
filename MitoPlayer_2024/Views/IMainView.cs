using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface IMainView
    {
        //MENUSTRIP
        //FILE
        event EventHandler OpenFiles;
        event EventHandler OpenDirectory;
        event EventHandler CreatePlaylist;
        event EventHandler LoadPlaylist;
        event EventHandler RenamePlaylist;
        event EventHandler DeletePlaylist;
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
        //HELP
        event EventHandler About;

        //OPEN VIEWS
        event EventHandler ShowProfileEditorView;
        event EventHandler ShowPlaylistView;
        event EventHandler ShowTagValueEditorView;
        event EventHandler ShowRuleEditorView;
        event EventHandler ShowTrackEditorView;
        event EventHandler ShowTemplateEditorView;
        event EventHandler ShowHarmonizerView;
    }
}
