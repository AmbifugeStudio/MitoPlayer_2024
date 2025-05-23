﻿using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;

namespace MitoPlayer_2024.Views
{
    public interface IPlaylistView
    {
        //PLAYER
        event EventHandler<Messenger> SetCurrentTrackEvent;
        event EventHandler<Messenger> PlayTrackEvent;
        event EventHandler PauseTrackEvent;
        event EventHandler StopTrackEvent;
        event EventHandler CopyCurrentPlayingTrackToDefaultPlaylistEvent;
        event EventHandler<Messenger> PrevTrackEvent;
        event EventHandler<Messenger> NextTrackEvent;
        event EventHandler RandomTrackEvent;
        event EventHandler<Messenger> ChangeVolumeEvent;

        //TRACKLIST
        event EventHandler<Messenger> OrderByColumnEvent;
        event EventHandler<Messenger> DeleteTracksEvent;
        event EventHandler<Messenger> InternalDragAndDropIntoTracklistEvent;
        event EventHandler<Messenger> InternalDragAndDropIntoPlaylistEvent;
        event EventHandler<Messenger> ExternalDragAndDropIntoTracklistEvent;
        event EventHandler<Messenger> ExternalDragAndDropIntoPlaylistEvent;
        event EventHandler ShowColumnVisibilityEditorEvent;
        event EventHandler ScanKeyAndBpmEvent;

        event EventHandler<Messenger> MoveTracklistRowsEvent;

        event EventHandler JumpBackwardEvent;
         event EventHandler JumpForwardEvent;

        //PLAYLIST
        event EventHandler<Messenger> CreatePlaylist;
        event EventHandler<Messenger> EditPlaylist;
        event EventHandler<Messenger> LoadPlaylistEvent;
        event EventHandler<Messenger> MovePlaylistEvent;
        event EventHandler<Messenger> DeletePlaylistEvent;
        event EventHandler<Messenger> SetQuickListEvent;
        event EventHandler<Messenger> ExportToM3UEvent;
        event EventHandler<Messenger> ExportToTXTEvent;
        event EventHandler<Messenger> ExportToDirectoryEvent;
        event EventHandler<Messenger> MovePlaylistRowEvent;
        event EventHandler DisplayPlaylistListEvent;

        //TAG EDITOR
        event EventHandler DisplayTagEditorEvent;
        event EventHandler<Messenger> SelectTagEvent;
        event EventHandler<Messenger> SetTagValueEvent;
        event EventHandler<Messenger> ClearTagValueEvent;

        event EventHandler<Messenger> ChangeFilterModeEnabled;
        event EventHandler EnableFilterModeEvent;
        event EventHandler EnableSetterModeEvent;
        event EventHandler<Messenger> LoadCoversEvent;
        event EventHandler<Messenger> ChangeOnlyPlayingRowModeEnabled;
        event EventHandler<Messenger> ChangeFilterParametersEvent;
        event EventHandler RemoveTagValueFilter;

        event EventHandler SaveTrackListEvent;

        event EventHandler TrainKeyDetectorEvent;
        event EventHandler<Messenger> DetectKeyEvent;
        event EventHandler<Messenger> AddToKeyDetectorEvent;
        event EventHandler CreateModelEvent;

        event EventHandler OpenModelTrainerEvent;

        event EventHandler LiveStreamAnimationEvent;
        event EventHandler LiveStreamAnimationSettingEvent;
        event EventHandler DisplayCoverImageComponentEvent;

        void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<String, Dictionary<String, Color>> tagValueListList);
        void InitializePlaylistList(DataTableModel model);
        void ReloadPlaylistList(DataTableModel model);
        void InitializeTrackList(DataTableModel model);
        void ReloadTrackList(DataTableModel model);



        void UpdateAfterPlayTrack(int currentTrackIndex, int currentTrackId);
        void UpdateAfterPlayTrackAfterPause();
        
        void UpdateAfterStopTrack();
        void UpdateAfterPauseTrack();

        void SetCurrentTrackColor(int trackIdInPlaylist);
        void UpdateTracklistColor(int trackIdInPlaylist);
        void SetVolume(int volume);
        void SetMuted(bool isMuted);
        void SetKeyAndBpmAnalization(bool showButton);
        void ChangeSaveButtonColor(bool v);
        void ChangeSaveStatus(bool isSaving);

        void UpdateCoverList(ConcurrentQueue<ImageExtension> coverList);
        void DisplayLog(string playlistName, decimal logMessageDisplayTime);
    }
}
