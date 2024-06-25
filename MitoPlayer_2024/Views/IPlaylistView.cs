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
        event EventHandler ShowColumnVisibilityEditorEvent;
        
        //PLAYLIST
        event EventHandler<ListEventArgs> ShowPlaylistEditorViewEvent;
        event EventHandler<ListEventArgs> LoadPlaylistEvent;
        event EventHandler<ListEventArgs> DeletePlaylistEvent;
        event EventHandler<ListEventArgs> SetQuickListEvent;
        event EventHandler<ListEventArgs> ExportToM3UEvent;
        event EventHandler<ListEventArgs> ExportToTXTEvent;

        //TAG EDITOR
        event EventHandler DisplayTagEditorEvent;
        event EventHandler<ListEventArgs> SelectTagEvent;
        event EventHandler<ListEventArgs> SetTagValueEvent;

        void SetPlaylistListBindingSource(BindingSource playlistList, bool[] columnVisibility, int currentPlaylistId);
        void SetTrackListBindingSource(BindingSource trackList, bool[] columnVisibility, int[] columnSortingId);
        void SetSelectedTrackListBindingSource(BindingSource selectedTrackList);
        void UpdateAfterPlayTrack(int currentTrackIndex);
        void UpdateAfterPlayTrackAfterPause();
        void UpdateAfterStopTrack();
        void UpdateAfterPauseTrack();
        void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString);
        void SetCurrentTrackColor(int trackIdInPlaylist);
        void SetVolume(int volume);
        void Show();
    }
}
