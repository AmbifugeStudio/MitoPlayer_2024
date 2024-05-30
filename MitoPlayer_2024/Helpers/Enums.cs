using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public enum ColumnGroup
    {
        PlaylistColumns,
        TracklistColumns
    }
    public enum TableName
    {
        Playlist,
        Track,
        PlaylistContent,
        Tag,
        TagValue,
        Profile,
        Setting,
        TrackProperty
    }
    public enum Settings
    {
        FirstRun,
        Volume,       
        LastOpenFilesFilterIndex,
        LastOpenDirectoryPath,
        LastGeneratedPlaylistId,
        PlaylistColumnNames,
        PlaylistColumnTypes,
        PlaylistColumnVisibility,
        TrackColumnNames,
        TrackColumnTypes,
        TrackColumnVisibility,
        LastGeneratedProfileId,
        LastGeneratedTagId,
        LastGeneratedTagValueId
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
