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
        //Properties - Fields
        List<PlaylistModel> PlaylistList { get; set; }
        List<TrackModel> TrackList { get; set; }
        bool IsEdit { get; set; }
        bool IsSuccessful { get; set; }
        string Message { get; set; }

        //Events
        event EventHandler OpenFiles;
        event EventHandler OpenDirectory;
        event EventHandler<ListEventArgs> ScanFiles;
        event EventHandler OrderByArtist;
        event EventHandler OrderByTitle;
        event EventHandler OrderByFileName;
        event EventHandler Shuffle;
        event EventHandler Reverse;
        event EventHandler Clear;
        event EventHandler<ListEventArgs> DeleteRows;
        event EventHandler<ListEventArgs> OrderTableByColumn;
        event EventHandler<ListEventArgs> DragAndDrop;

        event EventHandler<ListEventArgs> ShowPlaylistEditorView;
        event EventHandler<ListEventArgs> DeletePlaylist;
        event EventHandler<ListEventArgs> LoadPlaylist;


        /* event EventHandler CreatePlaylist;
         event EventHandler RenamePlaylist;
         event EventHandler ReorderPlaylist;
         event EventHandler AddTrackToPlaylist;
         event EventHandler RemoveTrackFromPlaylist;
         event EventHandler DeletePlaylist;
         event EventHandler ClearPlaylist;*/

        //Methods
        void SetPlaylistListBindingSource(BindingSource playlistList);
        void SetTrackListBindingSource(BindingSource trackList);
        void SetSelectedTrackListBindingSource(BindingSource selectedTrackList);
        void Show();//Optional
    }
}
