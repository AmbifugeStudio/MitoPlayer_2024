using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.IViews
{
    public interface IPlaylistCreatorView
    {
        event EventHandler<Messenger> CreatePlaylistEvent;
        event EventHandler<Messenger> EditPlaylistEvent;
        event EventHandler<Messenger> DeletePlaylistEvent;
        event EventHandler<Messenger> LoadPlaylistEvent;
        event EventHandler<Messenger> LoadSelectedPlaylistEvent;
        event EventHandler<Messenger> SelectPlaylistEvent;
        event EventHandler<Messenger> SelectTrackEvent;

        void DisablePlaylistListSelection();
        void EnablePlaylistListSelection();
        void DisableTracklistSelection();
        void EnableTracklistSelection();
        void DisableSelectorTracklistSelection();
        void EnableSelectorTracklistSelection();
        void InitializePlaylistList(DataTableModel model);
        void UpdatePlaylistList(DataTableModel model);
        void InitializeTracklist(DataTableModel model);
        void UpdateTracklist(DataTableModel model);
        void InitializeSelectorTracklist(DataTableModel model);
        void UpdateSelectorTracklist(DataTableModel model);
    }
}
