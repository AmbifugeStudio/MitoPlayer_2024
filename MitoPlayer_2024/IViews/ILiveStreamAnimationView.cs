using MitoPlayer_2024.Helpers;
using System;

namespace MitoPlayer_2024.IViews
{

    public interface ILiveStreamAnimationView
    {
        event EventHandler TrackChangeCheckEvent;
        event EventHandler CloseViewEvent;
        void FirstInitialization(Messenger msg);
        void InitializeView(Messenger msg);
       

    }
}
