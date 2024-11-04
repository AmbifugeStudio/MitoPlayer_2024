using Google.Protobuf.WellKnownTypes;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public interface IPlaylistView
    {

        //PLAYER
        event EventHandler<ListEventArgs> SetCurrentTrackEvent;
        event EventHandler<ListEventArgs> PlayTrackEvent;
        event EventHandler PauseTrackEvent;
        event EventHandler StopTrackEvent;
        event EventHandler<ListEventArgs> PrevTrackEvent;
        event EventHandler<ListEventArgs> NextTrackEvent;
        event EventHandler RandomTrackEvent;
        event EventHandler<ListEventArgs> ChangeVolumeEvent;
     //   event EventHandler GetMediaPlayerProgressStatusEvent;

        //TRACKLIST
        event EventHandler<ListEventArgs> OrderByColumnEvent;
        event EventHandler<ListEventArgs> DeleteTracksEvent;
        event EventHandler<ListEventArgs> InternalDragAndDropIntoTracklistEvent;
        event EventHandler<ListEventArgs> InternalDragAndDropIntoPlaylistEvent;
        event EventHandler<ListEventArgs> ExternalDragAndDropIntoTracklistEvent;
        event EventHandler<ListEventArgs> ExternalDragAndDropIntoPlaylistEvent;
       // event EventHandler<ListEventArgs> ChangeTracklistColorEvent;
        event EventHandler ShowColumnVisibilityEditorEvent;
        event EventHandler ScanKeyAndBpmEvent;

        event EventHandler<ListEventArgs> MoveTracklistRowsEvent;

        //PLAYLIST
        event EventHandler<ListEventArgs> CreatePlaylist;
        event EventHandler<ListEventArgs> EditPlaylist;
        event EventHandler<ListEventArgs> LoadPlaylistEvent;
        event EventHandler<ListEventArgs> MovePlaylistEvent;
        event EventHandler<ListEventArgs> DeletePlaylistEvent;
        event EventHandler<ListEventArgs> SetQuickListEvent;
        event EventHandler<ListEventArgs> ExportToM3UEvent;
        event EventHandler<ListEventArgs> ExportToTXTEvent;
        event EventHandler<ListEventArgs> ExportToDirectoryEvent;
        event EventHandler<ListEventArgs> MovePlaylistRowEvent;
        event EventHandler DisplayPlaylistListEvent;

        //TAG EDITOR
        event EventHandler DisplayTagEditorEvent;
        event EventHandler<ListEventArgs> SelectTagEvent;
        event EventHandler<ListEventArgs> SetTagValueEvent;
        event EventHandler<ListEventArgs> ClearTagValueEvent;

        event EventHandler<ListEventArgs> ChangeFilterModeEnabled;
        event EventHandler EnableFilterModeEvent;
        event EventHandler EnableSetterModeEvent;
        event EventHandler<ListEventArgs> LoadCoversEvent;
        event EventHandler<ListEventArgs> ChangeOnlyPlayingRowModeEnabled;
        event EventHandler<ListEventArgs> ChangeFilterParametersEvent;
        event EventHandler RemoveTagValueFilter;

        event EventHandler SaveTrackListEvent;

        event EventHandler TrainKeyDetectorEvent;
        event EventHandler<ListEventArgs> DetectKeyEvent;
        event EventHandler<ListEventArgs> AddToKeyDetectorEvent;
        event EventHandler CreateModelEvent;
        void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<String, Dictionary<String, Color>> tagValueListList);
        void InitializePlaylistList(DataTableModel model);
        void ReloadPlaylistList(DataTableModel model);
        void InitializeTrackList(DataTableModel model);
        void ReloadTrackList(DataTableModel model);



        void UpdateAfterPlayTrack(int currentTrackIndex, int currentTrackId);
        void UpdateAfterPlayTrackAfterPause();
        
        void UpdateAfterStopTrack();
        void UpdateAfterPauseTrack();
       // void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString);



        void SetCurrentTrackColor(int trackIdInPlaylist);
        void UpdateTracklistColor(int trackIdInPlaylist);
        void SetVolume(int volume);
        void SetMuted(bool isMuted);
        void SetKeyAndBpmAnalization(bool showButton);
        void ChangeSaveButtonColor(bool v);
        void ChangeSaveStatus(bool isSaving);

        void UpdateCoverList(ConcurrentQueue<ImageExtension> coverList);
        void ToggleTracklistSelectionChanged(bool isEnable);
    }
}
