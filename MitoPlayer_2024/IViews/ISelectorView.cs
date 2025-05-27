using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.IViews
{
    public interface ISelectorView
    {
        //PLAYER
        event EventHandler<Messenger> SetCurrentTrackEvent;
        event EventHandler<Messenger> PlayTrackEvent;
        event EventHandler PauseTrackEvent;
        event EventHandler StopTrackEvent;

        //TRACKLIST
        event EventHandler<Messenger> SetTrackListToActive;
        event EventHandler<Messenger> OrderByColumnEvent;
        event EventHandler<Messenger> DeleteTracksEvent;
        event EventHandler<Messenger> InternalDragAndDropIntoTracklistEvent;
        event EventHandler<Messenger> ExternalDragAndDropIntoTracklistEvent;
        event EventHandler<Messenger> MoveTracklistRowsEvent;
        event EventHandler SaveTrackListEvent;

        //SELECTOR
        event EventHandler<Messenger> SetSelectorToActive;
        event EventHandler<Messenger> OrderSelectorByColumnEvent;
        event EventHandler<Messenger> InternalDragAndDropIntoSelectorTracklistEvent;
        event EventHandler<Messenger> ExternalDragAndDropIntoSelectorTracklistEvent;
        event EventHandler<Messenger> MoveSelectorTracklistRowsEvent;
        event EventHandler<Messenger> ChangePlaylistSource;
        event EventHandler<Messenger> ChangeBestFit;
        event EventHandler<Messenger> ChangeResultSize;
        event EventHandler<Messenger> ChangeTrackMoveMode;
        event EventHandler SubtractTracksFromPlaylist;

        //PLAYLIST
        event EventHandler<Messenger> CreatePlaylist;
        event EventHandler<Messenger> EditPlaylist;
        event EventHandler<Messenger> LoadPlaylistEvent;
        event EventHandler<Messenger> LoadPlaylistIntoTracklistEvent;
        event EventHandler<Messenger> LoadPlaylistIntoSelectorEvent;
        event EventHandler<Messenger> MovePlaylistEvent;
        event EventHandler<Messenger> DeletePlaylistEvent;
        event EventHandler<Messenger> ExportToM3UEvent;
        event EventHandler<Messenger> ExportToTXTEvent;
        event EventHandler<Messenger> ExportToDirectoryEvent;
        event EventHandler<Messenger> MovePlaylistRowEvent;
        event EventHandler DisplayPlaylistListEvent;
        event EventHandler<Messenger> InternalDragAndDropIntoPlaylistEvent;
        event EventHandler<Messenger> ExternalDragAndDropIntoPlaylistEvent;

        //TAG EDITOR
        event EventHandler<Messenger> SetTagValueEvent;
        event EventHandler<Messenger> ClearTagValueEvent;
        event EventHandler EnableFilterModeEvent;
        event EventHandler<Messenger> ChangeFilterParametersEvent;
        event EventHandler RemoveTagValueFilter;

        void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<String, Dictionary<String, Color>> tagValueListList);
        void InitializePlaylistList(DataTableModel model);
        void ReloadPlaylistList(DataTableModel model);
        void InitializeTrackList(DataTableModel model);
        void InitializeSelectorTrackList(DataTableModel model);
        void ReloadTrackList(DataTableModel model);
        void UpdateAfterPlayTrack(int currentTrackIndex, int currentTrackId, SourceTable sourceList);
        void UpdateAfterPlayTrackAfterPause();
        void UpdateAfterStopTrack();
        void UpdateAfterPauseTrack();
        void SetCurrentTrackColor(int trackIdInPlaylist);
        void UpdateTracklistColor(int trackIdInPlaylist);
        void SetVolume(int volume);
        void SetMuted(bool isMuted);
        void ChangeSaveButtonColor(bool v);
        void ChangeSaveStatus(bool isSaving);
        void DisplayLog(string playlistName, decimal logMessageDisplayTime);
    }
}
