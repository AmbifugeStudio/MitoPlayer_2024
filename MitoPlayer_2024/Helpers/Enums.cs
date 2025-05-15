using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public enum SourceTable
    {
        Tracklist,
        Selector,
        FilteredSelector
    }
    public enum TableSourceForMediaPlayer
    {
        MainButton,
        TracklistDoubleClick,
        TracklistKeyDown,
        SelectorDoubleClick,
        SelectorButton
    }
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
        TrackTagValue,
        TrainingData
    }
    public enum Settings
    {
        //GLOBAL SETTINGS (??)
        LastGeneratedPlaylistId,
        LastGeneratedProfileId,
        LastGeneratedTagId,
        LastGeneratedTagValueId,
        CurrentPlaylistId,
        CurrentSelectorPlaylistId,
        //INNER SETTINGS
        LastOpenDirectoryPath,
        LastOpenFilesFilterIndex,
        PlaylistColumnNames,
        PlaylistColumnTypes,
        PlaylistColumnVisibility,
        TrackColumnNames,
        TrackColumnTypes,
        TrackColumnVisibility,
        //SETTING MENU
        AutomaticBpmImport,
        AutomaticKeyImport,
        ImportBpmFromVirtualDj,
        ImportKeyFromVirtualDj,
        PlayTrackAfterOpenFiles,
        PreviewPercentage,
        IsShortTrackColouringEnabled,
        ShortTrackColouringThreshold,
        //PLAYER SETTINGS
        IsShuffleEnabled,
        Volume,
        IsMuteEnabled,
        IsPreviewEnabled,
        //PLAYER FORM VIEW ELEMENT VISIBILITY
        IsTagEditorComponentDisplayed,
        IsOnlyPlayingRowModeEnabled,
        IsPlaylistListDisplayed,
        IsCoverImageComponentDisplayed,
        //EXPORT TO DIRECTORY
        LastExportDirectoryPath,
        IsRowNumberChecked,
        IsKeyCodeChecked,
        IsBpmNumberChecked,
        IsTrunkedBpmChecked,
        IsTrunkedArtistChecked,
        IsTrunkedTitleChecked,
        ArtistMinimumCharacter,
        TitleMinimumCharacter,
        //LIVE STREAM ANIMATION
        LiveStreamAnimationImagePath,
        PreventMusicPlayingWhileStream,
        //BPM AND KEY IMPORT
        Keys,
        KeysAlter,
        KeyCodes,
        KeyColors,
        //LOG
        IsLogMessageEnabled,
        LogMessageDisplayTime,
        //VARIOUS
        TrainingModelBatchCount,
        IsTracklistDetailsDisplayed,

        //SELECTOR
        IsTrackListActive
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
