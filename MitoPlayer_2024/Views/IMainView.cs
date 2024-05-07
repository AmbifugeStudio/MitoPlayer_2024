using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface IMainView
    {
        event EventHandler ShowPlaylistView;
        event EventHandler RemoveMissingTracks;
        event EventHandler RemoveDuplicatedTracks;
        event EventHandler OpenFiles;
        event EventHandler OpenDirectory;

        event EventHandler PlayTrack;
        event EventHandler PauseTrack;
        event EventHandler StopTrack;
        event EventHandler PrevTrack;
        event EventHandler NextTrack;
        event EventHandler RandomTrack;

        event EventHandler OrderByTitle;
        event EventHandler OrderByArtist;
        event EventHandler OrderByFileName;
        event EventHandler Reverse;
        event EventHandler Shuffle;
        event EventHandler Clear;

        event EventHandler CreatePlaylist;
        event EventHandler LoadPlaylist;
        event EventHandler RenamePlaylist;
        event EventHandler DeletePlaylist;
    }
}
