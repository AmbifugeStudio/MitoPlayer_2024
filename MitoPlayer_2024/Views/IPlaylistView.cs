using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
        event EventHandler GetMediaPlayerProgressStatusEvent;
        event EventHandler<ListEventArgs> SetCurrentTrackColorEvent;

        //TRACKLIST
        event EventHandler<ListEventArgs> OrderByColumnEvent;
        event EventHandler<ListEventArgs> DeleteTracksEvent;
        event EventHandler<ListEventArgs> InternalDragAndDropIntoTracklistEvent;
        event EventHandler<ListEventArgs> InternalDragAndDropIntoPlaylistEvent;
        event EventHandler<ListEventArgs> ExternalDragAndDropIntoTracklistEvent;
        event EventHandler<ListEventArgs> ExternalDragAndDropIntoPlaylistEvent;
        event EventHandler<ListEventArgs> ChangeTracklistColorEvent;
        event EventHandler ShowColumnVisibilityEditorEvent;
        event EventHandler ScanBpmEvent;
        
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
        event EventHandler DisplayPlaylistListEvent;

        //TAG EDITOR
        event EventHandler DisplayTagEditorEvent;
        event EventHandler<ListEventArgs> SelectTagEvent;
        event EventHandler<ListEventArgs> SetTagValueEvent;
        event EventHandler<ListEventArgs> ClearTagValueEvent;

        void SetPlaylistListBindingSource(BindingSource playlistList, bool[] columnVisibility, int currentPlaylistId);
        void SetTrackListBindingSource(BindingSource trackList, bool[] columnVisibility, int[] columnSortingId, int currentTrackIdInPlaylist);
        void SetSelectedTrackListBindingSource(BindingSource selectedTrackList);
        void UpdateAfterPlayTrack(int currentTrackIndex, int currentTrackId);
        void UpdateAfterPlayTrackAfterPause();
        void UpdateAfterStopTrack();
        void UpdateAfterPauseTrack();
        void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString);
        void SetCurrentTrackColor(int trackIdInPlaylist);
        void SetVolume(int volume);
        void Show();
    }
}
