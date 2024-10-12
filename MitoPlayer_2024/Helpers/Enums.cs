using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public enum Extension
    {
        mp3,
        wav,
        flac,
        ogg
    }
    public enum DefaultName
    {
        Profile,
        Playlist
    }
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
        TrackProperty,
        TrackTagValue
    }
    public enum Settings
    {
        FirstRun,
        Volume,   
        IsMuteEnabled,
        IsShuffleEnabled,
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
        LastGeneratedTagValueId,
        Keys,
        KeysAlter,
        KeyCodes,
        KeyColors,

        ExportPath,
        IsRowNumberChecked,
        IsKeyCodeChecked,
        IsBpmNumberChecked,
        IsTrunkedBpmChecked,
        IsTrunkedArtistChecked,
        IsTrunkedTitleChecked,
        ArtistMinimumCharacter,
        TitleMinimumCharacter,
        LastExportDirectoryPath,

        AutomaticBpmImport,
        AutomaticKeyImport,
        VirtualDjDatabasePath,

        IsTagEditorDisplayed,
        CurrentTagIndexForTracklistColouring,
        PlayTrackAfterOpenFiles,
        IsPlaylistListDisplayed,

        CurrentPlaylistId,
        SelectedRowEditing
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
