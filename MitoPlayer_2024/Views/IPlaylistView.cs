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
        List<Playlist> PlaylistList { get; set; }
        List<Track> TrackList { get; set; }

        event EventHandler<ListEventArgs> ShowPlaylistEditorView;
        event EventHandler<ListEventArgs> LoadPlaylist;
        event EventHandler<ListEventArgs> DeletePlaylist;
        event EventHandler<ListEventArgs> OrderByColumn;

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
        void SetVolume(int volume);
        void Show();
    }
}
