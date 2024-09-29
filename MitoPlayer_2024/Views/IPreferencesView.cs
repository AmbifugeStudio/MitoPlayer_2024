using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface IPreferencesView
    {
        event EventHandler CloseViewWithOkEvent;
        event EventHandler CloseViewWithCancelEvent;
        event EventHandler ClearDatabaseEvent;
        event EventHandler<ListEventArgs> SetAutomaticBpmImportEvent;
        event EventHandler<ListEventArgs> SetAutomaticKeyImportEvent;
        event EventHandler<ListEventArgs> SetVirtualDjDatabasePathEvent;
        event EventHandler<ListEventArgs> SetPlayTrackAfterOpenFilesEvent;

        void SetImportSettings(bool automaticBpmImport, bool automaticKeyImport, String virtualDjDatabasePath, bool playTrackAfterOpenFiles);
    }
}
