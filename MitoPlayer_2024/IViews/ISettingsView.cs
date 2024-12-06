using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface ISettingsView
    {
        event EventHandler CloseViewWithOkEvent;
        event EventHandler CloseViewWithCancelEvent;
        event EventHandler ClearDatabaseEvent;
        event EventHandler<Messenger> SetAutomaticBpmImportEvent;
        event EventHandler<Messenger> SetAutomaticKeyImportEvent;
        event EventHandler<Messenger> SetVirtualDjDatabasePathEvent;
        event EventHandler<Messenger> SetPlayTrackAfterOpenFilesEvent;
        event EventHandler<Messenger> SetPreviewPercentageEvent;

        event EventHandler<Messenger> SetShortTrackColouringEvent;
        event EventHandler<Messenger> SetShortTrackColouringThresholdEvent;
        event EventHandler<Messenger> SetImportBpmFromVirtualDjEvent;
        event EventHandler<Messenger> SetImportKeyFromVirtualDjEvent;

        void InitializeSettings(Messenger msg);
    }
}
