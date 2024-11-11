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
        event EventHandler<ListEventArgs> SetPreviewPercentageEvent;

        event EventHandler<ListEventArgs> SetShortTrackColouringEvent;
        event EventHandler<ListEventArgs> SetShortTrackColouringThresholdEvent;

        void SetImportSettings(
            bool automaticBpmImport, 
            bool automaticKeyImport,
            String virtualDjDatabasePath, 
            bool playTrackAfterOpenFiles,
            bool hasVirtualDj, 
            int previewPercentage,
            bool isShortTrackColouringEnabled,
            decimal shortTrackColouringThreshold);
    }
}
