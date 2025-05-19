using MitoPlayer_2024.Helpers;
using System;

namespace MitoPlayer_2024.IViews
{

    public interface ILiveStreamAnimationSettingView
    {
        event EventHandler BrowseDirectoryEvent;
        event EventHandler<Messenger> SetImagePathEvent;

        event EventHandler CloseViewWithOkEvent;
        event EventHandler CloseViewWithCancelEvent;

        void InitializeView(Messenger messenger);
    }
}
