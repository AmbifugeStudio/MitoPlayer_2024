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
        event EventHandler<ListEventArgs> ChangeProgressEvent;
        event EventHandler GetMediaPlayerProgressStatusEvent;

        //TRACKLIST
        event EventHandler<ListEventArgs> OrderByColumnEvent;
        event EventHandler<ListEventArgs> DeleteTracksEvent;

        //PLAYLIST
        event EventHandler<ListEventArgs> ShowPlaylistEditorViewEvent;
        event EventHandler<ListEventArgs> LoadPlaylistEvent;
        event EventHandler<ListEventArgs> DeletePlaylistEvent;
        event EventHandler<ListEventArgs> SetQuickListEvent;

       // event EventHandler<ListEventArgs> ScanFiles;
        
       // event EventHandler<ListEventArgs> PlayTrack;
       // event EventHandler StopTrack;

        /*event EventHandler OpenFiles;
        event EventHandler OpenDirectory;
        event EventHandler<ListEventArgs> ScanFiles;
        event EventHandler<ListEventArgs> OrderTableByColumn;
        
        event EventHandler RemoveMissingTracks;
        event EventHandler RemoveDuplicatedTracks;
        
        event EventHandler OrderByArtist;
        event EventHandler OrderByTitle;
        event EventHandler OrderByFileName;
        event EventHandler Shuffle;
        event EventHandler Reverse;
        event EventHandler Clear;

        event EventHandler<ListEventArgs> DeleteTracks;
       
        event EventHandler<ListEventArgs> TrackDragAndDrop;

        

        event EventHandler<ListEventArgs> ChangeVolume;*/

        void SetPlaylistListBindingSource(BindingSource playlistList, bool[] columnVisibility, int currentPlaylistId);
        void SetTrackListBindingSource(BindingSource trackList, bool[] columnVisibility);
        void SetSelectedTrackListBindingSource(BindingSource selectedTrackList);
        void UpdateAfterPlayTrack(int currentTrackIndex);
        void UpdateAfterPlayTrackAfterPause();
        void UpdateAfterStopTrack();
        void UpdateAfterPauseTrack();
        void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString);
        void SetVolume(int volume);
        void Show();
    }
}
