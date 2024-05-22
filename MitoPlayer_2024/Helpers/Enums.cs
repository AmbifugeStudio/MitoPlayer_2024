using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public enum Settings
    {
        Volume,
        DefaultPlaylistName,
        CurrentPlaylistId,
        LastOpenFilesFilterIndex,
        LastOpenDirectoryPath,
        LastGeneratedPlaylistId,
        PlaylistColumnNames,
        PlaylistColumnTypes,
        PlaylistColumnVisibility,
        TrackColumnNames,
        TrackColumnTypes,
        TrackColumnVisibility,
        DefaultProfileName,
        CurrentProfileId
    }
    public enum ObjectType
    {
        Playlist,
        Tracklist
    }

    public enum MediaPlayerUpdateState
    {
        Undefined,
        AfterPlay,
        AfterPlayAfterPause,
        AfterPause,
        AfterStop
    }

}
